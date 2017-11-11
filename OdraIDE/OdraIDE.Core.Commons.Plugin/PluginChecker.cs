using System.Collections.Generic;
using System.IO;
using OdraIDE.Core.Exceptions;

namespace OdraIDE.Core
{
    public class PluginChecker
    {
        public static string LIB_DIR = Directory.GetCurrentDirectory() + @"\Lib";
        public static string EXT_DIR = Directory.GetCurrentDirectory() + @"\Plugins";

        private ISet<Plugin> plugins = new HashSet<Plugin>();
        static readonly PluginChecker instance = new PluginChecker();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PluginChecker() {}
        PluginChecker() {}

        public static PluginChecker Instance
        {
            get
            {
                return instance;
            }
        }

        public void AddPlugin(Plugin plugin)
        {
            plugins.Add(plugin);
        }

        public void RemovePlugin(Plugin plugin)
        {
            plugins.Remove(plugin);
        }

        public IList<Plugin> GetExtPlugins()
        {
            IList<Plugin> extPlugins = new List<Plugin>();

            foreach (var p in plugins)
            {
                if (p.Directory == EXT_DIR)
                {
                    extPlugins.Add(p);
                }
            }
            return extPlugins;
        }

        public IDictionary<string, IList<string>> ReadAndCheckPlugins()
        {
            IList<string> dirs = new List<string>();
            dirs.Add(LIB_DIR);
            dirs.Add(EXT_DIR);

            IDictionary<string, IList<string>> dlls = new Dictionary<string, IList<string>>();

            foreach (string dir in dirs)
            {
                foreach (string file in Directory.GetFiles(dir, "*.plugin", SearchOption.TopDirectoryOnly))
                {
                    Plugin plugin = Plugin.Read(file);
                    CheckPlugin(plugin);
                    plugin.Directory = dir;
                    AddPlugin(plugin);              
                }
                dlls[dir] = new List<string>();
            }

            //checking dependencies
            foreach (Plugin plugin in plugins)
            {
                CheckPluginDependecies(plugin);
                if (plugin.Enabled)
                {
                    plugin.Loaded = true;
                    dlls[plugin.Directory].Add(plugin.Config.File);
                }
                else
                {
                    plugin.Loaded = false;
                }
            }
            return dlls;
        }

        /// <summary>
        /// Check plugin
        /// </summary>
        /// <param name="plugin">Plugin to check</param>
        /// <returns>True if ok</returns>
        public bool CheckPlugin(Plugin plugin)
        {
            if (plugins.Contains(plugin))
            {
                InvalidPluginException ex = new InvalidPluginException("There is multiple plugin [" + plugin + "]");
                ex.Plugin = plugin.Config;
                throw ex;
            }
            return true;
        }

        public bool ContainsPlugin(PluginConfig pluginXML, out Plugin plugin)
        {
            foreach (Plugin p in plugins)
            {
                if (p.Config.Equals(pluginXML))
                {
                    plugin = p;
                    return true;
                }
            }
            plugin = null;
            return false;
        }

        /// <summary>
        /// Checks plugin dependecies
        /// </summary>
        /// <param name="plugin">Plugin to check</param>
        /// <returns>True if ok</returns>
        public bool CheckPluginDependecies(Plugin plugin)
        {
            if (plugin.Dependencies != null)
            {
                foreach (PluginConfig dep in plugin.Dependencies)
                {
                    Plugin depPlugin = null;
                    if (!ContainsPlugin(dep, out depPlugin))
                    {
                        string msg = "Plugin [" + plugin.Config + "] definition in XML file is invalid. Dependency plugin [" + dep + "] is missing.";
                        InvalidPluginException ex = new InvalidPluginException(msg);
                        ex.Plugin = plugin.Config;
                        throw ex;
                    } 
                    else if(!depPlugin.Enabled)
                    {
                        string msg = "Plugin [" + plugin.Config + "] definition in XML file is invalid. Dependency plugin [" + dep + "] is disable.";
                        InvalidPluginException ex = new InvalidPluginException(msg);
                        ex.Plugin = plugin.Config;
                        throw ex;
                    }
                }
            }
            return true;
        }
    }
}
