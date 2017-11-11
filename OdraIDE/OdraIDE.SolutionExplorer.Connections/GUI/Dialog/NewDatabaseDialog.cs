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
    [Export(Workbench.NewDatabaseDialog, typeof(NewDatabaseDialog))]
    public class NewDatabaseDialog : AbstractViewModel
    {
        [Import(Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(Messaging.MessagingService, typeof(IMessagingService))]
        private IMessagingService messagingService { get; set; }

        [Import(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
        private IResultsService resultsService { get; set; }

        public NewDatabaseDialog()
        {
            PropertyChanged += new PropertyChangedEventHandler(NewDatabaseDialog_PropertyChanged);
        }

        void NewDatabaseDialog_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool validate = !string.IsNullOrEmpty(NewDatabaseName);
            m_dirtyCondition.SetCondition(validate);
        }

        public void CreateNewDatabase()
        {
            connectionService.CreateNewDatabase(new DatabaseInfo{Name = NewDatabaseName}, CreateNewDatabasCompleted);
        }

        private void CreateNewDatabasCompleted(ExecuteQueryStatus status, DatabaseInfo dbName, IQueryResult queryResult)
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
                    messagingService.ShowMessage("Database " + dbName.Name + " was created.", "Info");
                    break;
                case ExecuteQueryStatus.Canceled:
                    break;
                case ExecuteQueryStatus.Error:
                    messagingService.ShowMessage("Database " + dbName.Name + " was not created !!!", "Error");
                    break;
                default:
                    break;
            }
        }

        private string m_NewDatabaseName;

        public string NewDatabaseName 
        {
            get
            {
                return m_NewDatabaseName;
            }

            set
            {
                if (value != m_NewDatabaseName)
                {
                    m_NewDatabaseName = value;
                    NotifyPropertyChanged(m_NewDatabaseNameArgs);
                }
            }
        }

        static readonly PropertyChangedEventArgs m_NewDatabaseNameArgs =
            NotifyPropertyChangedHelper.CreateArgs<NewDatabaseDialog>(o => o.NewDatabaseName);

        [Import(Host.MainWindow)]
        private Lazy<Window> mainWindowExport { get; set; }

        /// <summary>
        /// Displays the New Database Dialog as modal
        /// </summary>
        public void ShowDialog()
        {
            Window mainWindow = mainWindowExport.Value;
            Window optionsDialog = new NewDatabaseDialogView();
            optionsDialog.Owner = mainWindow;
            optionsDialog.DataContext = this;
            m_dirtyCondition.SetCondition(false);
            optionsDialog.ShowDialog();
        }

        private ICondition dirtyCondition
        {
            get
            {
                return m_dirtyCondition;
            }
        }
        private ConcreteCondition m_dirtyCondition = new ConcreteCondition(false);

        #region " OK Button "
        public IControl OKButton
        {
            get
            {
                if (m_OKButton == null)
                {
                    m_OKButton = new OkButtonClass(this);
                }
                return m_OKButton;
            }
        }
        private IControl m_OKButton = null;

        private class OkButtonClass : AbstractButton
        {
            public OkButtonClass(NewDatabaseDialog dlg)
            {
                m_NewDatabaseDialog = dlg;
                EnableCondition = dlg.dirtyCondition;
            }

            private NewDatabaseDialog m_NewDatabaseDialog = null;

            protected override void Run()
            {
                m_NewDatabaseDialog.CreateNewDatabase();
                m_NewDatabaseDialog.NewDatabaseName = null;
            }
        }

        #endregion

        #region " Cancel Button "
        public IControl CancelButton
        {
            get
            {
                if (m_CancelButton == null)
                {
                    m_CancelButton = new CancelButtonClass(this);
                }
                return m_CancelButton;
            }
        }
        private IControl m_CancelButton = null;

        private class CancelButtonClass : AbstractButton
        {
            public CancelButtonClass(NewDatabaseDialog dlg)
            {
                m_NewDatabaseDialog = dlg;
            }

            private NewDatabaseDialog m_NewDatabaseDialog = null;

            protected override void Run()
            {
                m_NewDatabaseDialog.NewDatabaseName = null;
            }
        }
        #endregion

    }
}
