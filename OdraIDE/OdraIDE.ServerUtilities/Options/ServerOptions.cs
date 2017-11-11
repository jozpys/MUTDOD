using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.ServerUtilities
{
    [Export(OdraIDE.Core.ExtensionPoints.Options.OptionsDialog.OptionsItems, typeof(IOptionsItem))]
    public class ServerOptions : AbstractOptionsItem, IPartImportsSatisfiedNotification
    {
        public ServerOptions()
        {
            Header = "Server";
        }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Options.ServerItems, typeof(IOptionsItem), AllowRecomposition = true)]
        private IEnumerable<IOptionsItem> items { get; set; }

        [Import(CompositionPoints.Workbench.Options.ServerOptionsPad, typeof(ServerOptionsPad))]
        private ServerOptionsPad pad { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(items);
            Pad = pad;
        }
    }

    [Export(ExtensionPoints.Options.ServerItems, typeof(IOptionsItem))]
    class ServerOptionsGeneral : AbstractOptionsItem, IPartImportsSatisfiedNotification
    {
        public ServerOptionsGeneral()
        {
            Header = "General";
        }

        [Import(CompositionPoints.Workbench.Options.ServerOptionsPad, typeof(ServerOptionsPad))]
        private ServerOptionsPad pad { get; set; }

        public void OnImportsSatisfied()
        {
            Pad = pad;
        }
    }
}
