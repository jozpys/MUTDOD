using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace OdraIDE.ServerUtilities
{
    public class ServerManager
    {
        private static ServerManager m_Instance = new ServerManager();

        private ProcessCaller pcCentralServer;
        private ProcessCaller pcDataServer;

        public event EventHandler CompletedCentralServer;
        public event EventHandler CompletedDataServer;

        public event EventHandler CancelledCentralServer;
        public event EventHandler CancelledDataServer;

        private ServerManager()
        {

        }

        public static ServerManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public void RunCentralServer(string location, DispatcherObject isi)
        {
            pcCentralServer = new ProcessCaller(isi);
            pcCentralServer.FileName = location;
            pcCentralServer.Arguments = "";
            //pcDataServer.StdErrReceived += new DataReceivedHandler(writeStreamInfo);
            //pcDataServer.StdOutReceived += new DataReceivedHandler(writeStreamInfo);
            pcCentralServer.Completed += CompletedCentralServer;
            pcCentralServer.Cancelled += CancelledCentralServer;
            pcCentralServer.Start();
        }

        public void RunDataServer(string location, DispatcherObject isi)
        {
            pcDataServer = new ProcessCaller(isi);
            pcDataServer.FileName = location;
            pcDataServer.Arguments = "";
            //pcDataServer.StdErrReceived += new DataReceivedHandler(writeStreamInfo);
            //pcDataServer.StdOutReceived += new DataReceivedHandler(writeStreamInfo);
            pcDataServer.Completed += CompletedDataServer;
            pcDataServer.Cancelled += CancelledDataServer;
            pcDataServer.Start();
        }

        public void StopCentralServer()
        {
            if (pcCentralServer != null)
            {
                pcCentralServer.Kill();
            }
        }

        public void StopDataServer()
        {
            if (pcDataServer != null)
            {
                pcDataServer.Kill();
            }
        }
    }
}
