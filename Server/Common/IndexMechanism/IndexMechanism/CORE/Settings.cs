using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using MUTDOD.Common;

namespace IndexMechanism.CORE
{
    [Serializable]
    public class Settings<T>
    {
        [XmlElement]
        public string IndexesStorageDirectory
        {
            get { return this._indexesStorageDirectory; }
            set
            {
                bool valueChanged = this._indexesStorageDirectory != value;

                this._indexesStorageDirectory = value;

                if (valueChanged)
                    this.SettingsValueChanged();
            }
        }

        [XmlElement]
        public string IndexesDataStorageDirectory
        {
            get { return this._indexesDataStorageDirectory; }
            set
            {
                bool valueChanged = this._indexesDataStorageDirectory != value;

                this._indexesDataStorageDirectory = value;

                if (valueChanged)
                    this.SettingsValueChanged();
            }
        }

        [XmlElement]
        public int StatisticSaveSecoundsInterval
        {
            get { return this._statisticSavingInterval; }
            set
            {
                bool valueChanged = this._statisticSavingInterval != value;

                this._statisticSavingInterval = value;

                if (valueChanged)
                    this.SettingsValueChanged();
            }
        }

        [XmlElement]
        public int IndexesIdentity
        {
            get { return _indexesIdentity; }
            set
            {
                if (this._indexesIdentityInitialized)
                    throw new Exception("Autoincrement, read only value!");

                this._indexesIdentity = value;
                this._indexesIdentityInitialized = true;
            }
        }

        [XmlIgnore]
        public int NextIndexesIdentity
        {
            get
            {
                int ret = this._indexesIdentity++;
                this.SettingsValueChanged();
                return ret;
            }
        }

        #region methods and private attributes

        private int _statisticSavingInterval;
        private string _indexesStorageDirectory;
        private int _indexesIdentity;
        private bool _indexesIdentityInitialized = false;
        private string _indexesDataStorageDirectory;
        private static Settings<T> _instance = null;


        internal static Settings<T> GetInstance()
        {
            if (_instance == null)
            {
                if (!File.Exists(SettingsXmlPath))
                {
                    (_instance = new Settings<T>()).setDefaultValues();
                }
                else
                {
                    using (TextReader textReader = new StreamReader(SettingsXmlPath))
                    {
                        Settings<T> s = null;
                        try
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof (Settings<T>));
                            s = serializer.Deserialize(textReader) as Settings<T>;
                        }
                        catch (Exception ex)
                        {
                            if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism<T>.GetLoger() != null)
                                MUTDOD.Server.Common.IndexMechanism.IndexMechanism<T>.GetLoger().Log("IndexMechanism",string.Format("Unable to read file with setting\n{0}", ex), MessageLevel.Error);
                        }

                        textReader.Close();
                        _instance = s ?? new Settings<T>();
                    }
                }
            }
            return _instance;
        }

        private Settings()
        {
        }

        private void setDefaultValues()
        {
            this._indexesDataStorageDirectory = String.Format("{0}\\plugins\\Data\\",
                                                              Path.GetDirectoryName(SettingsXmlPath));
            this._indexesStorageDirectory = String.Format("{0}\\plugins\\", Path.GetDirectoryName(SettingsXmlPath));
            this._indexesIdentity = 1;
            this._indexesIdentityInitialized = true;
            this._statisticSavingInterval = 60;

            if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism<T>.GetLoger() != null)
                MUTDOD.Server.Common.IndexMechanism.IndexMechanism<T>.GetLoger().Log("IndexMechanism","Assigned default values", MessageLevel.Info);

            this.SettingsValueChanged();
        }

        private static string SettingsXmlPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return string.Format("{0}\\IndexSettings.xml", Path.GetDirectoryName(path));
            }
        }

        private void SettingsValueChanged()
        {
            try
            {
                using (TextWriter textWriter = new StreamWriter(SettingsXmlPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (Settings<T>));
                    serializer.Serialize(textWriter, _instance);
                    textWriter.Close();
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism<T>.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism<T>.GetLoger().Log("IndexMechanism",string.Format("Unable to save file with setting\n{0}", ex), MessageLevel.Error);
            }
        }

        #endregion
    }
}