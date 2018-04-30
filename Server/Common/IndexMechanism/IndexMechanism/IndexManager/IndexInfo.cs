using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using MUTDOD.Common;

namespace IndexMechanism.IndexManager
{
    [Serializable]
    public class IndexInfo
    {
        public IndexInfo()
        {
            StatisticChanged = false;
            IndexStatistic.StatisticsChangedHandler += new StatisticsChanged(saveAfterStatisticsChanged);
        }

        [XmlIgnore] public bool StatisticChanged;
        private string _settingsXML;

        [XmlElement] public int IndexID;

        [XmlElement] public string IndexFileName;

        [XmlElement] public string IndexClassName;

        [XmlElement] public string IndexName;

        [XmlElement]
        public string IndexSettings
        {
            get { return _settingsXML; }
            set
            {
                _settingsXML = value ?? string.Empty;
                Save(this, _xmlFileLoacation);
            }
        }

        [XmlElement] public IndexStatistics IndexStatistic = new IndexStatistics();

        private string _xmlFileLoacation;

        #region Serialization methods

        internal static bool Save(IndexInfo ixi, string path)
        {
            bool ret = false;

            try
            {
                using (TextWriter textWriter = new StreamWriter(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (IndexInfo));
                    serializer.Serialize(textWriter, ixi);
                    textWriter.Close();
                    ret = true;
                    if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                        MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",string.Format("Saved IndexInfo: {0}", ixi.IndexClassName), MessageLevel.Info);
                    ixi._xmlFileLoacation = path;
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",string.Format("Unable to save IndexInfo\n{0}", ex), MessageLevel.Warning);
            }

            return ret;
        }

        internal static IndexInfo Load(string path)
        {
            IndexInfo ixi = null;

            try
            {
                using (TextReader textReader = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (IndexInfo));
                    ixi = serializer.Deserialize(textReader) as IndexInfo;
                    textReader.Close();
                    ixi._xmlFileLoacation = path;
                    ixi.IndexStatistic.StatisticsChangedHandler += new StatisticsChanged(ixi.saveAfterStatisticsChanged);
                    if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                        MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism", message: "Loaded IndexInfo: " + ixi.IndexClassName, messageLevel: MessageLevel.Info);
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",string.Format("Unable to load IndexInfo\n{0}", ex), MessageLevel.Error);
            }

            return ixi;
        }

        #endregion

        public bool IsValid()
        {
            bool ret = false;

            if (
                !File.Exists(string.Format("{0}\\{1}", Path.GetDirectoryName(this._xmlFileLoacation), this.IndexFileName)))
                return false;

            AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");

            try
            {
                Assembly assembly =
                    tempDomain.Load(
                        AssemblyName.GetAssemblyName(string.Format("{0}\\{1}",
                                                                   Path.GetDirectoryName(this._xmlFileLoacation),
                                                                   this.IndexFileName)));
                // Assembly assembly =
                //     Assembly.LoadFile(string.Format("{0}\\{1}", Path.GetDirectoryName(this._xmlFileLoacation), this.IndexFileName));

                Type[] assemblyTypes;
                try
                {
                    assemblyTypes = assembly.GetTypes();
                }
                catch (Exception ex)
                {
                    if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                        MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",
                            string.Format("Iterating types in index plugin {0} failed\n{1}", this.IndexFileName,ex.Message),
                            MessageLevel.Error);
                    return false;
                }

                var t = assemblyTypes
                    .Where(p => p.IsClass)
                    .Where(p => p.FullName == this.IndexClassName);

                if (t.Count() == 1)
                {
                    Type type = t.Single();
                    IndexPlugin.IIndex<object> i = (IndexPlugin.IIndex<object>) Activator.CreateInstance(type);

                    if (!i.EmptyIndexData.GetType().IsSerializable)
                        throw new Exception("IndexData must be serializable!");

                    ret = true;
                    i.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",
                        string.Format("Unable to read index assembly {0} from file {1}\n{2}", this.IndexClassName,
                                      this.IndexFileName, ex), MessageLevel.Error);
            }

            AppDomain.Unload(tempDomain);

            return ret;
        }

        internal bool Remove()
        {
            bool ret = false;
            try
            {
                File.Delete(this._xmlFileLoacation);
                ret = true;

                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism","Removed IndexInfo XML file", MessageLevel.Info);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",string.Format("Unable to remove IndexInfo XML file\n{0}", ex), MessageLevel.Error);
            }
            return ret;
        }

        private void saveAfterStatisticsChanged()
        {
            StatisticChanged = true;
        }

        internal void saveStatistics()
        {
            StatisticChanged = false;
            Save(this, _xmlFileLoacation);
        }
    }
}