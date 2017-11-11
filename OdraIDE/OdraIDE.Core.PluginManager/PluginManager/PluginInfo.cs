using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core.PluginManager
{
    public class PluginInfo
    {
        public event EventHandler EnabledChanged;

        public PluginInfo(bool enabled)
        {
            m_enabled = enabled;
            Loaded = enabled;
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string Copyright { get; set; }
        public bool Loaded { get; set; }

        private bool m_enabled;

        public bool Enabled 
        {
            get
            {
                return m_enabled;
            }

            set
            {
                if (m_enabled != value)
                {
                    m_enabled = value;
                    EnabledChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
