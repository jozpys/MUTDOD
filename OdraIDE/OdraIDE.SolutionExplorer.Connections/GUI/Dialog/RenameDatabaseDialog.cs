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
    [Export(Workbench.RenameDatabaseDialog, typeof(RenameDatabaseDialog))]
    public class RenameDatabaseDialog : AbstractViewModel
    {
        [Import(Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(Messaging.MessagingService, typeof(IMessagingService))]
        private IMessagingService messagingService { get; set; }

        [Import(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
        private IResultsService resultsService { get; set; }

        public string DatabaseName { get; set; }

        public RenameDatabaseDialog()
        {
            PropertyChanged += new PropertyChangedEventHandler(RenameDatabaseDialog_PropertyChanged);
        }

        void RenameDatabaseDialog_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool validate = !string.IsNullOrEmpty(RenameDatabaseName);
            m_dirtyCondition.SetCondition(validate);
        }

        public void RenameDatabase()
        {
            connectionService.RenameDatabase(new DatabaseInfo{Name = DatabaseName}, RenameDatabaseName, RenameDatabaseNameCompleted);
        }

        private void RenameDatabaseNameCompleted(ExecuteQueryStatus status, DatabaseInfo dbName, IQueryResult queryResult)
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
                    messagingService.ShowMessage("Database " + dbName.Name + " name has been changed.", "Info");
                    break;
                case ExecuteQueryStatus.Canceled:
                    break;
                case ExecuteQueryStatus.Error:
                    messagingService.ShowMessage("Database " + dbName.Name + " name could not by changed !!!", "Error");
                    break;
                default:
                    break;
            }
        }

        private string m_RenameDatabaseName;

        public string RenameDatabaseName 
        {
            get
            {
                return m_RenameDatabaseName;
            }

            set
            {
                if (value != m_RenameDatabaseName)
                {
                    m_RenameDatabaseName = value;
                    NotifyPropertyChanged(m_NewDatabaseNameArgs);
                }
            }
        }

        static readonly PropertyChangedEventArgs m_NewDatabaseNameArgs =
            NotifyPropertyChangedHelper.CreateArgs<RenameDatabaseDialog>(o => o.RenameDatabaseName);

        [Import(Host.MainWindow)]
        private Lazy<Window> mainWindowExport { get; set; }

        /// <summary>
        /// Displays the New Database Dialog as modal
        /// </summary>
        public void ShowDialog()
        {
            Window mainWindow = mainWindowExport.Value;
            RenameDatabaseDialogView optionsDialog = new RenameDatabaseDialogView();
            optionsDialog.Owner = mainWindow;
            optionsDialog.DataContext = this;
            RenameDatabaseName = DatabaseName;
            m_dirtyCondition.SetCondition(true);
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
            public OkButtonClass(RenameDatabaseDialog dlg)
            {
                m_RenameDatabaseDialog = dlg;
                EnableCondition = dlg.dirtyCondition;
            }

            private RenameDatabaseDialog m_RenameDatabaseDialog = null;

            protected override void Run()
            {
                m_RenameDatabaseDialog.RenameDatabase();
                m_RenameDatabaseDialog.RenameDatabaseName = null;
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
            public CancelButtonClass(RenameDatabaseDialog dlg)
            {
                m_RenameDatabaseDialog = dlg;
            }

            private RenameDatabaseDialog m_RenameDatabaseDialog = null;

            protected override void Run()
            {
                m_RenameDatabaseDialog.RenameDatabaseName = null;
            }
        }
        #endregion

    }
}
