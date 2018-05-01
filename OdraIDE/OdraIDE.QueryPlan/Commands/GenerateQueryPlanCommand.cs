using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdraIDE.Core;
using OdraIDE.QueryPlan.Services;
using System.Windows.Input;
using OdraIDE.Editor.DODQL.Workbench.ToolBar;
using OdraIDE.Editor.Sbql;
using MUTDOD.Common;
using OdraIDE.Editor.Sbql.Commands;
using OdraIDE.Editor;

namespace OdraIDE.QueryPlan.Commands
{
    [Export(QueryPlan.CompositionPoints.Workbench.Commands.GenerateQueryPlan, typeof(Core.ICustomCommand))]
    public class GenerateQueryPlanCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.QueryPlan.QueryPlanService, typeof(IQueryPlanService))]
        private IQueryPlanService queryPlanService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(OdraIDE.Editor.Sbql.CompositionPoints.Workbench.ExecuteToolbar.DatabasesComboBox, typeof(DatabasesToolBarComboBox))]
        private DatabasesToolBarComboBox databasesComboBox { get; set; }

        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        private string m_lastQuery;

        private bool isExecuting = false;

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (F6)
                return new KeyBinding(this, new KeyGesture(Key.F6, ModifierKeys.None));
            }
        }

        public GenerateQueryPlanCommand()
        {
            EnableCondition = new ConcreteCondition(false);
            ExecuteCommand += new ExecuteHandler(GenerateQueryPlan);
        }

        private static FileName m_SelectedFileName;

        private void GenerateQueryPlan()
        {
            OpenedFile file = fileService.Value.GetActiveFile();
            int line;
            FileName filename;
            int column;

            if (file is SbqlOpenedFile)
            {
                string dbName = databasesComboBox.SelectedItem as string;
                SbqlOpenedFile sbqlFile = file as SbqlOpenedFile;
                string query = GetSelectedQuery(out filename, out line, out column);
                if (string.IsNullOrEmpty(query))
                   query = sbqlFile.Query;
                this.m_lastQuery = query;
                if (!string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(query))
                {
                    SetEnableCondition(false);
                    queryPlanService.Clear();
                    applicationStatus.SetStatus("Generating query plan...", true);
                    m_SelectedFileName = file.FileName;
                    connectionService.GetQueryPlan(new DatabaseInfo { Name = dbName }, new SbqlQuery(query), GetQueryPlanCompleted);

                }
            }
        }

        private string GetSelectedQuery(out FileName filename, out int line, out int column)
        {
            OpenedFile file = fileService.Value.GetActiveFile();
            if (file != null && file is SbqlOpenedFile)
            {
                SbqlOpenedFile sbqlFile = file as SbqlOpenedFile;

                int selectionLength = (sbqlFile.Document as ISourceEditor).TextEditor.TextArea.Selection.Length;
                int caretColumn = (sbqlFile.Document as ISourceEditor).TextEditor.TextArea.Caret.Column;

                line = (sbqlFile.Document as ISourceEditor).TextEditor.TextArea.Caret.Line;
                int col1 = caretColumn - selectionLength;
                column = Math.Min(col1 >= 1 ? col1 : caretColumn, caretColumn);
                filename = sbqlFile.FileName;
                return sbqlFile.SelectedQuery;
            }
            line = 0;
            column = 0;
            filename = null;
            return null;
        }

        private void GetQueryPlanCompleted(ExecuteQueryStatus status, IQueryPlanReslult result)
        {
            SetEnableCondition(true);

            switch (status)
            {
                case ExecuteQueryStatus.Canceled:
                    applicationStatus.SetStatus("Generating canceled", false);
                    break;
                case ExecuteQueryStatus.Error:
                    applicationStatus.SetStatus("Generating failure", false);
                    queryPlanService.ShowErrorMessage(result.StringOutput);
                    break;
                case ExecuteQueryStatus.Done:
                    applicationStatus.SetStatus("Query Plan generated", false);

                    if(result != null)
                    {
                        List<MUTDOD.Common.QueryPlan> list = new List<MUTDOD.Common.QueryPlan>();
                        list.Add(result.QueryPlan);
                        queryPlanService.ShowQueryPlan(list);
                    }
                    else
                    {
                        applicationStatus.SetStatus("Query Plan generated without result", false);
                    }

                    break;
            }
            m_SelectedFileName = null;
        }
        void CheckEnableCondition()
        {
            bool enable = connectionService.IsConnected &&
                            connectionService.Databases != null &&
                            connectionService.Databases.Count > 0 &&
                            fileService.Value.OpenedFiles.Count > 0 &&
                            !isExecuting;
            SetEnableCondition(enable);
        }

        private void SetEnableCondition(bool value)
        {
            (EnableCondition as ConcreteCondition).SetCondition(value);
        }

        public void OnImportsSatisfied()
        {
            fileService.Value.FileClosed += new EventHandler<FileEventArgs>(fileService_FileClosed);
            fileService.Value.FileCreated += new EventHandler<FileEventArgs>(fileService_FileCreated);

            connectionService.DatabasesChanged += new EventHandler(connectionService_DatabasesChanged);
            connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
            connectionService.IsExecutingChanged += new EventHandler<IsExecutingEventArgs>(connectionService_IsExecutingChanged);
        }

        void connectionService_IsExecutingChanged(object sender, IsExecutingEventArgs e)
        {
            isExecuting = e.IsExecuting;
            CheckEnableCondition();
        }

        void connectionService_Disconnected(object sender, EventArgs e)
        {
            CheckEnableCondition();
        }

        void connectionService_DatabasesChanged(object sender, EventArgs e)
        {
            CheckEnableCondition();
        }

        void fileService_FileCreated(object sender, FileEventArgs e)
        {
            CheckEnableCondition();
        }

        void fileService_FileClosed(object sender, FileEventArgs e)
        {
            CheckEnableCondition();
        }
    }


}
