using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using MUTDOD.Common;
using OdraIDE.Core;
using OdraIDE.Core.Services;
using OdraIDE.SolutionExplorer.Connections.CompositionPoints;
using OdraIDE.Utilities;
using Host = OdraIDE.Core.CompositionPoints.Host;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(Workbench.DeleteDatabaseDialog, typeof(DeleteDatabaseDialog))]
    public class DeleteDatabaseDialog : AbstractViewModel
    {
        [Import(Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(Messaging.MessagingService, typeof(IMessagingService))]
        private IMessagingService messagingService { get; set; }

        [Import(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
        private IResultsService resultsService { get; set; }

        public string DatabaseName { get; set; }

        public DeleteDatabaseDialog()
        {
            PropertyChanged += new PropertyChangedEventHandler(RenameDatabaseDialog_PropertyChanged);
        }

        void RenameDatabaseDialog_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool validate = !string.IsNullOrEmpty(DatabaseName);
            m_dirtyCondition.SetCondition(validate);
        }

        public void DeleteDatabase()
        {
            connectionService.DeleteDatabase(new DatabaseInfo{Name = DatabaseName }, DeleteDatabaseNameCompleted);
        }

        private void DeleteDatabaseNameCompleted(ExecuteQueryStatus status, DatabaseInfo dbName, IQueryResult queryResult)
        {
            if (queryResult != null)
                switch (queryResult.QueryResultType)
                {
                    case ResultType.StringResult:
                        resultsService.ShowStringResult(queryResult.StringOutput);
                        break;
                }

            switch (status)
            {
                case ExecuteQueryStatus.Done:
                    messagingService.ShowMessage("Database " + dbName.Name + " name has been deleted.", "Info");
                    break;
                case ExecuteQueryStatus.Canceled:
                    break;
                case ExecuteQueryStatus.Error:
                    messagingService.ShowMessage("Database " + dbName.Name + " name could not by deleted !!!", "Error");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays the New Database Dialog as modal
        /// </summary>
        public void ShowDialog()
        {
            m_dirtyCondition.SetCondition(true);
            String message = "Deleting database " + DatabaseName + ". Are yot shure?";
            MessageBoxResult result = MessageBox.Show(message, "Delete database",
               MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                DeleteDatabase();
            }
        }

        private ICondition dirtyCondition
        {
            get
            {
                return m_dirtyCondition;
            }
        }
        private ConcreteCondition m_dirtyCondition = new ConcreteCondition(false);

    }
}
