using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;
using System.Windows;

namespace OdraIDE.Core.PluginManager
{
    /// <summary>
    /// Adds the Options Dialog to the tools menu
    /// </summary>
    [Export(ExtensionPoints.Workbench.MainMenu.ToolsMenu, typeof(IMenuItem))]
    class ToolsMenuPluginManager : AbstractMenuItem
    {
        public ToolsMenuPluginManager()
        {
            ID = Extensions.Workbench.MainMenu.ToolsMenu.PluginManager;
            Header = "Plugin Manager";
            BeforeOrAfter = RelativeDirection.Before;
            InsertRelativeToID = Extensions.Workbench.MainMenu.ToolsMenu.Options;
            SetIconFromBitmap(Resources.Images.PluginManager);
        }

        [Import(CompositionPoints.PluginManager.PluginManagerDialog, typeof(PluginManager))]
        private PluginManager pluginManager { get; set; }

        protected override void Run()
        {
            pluginManager.ShowDialog();
        }
    }
}
