using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.IO;

namespace OdraIDE.Core.Layout
{
    [Export(ExtensionPoints.Host.ShutdownCommands, typeof(IExecutableCommand))]
    public class SaveLayoutCommand : AbstractExtension, IExecutableCommand
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        public ILayoutManager LayoutManager { get; set; }

        public void Run(params object[] args)
        {
            if (Properties.Settings.Default.SaveRestoreLayout)
            {
                FileStream file = new FileStream("AppState.xml", FileMode.Create, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(file);
                LayoutManager.SaveLayout(sw);
                sw.Close();
            }
        }
    }

    [Export(ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand))]
    public class RestoreLayoutCommand : AbstractExtension, IExecutableCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        public ILayoutManager LayoutManager { get; set; }

        public void Run(params object[] args)
        {

        }
        
        public void OnImportsSatisfied()
        {
            //TODO odczyt plikow
            LayoutManager.Loaded += new EventHandler(LayoutManager_Loaded);
        }

        void LayoutManager_Loaded(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.SaveRestoreLayout && File.Exists("AppState.xml"))
            {
                FileStream file = new FileStream("AppState.xml", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                LayoutManager.RestoreLayout(sr);
            }
        }
    }
}
