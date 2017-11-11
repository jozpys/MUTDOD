using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.ServerUtilities
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.ShutdownCommands, typeof(IExecutableCommand))]
    public class ShutdownCommands : AbstractExtension, IExecutableCommand
    {
        public void Run(params object[] args)
        {
            ServerManager.Instance.StopCentralServer();
            ServerManager.Instance.StopDataServer();
        }
    }
}
