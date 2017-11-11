using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;
using System.Windows;

namespace OdraIDE.Core.Options
{
    /// <summary>
    /// Adds the Options Dialog to the tools menu
    /// </summary>
    [Export(ExtensionPoints.Workbench.MainMenu.ToolsMenu, typeof(IMenuItem))]
    class ToolsMenuOptions : AbstractMenuItem
    {
        public ToolsMenuOptions()
        {
            ID = Extensions.Workbench.MainMenu.ToolsMenu.Options;
            Header = Resources.Strings.Workbench_MainMenu_Tools_Options;
            SetIconFromBitmap(Resources.Images.Workbench_MainMenu_ToolsMenu_Options);
        }

        [Import(CompositionPoints.Options.OptionsDialog, typeof(OptionsDialog))]
        private OptionsDialog optionsDialog { get; set; }

        protected override void Run()
        {
            optionsDialog.ShowDialog();
        }
    }
}
