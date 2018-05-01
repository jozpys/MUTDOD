using MUTDOD.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MUTDOD.Server.Common.ServerStats
{
    [Serializable]
    public class Settings
    {
        [XmlElement]
        public string ServerStatisticDataStorageDirectory
        {
            get { return this.serverStatisticDataStorageDirectory; }
            set
            {
                bool valueChanged = this.serverStatisticDataStorageDirectory != value;

                this.serverStatisticDataStorageDirectory = value;

                if (valueChanged)
                    this.SettingsValueChanged();
            }
        }

        #region methods and private attributes

        private string serverStatisticDataStorageDirectory;
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
                            if (ServerSchemaStats.GetLoger() != null)
                                ServerSchemaStats.GetLoger().Log("ServerStats", string.Format("Unable to read file with setting\n{0}", ex), MessageLevel.Error);
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
            this.serverStatisticDataStorageDirectory = String.Format("{0}\\plugins\\Data\\",
                                                                Path.GetDirectoryName(SettingsXmlPath));

            if (ServerSchemaStats.GetLoger() != null)
                ServerSchemaStats.GetLoger().Log("ServerStats", "Assigned default values", MessageLevel.Info);

            this.SettingsValueChanged();
        }

        private static string SettingsXmlPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return string.Format("{0}\\Settings.xml", Path.GetDirectoryName(path));
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
                if (ServerSchemaStats.GetLoger() != null)
                    ServerSchemaStats.GetLoger().Log("ServerStats", string.Format("Unable to save file with setting\n{0}", ex), MessageLevel.Error);
            }
        }

        #endregion
    }
    
}
