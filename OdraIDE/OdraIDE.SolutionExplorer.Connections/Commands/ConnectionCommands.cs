using System;
using System.ComponentModel.Composition;
using System.Linq;
using MUTDOD.Common;
using OdraIDE.Core;
using OdraIDE.Core.Services;
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

		[Import("CentralServerNode")]
		private ExportFactory<CentralServerNode> csnFactory { get; set; }

		[Import("DataServerNode")]
		private ExportFactory<DataServerNode> dsnFactory { get; set; }

		[Import("DatabasesFolderNode")]
		private ExportFactory<DatabasesFolderNode> dfnFactory { get; set; }

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

					CentralServerNode centralServerNode = csnFactory.CreateExport().Value;
					centralServerNode.Properties = CentralServerProperties.From(systemInfo.CentralServer);

					DatabasesFolderNode databasesFolderNode = dfnFactory.CreateExport().Value;

					connectionService.DatabasesChanged += delegate(object s, EventArgs e)
					{
						databasesFolderNode.Children.Clear();
						foreach (string database in connectionService.Databases)
						{
							DatabaseNode databaseNode = new DatabaseNode(database);
							databasesFolderNode.Children.Add(databaseNode);
						}
					};

			        foreach (var database in systemInfo.Databases.OrderBy(db => db.Name))
			        {
			            DatabaseNode databaseNode = new DatabaseNode(database.Name);
			            if (database.Classes != null)
			                foreach (var @class in database.Classes.OrderBy(c => c.Name))
			                {
			                    var cn = new ClassNode(@class.Name);
			                    foreach (var f in @class.Fields.OrderBy(f => f))
			                    {
			                        var fn = new FieldNode(f);
			                        cn.Children.Add(fn);
			                    }
			                    foreach (var m in @class.Methods.OrderBy(m => m))
			                    {
			                        var mn = new MethodNode(m);
			                        cn.Children.Add(mn);
			                    }
			                    databaseNode.Children.Add(cn);
			                }
			            databasesFolderNode.Children.Add(databaseNode);
			        }

			        centralServerNode.Children.Add(databasesFolderNode);

					DataServersFolderNode dataServersFolderNode = new DataServersFolderNode();
			        foreach (var dataServer in systemInfo.DataServer)
			        {
			            DataServerNode dataServerNode = dsnFactory.CreateExport().Value;
			            dataServerNode.Properties = DataServerProperties.From(dataServer);


			            //DatabasesFolderNode databasesFolderNode2 = dfnFactory.CreateExport().Value;

                        //connectionService.DatabasesChanged += delegate(object s, EventArgs e)
                        //{
                        //    databasesFolderNode2.Children.Clear();
                        //    foreach (string database in connectionService.Databases)
                        //    {
                        //        DatabaseNode databaseNode = new DatabaseNode(database);
                        //        databasesFolderNode2.Children.Add(databaseNode);
                        //    }
                        //};

                        //foreach (string database in databasesList)
                        //{
                        //    DatabaseNode databaseNode = new DatabaseNode(database);
                        //    databasesFolderNode2.Children.Add(databaseNode);
                        //}
			            //dataServerNode.Children.Add(databasesFolderNode2);

			            dataServersFolderNode.Children.Add(dataServerNode);
			        }
			        centralServerNode.Children.Add(dataServersFolderNode);

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
