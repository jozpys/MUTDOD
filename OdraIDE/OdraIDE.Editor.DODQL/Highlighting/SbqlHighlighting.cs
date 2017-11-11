using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Xml;
using System.Reflection;

namespace OdraIDE.Editor.Sbql
{
    [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.Highlighting, typeof(IHighlighting))]
    public class SbqlHighlighting : AbstractExtension, IHighlighting
    {

        public SbqlHighlighting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("OdraIDE.Editor.DODQL.DodqlHighlighting.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    m_Definition = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            
            Title = "DODQL Hightlighting";
            Extenstions = new string[] { ".dodql" };
        }

        #region Title

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
            }
        }

        private string m_Title;

        #endregion

        #region Extensions

        public string[] Extenstions
        {
            get
            {
                return m_Extensions;
            }
            set
            {
                m_Extensions = value;
            }
        }

        private string[] m_Extensions;

        #endregion

        #region Definition

        public IHighlightingDefinition Definition
        {
            get
            {
                return m_Definition;
            }
            set
            {
                m_Definition = value;
            }
        }

        private IHighlightingDefinition m_Definition;

        #endregion

    }
}
