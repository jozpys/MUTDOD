using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;

namespace OdraIDE.ServerUtilities
{
    /// <summary>
    /// Adds the Options Dialog to the tools menu
    /// </summary>
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ToolsMenu, typeof(IMenuItem))]
    class ToolsMenuRunServers : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private ILayoutManager layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.ServerConsolePad, typeof(ServerConsolePad))]
        private ServerConsolePad consolePad { get; set; }

        [Import(CompositionPoints.Workbench.Options.ServerOptionsPad, typeof(ServerOptionsPad))]
        private ServerOptionsPad serverOptionsPad { get; set; }

        [Import(OdraIDE.Core.Services.Messaging.MessagingService, typeof(IMessagingService))]
        private IMessagingService messagingService { get; set; }

        private bool isRunning = false;

        public ToolsMenuRunServers()
        {
            ID = "ServerRunner";
            Header = "Run servers (data and central)...";
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = OdraIDE.Core.Extensions.Workbench.MainMenu.ToolsMenu.Options;
            SetIconFromBitmap(Images.DatabaseProcess);

            ServerManager.Instance.CompletedCentralServer += new EventHandler(Instance_CompletedCentralServer);
            ServerManager.Instance.CompletedDataServer += new EventHandler(Instance_CompletedDataServer);
        }

        void Instance_CompletedDataServer(object sender, EventArgs e)
        {
            consolePad.AttachOutput(DateTime.Now + ": Data Server COMPLETED");
        }

        void Instance_CompletedCentralServer(object sender, EventArgs e)
        {
            consolePad.AttachOutput(DateTime.Now + ": Central Server COMPLETED");
        }
        
        protected override void Run()
        {
            string CentralServerAppPath = serverOptionsPad.CentralServerLocation;
            string DataServerAppPath = serverOptionsPad.DataServerLocation;

            bool centralServerNull = string.IsNullOrEmpty(CentralServerAppPath);
            bool dataServerNull = string.IsNullOrEmpty(DataServerAppPath);

            if (centralServerNull || dataServerNull)
            {
                string msg = "There is no server location. Enter the server location in the options!";
                messagingService.ShowMessage(msg, "Error");
                return;
            }

            layoutManager.ShowPad(consolePad);
            consolePad.Clear();
            
            if (isRunning)
            {
                consolePad.AttachOutput(DateTime.Now + ": Stoping servers...");
                ServerManager.Instance.StopCentralServer();
                consolePad.AttachOutput(DateTime.Now + ": Central server stoped");
                ServerManager.Instance.StopDataServer();
                consolePad.AttachOutput(DateTime.Now + ": Data server stoped");
                Header = "Run servers (data and central)...";
                isRunning = false;
            }
            else
            {
                consolePad.AttachOutput(DateTime.Now + ": Starting servers...");
                ServerManager.Instance.RunCentralServer(CentralServerAppPath, this);
                consolePad.AttachOutput(DateTime.Now + ": Central server started");
                ServerManager.Instance.RunDataServer(DataServerAppPath, this);
                consolePad.AttachOutput(DateTime.Now + ": Data server started");
                Header = "Stop servers (data and central)...";
                isRunning = true;
            }
        }

        private void RunCentralServer()
        {
            // Application path and command line arguments

            string CentralServerAppPath = serverOptionsPad.CentralServerLocation;
            
            ProcessStartInfo CentralServerProcessInfo = new ProcessStartInfo();

            CentralServerProcessInfo.FileName = CentralServerAppPath;

            // These two optional flags ensure that no DOS window
            // appears
            CentralServerProcessInfo.UseShellExecute = false;
            CentralServerProcessInfo.CreateNoWindow = false;
            CentralServerProcessInfo.RedirectStandardOutput = true;
            CentralServerProcessInfo.RedirectStandardError = true;
            //CentralServerProcessInfo.RedirectStandardInput = true;
            //CentralServerProcessInfo.WindowStyle = ProcessWindowStyle.Minimized;
            try
            {
                Process p1 = Process.Start(CentralServerProcessInfo);
                //p1.OutputDataReceived += new DataReceivedEventHandler(CentralServerProcess_OutputDataReceived);
                p1.BeginOutputReadLine();
                p1.WaitForExit();
            }
            catch (Exception)
            {
                
                //throw;
            }

            // Now read the output of the DOS application
            //string Result = CentralServerProcess.StandardOutput.ReadToEnd();
        }

        private void RunDataServer()
        {
            // Application path and command line arguments
            string DataServerAppPath = serverOptionsPad.DataServerLocation;

            Process DataServerProcess = new Process();

            DataServerProcess.StartInfo.FileName = DataServerAppPath;

            // These two optional flags ensure that no DOS window
            // appears
            DataServerProcess.StartInfo.UseShellExecute = false;
            DataServerProcess.StartInfo.CreateNoWindow = true;
            //DataServerProcess.StartInfo.RedirectStandardOutput = true;
            //DataServerProcess.StartInfo.RedirectStandardError = true;
            //DataServerProcess.OutputDataReceived += new DataReceivedEventHandler(CentralServerProcess_OutputDataReceived);
            // Start the process
            DataServerProcess.Start();
            //DataServerProcess.BeginOutputReadLine();
            //consolePad.AttachOutput(DateTime.Now + ": " + );
        }

        //void CentralServerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    string strMessage = e.Data;
        //    if (!string.IsNullOrEmpty(strMessage))
        //    {
        //        consolePad.AttachOutput(DateTime.Now + ": " + e.Data);
        //    }

        //}
    }
}
