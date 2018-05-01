using System;
using System.IO;
using System.Xml;
using OdraIDE.Core.Exceptions;
using Xstream.Core;

namespace OdraIDE.Core
{
    public class PluginConfig
    {
        /// <summary>
        /// Plugin's identifier. Required
        /// </summary>
        public string Id;

        /// <summary>
        /// Plugin's file path. Required
        /// </summary>
        public string File;

        /// <summary>
        /// Plugin's version. Required
        /// </summary>
        public string Version;

        public string Copyright;

        public string Name;
        public string Company;
        public string Description;
        public bool Enabled = true;
        public PluginConfig[] Dependencies;

        public bool Valid(bool isDependency = false)
        {
            if (string.IsNullOrEmpty(Id))
            {
                string msg = "Plugin definition in XML file is invalid. There is no Id";
                if (isDependency) msg += " for dependency";
                InvalidPluginException ex = new InvalidPluginException(msg);
                ex.Plugin = this;
                throw ex;
            }
            if (string.IsNullOrEmpty(Version))
            {
                string msg = "Plugin definition in XML file is invalid. There is no Version";
                if (isDependency) msg += " for dependency";
                InvalidPluginException ex = new InvalidPluginException(msg);
                ex.Plugin = this;
                throw ex;
            }

            if (!isDependency && string.IsNullOrEmpty(File))
            {
                InvalidPluginException ex = new InvalidPluginException("Plugin definition in XML file is invalid. There is no File");
                ex.Plugin = this;
                throw ex;
            }

            if (Dependencies != null)
            {
                foreach (PluginConfig plugin in Dependencies)
                {
                    if (!plugin.Valid(true)) return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (!(obj is PluginConfig)) return false;

            PluginConfig plugin = obj as PluginConfig;

            return string.Compare(this.Id, plugin.Id) == 0 && string.Compare(this.Version, plugin.Version) == 0;
        }

        public override int GetHashCode()
        {
            int result = 37;

            if (Id != null)
            {
                result = 31 * result + Id.GetHashCode();
            }
            if (Version != null)
            {
                result = 31 * result + Version.GetHashCode();
            }

            return result;
        }

        public override string ToString()
        {
            return "Id: " + Id + ", Version: " + Version;
        }
    }

    public class Plugin
    {
        public event EventHandler EnabledChanged;
        private string Xml;

        public PluginConfig Config
        {
            get;
            private set;
        }

        public Plugin(PluginConfig config)
        {
            Config = config;
        }

        public static Plugin Read(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch (Exception)
            {
                InvalidPluginException e = new InvalidPluginException("Error while parsing plugin file: " + fileName);
                throw e;
            }            
            XmlNode pluginNode = doc.GetElementsByTagName("Plugin").Item(0);
            if (pluginNode == null) 
            {
                InvalidPluginException e = new InvalidPluginException("There is no plugin definition in file: " + fileName);
                throw e;
            }

            XStream xs = new XStream();
            xs.Alias("Plugin", typeof(PluginConfig));
            PluginConfig pluginXML = null;
            try
            {
                pluginXML = xs.FromXml(pluginNode.OuterXml) as PluginConfig;
            }
            catch (Exception ex)
            {
                InvalidPluginException e = new InvalidPluginException("Wrong plugin definition in file: " + fileName+ex.Message);
                throw e;
                Console.WriteLine(e.StackTrace);
            }
                
            try
            {
                pluginXML.Valid();
            }
            catch (InvalidPluginException ex)
            {
                InvalidPluginException e = new InvalidPluginException(ex.Message + " in file: " + fileName + ex.StackTrace, ex);
                e.Plugin = pluginXML;
                throw e;
            }

            Plugin plugin = new Plugin(pluginXML);
            plugin.Xml = pluginNode.OuterXml;
            return plugin;
        }

        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Xml);
            XmlNode enabledNode = doc.GetElementsByTagName("Enabled").Item(0);
            if (enabledNode == null)
            {
                enabledNode = doc.CreateElement("Enabled");
                XmlNode textNode = doc.CreateTextNode(Enabled ? "True" : "False");
                enabledNode.AppendChild(textNode);
                doc.FirstChild.AppendChild(enabledNode);
            }
            else
            {
                enabledNode.ChildNodes.Item(0).Value = Enabled ? "True" : "False";
            }
            /*
             * Save to file 
             */
            StreamWriter sw = new StreamWriter(Path.Combine(Directory, Config.File + ".plugin"));
            doc.Save(sw);
            sw.Close();
            Xml = doc.OuterXml;
        }

        #region FOR XAML

        public PluginConfig[] Dependencies
        {
            get { return Config.Dependencies; }
        }

        public string File
        {
            get { return Config.File; }
        }

        public string Name
        {
            get { return Config.Name; }
        }

        public string Version
        {
            get { return Config.Version; }
        }

        public string Company
        {
            get { return Config.Company; }
        }

        public string Copyright
        {
            get { return Config.Copyright; }
        }

        public string Description
        {
            get { return Config.Description; }
        }

        public bool Enabled
        {
            get
            {
                return Config.Enabled;
            }

            set
            {
                if (Config.Enabled != value)
                {
                    Config.Enabled = value;
                    Save();
                    EnabledChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        public string Directory
        {
            get;
            set;
        }

        public bool Loaded
        {
            get;
            set;
        }            

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (!(obj is Plugin)) return false;

            Plugin plugin = obj as Plugin;

            return string.Compare(this.Config.Id, plugin.Config.Id) == 0 && string.Compare(this.Version, plugin.Version) == 0;
        }

        public override int GetHashCode()
        {
            int result = 37;

            if (Config.Id != null)
            {
                result = 31 * result + Config.Id.GetHashCode(); 
            }
            if (Version != null)
            {
                result = 31 * result + Version.GetHashCode();
            }

            return result;
        }

        public override string ToString()
        {
            return Config.ToString();
        }

        public string FilePath(string dir)
        {
            return Path.Combine(dir, Config.File + ".dll");
        }
    }
}
