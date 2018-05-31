using MUTDOD.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MUTDOD.Server.Common.QueryOptimizer.Settings
{
    [Serializable]
    public class Settings
    {
        [XmlElement]
        public long MaxTimeSearchingBestQueryPlan
        {
            get { return this.maxTimeSearchingBestQueryPlan; }
            set
            {
                bool valueChanged = this.maxTimeSearchingBestQueryPlan != value;


                if (valueChanged)
                    this.SettingsValueChanged();
                this.maxTimeSearchingBestQueryPlan = value;
            }
        }

        #region methods and private attributes

        private long maxTimeSearchingBestQueryPlan;
        private static Settings instance = null;


        internal static Settings GetInstance()
        {
            if (instance == null)
            {
                if (!File.Exists(SettingsXmlPath))
                {
                    (instance = new Settings()).setDefaultValues();
                }
                else
                {
                    using (TextReader textReader = new StreamReader(SettingsXmlPath))
                    {
                        Settings s = null;
                        try
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                            s = serializer.Deserialize(textReader) as Settings;
                        }
                        catch (Exception ex)
                        {
                            if (QueryOptimizer.GetLoger() != null)
                                QueryOptimizer.GetLoger().Log("QueryOptimizer", string.Format("Unable to read file with setting\n{0}", ex), MessageLevel.Error);
                        }

                        textReader.Close();
                        instance = s ?? new Settings();
                    }
                }
            }
            return instance;
        }

        private Settings()
        {
        }

        private void setDefaultValues()
        {
            this.maxTimeSearchingBestQueryPlan = 500000;
            if (QueryOptimizer.GetLoger() != null)
                QueryOptimizer.GetLoger().Log("QueryOptimizer", "Assigned default values", MessageLevel.Info);

            this.SettingsValueChanged();
        }

        private static string SettingsXmlPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return string.Format("{0}\\QueryOptimizerSettings.xml", Path.GetDirectoryName(path));
            }
        }

        private void SettingsValueChanged()
        {
            try
            {
                using (TextWriter textWriter = new StreamWriter(SettingsXmlPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    serializer.Serialize(textWriter, instance);
                    textWriter.Close();
                }
            }
            catch (Exception ex)
            {
                if (QueryOptimizer.GetLoger() != null)
                    QueryOptimizer.GetLoger().Log("QueryOptimizer", string.Format("Unable to save file with setting\n{0}", ex), MessageLevel.Error);
            }
        }

        #endregion
    }
}
