using System;
using System.ComponentModel.Composition;
using System.Linq;
using MUTDOD.Common;
using OdraIDE.Core;
using OdraIDE.Core.Services;
using OdraIDE.SolutionExplorer.Connections.Commands;
using OdraIDE.SolutionExplorer.Connections.CompositionPoints;

namespace OdraIDE.SolutionExplorer.Connections
{
	[Export(Workbench.Commands.Connect, typeof(ICustomCommand))]
	public class ConnectCommand : BaseCommand, IPartImportsSatisfiedNotification
	{
		[Import(Connection.ConnectionService, typeof(IConnectionService))]
		private IConnectionService connectionService { get; set; }

		[Import(SolutionExplorer.CompositionPoints.Workbench.Pads.SolutionExplorer, typeof(ISolutionExplorer))]
		private ISolutionExplorer solutionExplorer { get; set; }

		[Import(Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        [Import(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
        private IResultsService resultsService { get; set; }

        [Import(Workbench.TreeLoader, typeof(TreeLoader))]
        private TreeLoader treeLoader { get; set; }

        public ConnectCommand()
		{
			ExecuteCommand += new ExecuteHandler(Connect);
		}

		void Connect()
		{
			applicationStatus.SetStatus("Connecting...", true);
			connectionService.Connect();
			FillSolutionExplorer();
		}

		private void FillSolutionExplorer()
		{
			connectionService.GetSystemInfo(GetSystemInfoCompleted);
		}

		private void GetSystemInfoCompleted(ExecuteQueryStatus status, SystemInfo systemInfo, IQueryResult queryResult)
		{
            if(queryResult!=null)
                switch (queryResult.QueryResultType)
                {
                        case ResultType.StringResult:
                        resultsService.ShowStringResult(queryResult.StringOutput);
                        break;
                }

			switch (status)
			{
				case ExecuteQueryStatus.Done:

                    connectionService.DatabasesChanged += delegate (object s, EventArgs e)
                    {
                        IConnectionService senderService = (IConnectionService)s;
                        solutionExplorer.TreeView.Root = treeLoader.load(senderService.SystemInfo);
                    };

                    CentralServerNode centralServerNode = treeLoader.load(systemInfo);
                    solutionExplorer.TreeView.Root = centralServerNode;
					solutionExplorer.TreeView.ShowRootExpander = true;
					applicationStatus.SetStatus("Connected", false);
					break;
				case ExecuteQueryStatus.Canceled:
					break;
				case ExecuteQueryStatus.Error:
					applicationStatus.SetStatus("Connection failure", false);
					break;
				default:
					break;
			}
		}


		public void OnImportsSatisfied()
		{
			connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
		}

		void connectionService_Disconnected(object sender, EventArgs e)
		{
			try
			{
				solutionExplorer.TreeView.Root = null;
			}
			catch (InvalidOperationException)
			{
				//do nothing
			}
			
		}
	}

	[Export(Workbench.Commands.Disconnect, typeof(ICustomCommand))]
	public class DisconnectCommand : BaseCommand, IPartImportsSatisfiedNotification
	{
		[Import(Connection.ConnectionService, typeof(IConnectionService))]
		private IConnectionService connectionService { get; set; }

		[Import(Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
		private ApplicationStatus applicationStatus { get; set; }

		public DisconnectCommand()
		{
			ExecuteCommand += new ExecuteHandler(Disconnect);
		}

		public void Disconnect()
		{
			applicationStatus.SetStatus("Disconnecting...", true);
			connectionService.Disconnect();
		}

		public void OnImportsSatisfied()
		{
			connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
		}

		void connectionService_Disconnected(object sender, EventArgs e)
		{
			applicationStatus.SetStatus("Disconnected", false);
		}
	}
}
