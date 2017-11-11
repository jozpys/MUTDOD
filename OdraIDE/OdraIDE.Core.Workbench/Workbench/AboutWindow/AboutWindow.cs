using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;

namespace OdraIDE.Core.Workbench
{
    [Export(OdraIDE.Core.CompositionPoints.Help.AboutDialog, typeof(AboutWindow))]
    public class AboutWindow : AbstractExtension
    {

        [Import(OdraIDE.Core.CompositionPoints.Host.MainWindow)]
        private Lazy<Window> mainWindowExport { get; set; }

        public void ShowWindow()
        {
            Window mainWindow = mainWindowExport.Value;
            Window pluginManagerDialog = new AboutWindowView();
            pluginManagerDialog.Owner = mainWindow;
            pluginManagerDialog.DataContext = this;
            pluginManagerDialog.ShowDialog();
        }
    }
}
