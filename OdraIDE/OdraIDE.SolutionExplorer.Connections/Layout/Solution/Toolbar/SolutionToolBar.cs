using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.SolutionExplorer.Connections
{
    [Export(ExtensionPoints.ToolBars.Solution, typeof(IToolBarItem))]
    public class SolutionToolbarConnectItem : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.Commands.Connect, typeof(ICustomCommand))]
        private ICustomCommand command;

        public SolutionToolbarConnectItem()
        {
            ID = "ToolbarConnect";
            ToolTip = "Connect to database...";
            SetIconFromBitmap(Resources.Images.Connect);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }

    [Export(ExtensionPoints.ToolBars.Solution, typeof(IToolBarItem))]
    public class SolutionToolbarDisconnectItem : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.Commands.Disconnect, typeof(ICustomCommand))]
        private ICustomCommand command;

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        public SolutionToolbarDisconnectItem()
        {
            ID = "ToolbarDisconnect";
            ToolTip = "Disconnect";
            InsertRelativeToID = "ToolbarConnect";
            BeforeOrAfter = RelativeDirection.After;
            SetIconFromBitmap(Resources.Images.Disconnect);
            VisibleCondition = new ConcreteCondition(false);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
            connectionService.Connected += new EventHandler(connectionService_Connected);
            connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
        }

        void connectionService_Disconnected(object sender, EventArgs e)
        {
            CheckVisibleCondition();
        }

        void connectionService_Connected(object sender, EventArgs e)
        {
            CheckVisibleCondition();
        }

        void CheckVisibleCondition()
        {
            (VisibleCondition as ConcreteCondition).SetCondition(connectionService.IsConnected);
        }
    }
}
