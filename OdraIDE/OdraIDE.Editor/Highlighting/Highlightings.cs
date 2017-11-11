using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using ICSharpCode.AvalonEdit.Highlighting;

namespace OdraIDE.Editor
{
    [Export(typeof(Highlightings))]
    public class Highlightings : IPartImportsSatisfiedNotification
    {
        #region SyntaxHighlightings

        [ImportMany(ExtensionPoints.SourceEditor.Highlighting, typeof(IHighlighting))]
        private IEnumerable<IHighlighting> highlightings { get; set; }

        private void InitHighlighting()
        {
            foreach (IHighlighting item in highlightings)
            {
                HighlightingManager.Instance.RegisterHighlighting(item.Title, item.Extenstions, item.Definition);
            }
        }

        #endregion

        public void OnImportsSatisfied()
        {
            InitHighlighting();
        }
    }
}
