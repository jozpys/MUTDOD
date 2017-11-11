using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using AvalonDock;

namespace OdraIDE.Results
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand))]
    public class StartupCommand : AbstractExtension, IExecutableCommand
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.StringResultsPad, typeof(StringResultsPad))]
        private Lazy<StringResultsPad> stringResultsPad { get; set; }

        [Import(CompositionPoints.Workbench.Pads.GridResultsPad, typeof(GridResultsPad))]
        private GridResultsPad gridResultsPad { get; set; }

        public void Run(params object[] args)
        {
            layoutManager.Value.ShowPad(stringResultsPad.Value);
            layoutManager.Value.ShowPad(gridResultsPad);
        }
    }
}
