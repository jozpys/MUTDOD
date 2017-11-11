using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor
{
    [Export(OdraIDE.Core.ExtensionPoints.Options.OptionsDialog.OptionsItems, typeof(IOptionsItem))]
    public class SourceEditorOptions : AbstractOptionsItem, IPartImportsSatisfiedNotification
    {
        public SourceEditorOptions()
        {
            //TODO Translate
            Header = "Source Editor";
        }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Options.SourceEditorItems, typeof(IOptionsItem), AllowRecomposition = true)]
        private IEnumerable<IOptionsItem> items { get; set; }

        [Import(CompositionPoints.Workbench.Options.SourceEditorGeneralOptionsPad, typeof(SourceEditorGeneralOptionsPad))]
        private SourceEditorGeneralOptionsPad pad { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(items);
            Pad = pad;
        }
    }

    [Export(ExtensionPoints.Options.SourceEditorItems, typeof(IOptionsItem))]
    class SourceEditorOptionsGeneral : AbstractOptionsItem, IPartImportsSatisfiedNotification
    {
        public SourceEditorOptionsGeneral()
        {
            Header = "General";
        }

        [Import(CompositionPoints.Workbench.Options.SourceEditorGeneralOptionsPad, typeof(SourceEditorGeneralOptionsPad))]
        private SourceEditorGeneralOptionsPad pad { get; set; }

        public void OnImportsSatisfied()
        {
            Pad = pad;
        }
    }
}
