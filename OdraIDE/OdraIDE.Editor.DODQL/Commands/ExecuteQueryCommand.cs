using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Windows.Input;
using System.ComponentModel;
using System.Xml;
using System.Text.RegularExpressions;
using MUTDOD.Common;
using OdraIDE.Editor.DODQL.Workbench.ToolBar;
using OdraIDE.Editor.Sbql.Commands;

namespace OdraIDE.Editor.Sbql
{
    [Export(CompositionPoints.Workbench.Commands.ExecuteQuery, typeof(ICustomCommand))]
    public class ExecuteQueryCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        private ITaskService taskService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(CompositionPoints.Workbench.ExecuteToolbar.DatabasesComboBox, typeof(DatabasesToolBarComboBox))]
        private DatabasesToolBarComboBox databasesComboBox { get; set; }

        [Import(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
        private IResultsService resultsService { get; set; }

        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        private string m_lastQuery;

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (F5)
                return new KeyBinding(this, new KeyGesture(Key.F5, ModifierKeys.None));
            }
        }

        public ExecuteQueryCommand()
        {
            EnableCondition = new ConcreteCondition(false);
            ExecuteCommand += new ExecuteHandler(ExecuteQuery);
        }

        private static FileName m_SelectedFileName;

        private void ExecuteQuery()
        {
            OpenedFile file = fileService.Value.GetActiveFile();
            if (file is SbqlOpenedFile)
            {
                string dbName = databasesComboBox.SelectedItem as string;
                SbqlOpenedFile sbqlFile = file as SbqlOpenedFile;
                string query = sbqlFile.Query;
                this.m_lastQuery = query;
                if (!string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(query))
                {
                    SetEnableCondition(false);
                    resultsService.Clear();
                    applicationStatus.SetStatus("Executing query...", true);
                    m_SelectedFileName = file.FileName;
                    connectionService.ExecuteQuery(new DatabaseInfo { Name = dbName }, new SbqlQuery(query), ExecuteQueryCompleted);
                }
            }
        }

        private void ExecuteQueryCompleted(ExecuteQueryStatus status, IQueryResult result)
        {
            SetEnableCondition(true);
            switch (status)
            {
                case ExecuteQueryStatus.Canceled:
                    applicationStatus.SetStatus("Query canceled", false);
                    break;
                case ExecuteQueryStatus.Error:
                    applicationStatus.SetStatus("Query failure", false);
                    break;
                case ExecuteQueryStatus.Done:
                default:
                    taskService.RemoveAll(task => task.FileName == m_SelectedFileName);
                    applicationStatus.SetStatus("Query executed", false);
                        StringBuilder sb = new StringBuilder();
                    if (result != null)
                    {
                        bool focusOnGrid = false;

                        while (result != null)
                        {
                            try
                            {
                                string refactoredResults = RefactorResults(result.StringOutput);
                                DataMatrix dm = CreateDataMatrix(refactoredResults);
                                resultsService.ShowDataResult(dm);
                                sb.AppendLine(string.Format("{0} objects affected", dm.Rows.Count));
                                focusOnGrid = true;
                            }
                            catch (Exception)
                            {
                                //show as string
                                sb.AppendLine(result.StringOutput);
                                //show references
                                if (result.QueryResultType == ResultType.ReferencesOnly)
                                {
                                    int id = 0;
                                    DataMatrix dm = new DataMatrix();
                                    dm.Columns.Add(new MatrixColumn() {Name = "No"});
                                    dm.Columns.Add(new MatrixColumn() {Name = "Oid"});
                                    result.QueryResults.ForEach(o => dm.Rows.Add(new object[] {++id, o.Id}));
                                    resultsService.ShowDataResult(dm);
                                    sb.AppendLine(string.Format("{0} objects affected", id));
                                    focusOnGrid = true;
                                }
                            }
                            result = result.NextResult;
                        }
                        resultsService.ShowStringResult(sb.ToString());
                        if(focusOnGrid)
                        {
                            resultsService.FocusOnDataResult();
                        }
                    }
                    else
                    {
                        applicationStatus.SetStatus("Query executed without result", false);
                        resultsService.ShowStringResult("There is no result...");
                    }

                    break;
            }
            m_SelectedFileName = null;
        }


        public static Task CheckQueryError(FileName filename, string output, string query, int lineOffset = 0, int columnOffset = 0)
        {
            if (output.Contains("error"))
            {
                int lineIndex = output.IndexOf("line");
                int lineStartIndex = lineIndex + 5;
                //int lineEndIndex = output.IndexOf(" ", lineStartIndex);

                int colIndex = output.IndexOf("column");
                int colStartIndex = colIndex + 7;
                int colEndIndex = Math.Min(output.IndexOf(" ", colStartIndex), output.IndexOf("\n", colStartIndex));

                string lineString = output.Substring(lineStartIndex, colIndex - lineStartIndex - 1);
                string colString = output.Substring(colStartIndex, colEndIndex - colStartIndex - 1);

                string[] lineNumber = Regex.Split(lineString, @"\D+");
                string[] colNumber = Regex.Split(colString, @"\D+");

                int line = Int32.Parse(lineNumber[0]);
                int column = Int32.Parse(colNumber[0]);

                int startMsgIndex = output.IndexOf(column.ToString());
                int endMsgIndex = output.IndexOf(query);

                string msg = output;
                try
                {
                     msg = output.Substring(startMsgIndex + column.ToString().Length, endMsgIndex - startMsgIndex - 2);
                }
                catch (ArgumentOutOfRangeException )
                {
                    //do nothing
                }
                msg = msg.Replace("\n", "").Replace("\r", "").Replace("\t", "");

                int errorIndex = output.IndexOf("error");
                msg = output.Substring(0, errorIndex) + "error: " + msg;

                Task task = new Task(filename, TaskType.Error, 
                                    (lineOffset > 0 ? lineOffset - 1 : 0) + line, 
                                    (columnOffset > 0 ? columnOffset - 1 : 0) + column, 
                                    msg);
                return task;
            } 
            else
            {
                return null;
            }

        }

        public static DataMatrix CreateDataMatrix(string xmlResults)
        {
            DataMatrix dm = new DataMatrix();
            dm.Columns.Add(new MatrixColumn() { Name = "No" });

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResults);

            HashSet<string> columnSet = new HashSet<string>();
            LinkedList<string> columnList = new LinkedList<string>(); //save order
            LinkedList<LinkedList<string>> valueList = new LinkedList<LinkedList<string>>();
            
            XmlNodeList list = doc.GetElementsByTagName("Object");
            
            for (int i = 0; i < list.Count; i++)
            {
                XmlNodeList nodeList = list.Item(i).ChildNodes;
                int columnsCount = nodeList.Count;
                object[] row = new object[columnsCount + 1];
                row[0] = i+1; //No
                    StringBuilder sb = new StringBuilder();
                for (int j = 1; j < columnsCount + 1; j++)
                {
                    var node = nodeList.Item(j - 1);
                    sb.Clear();
                    foreach (XmlElement n in node.ChildNodes)
                        sb.AppendLine(string.Format("{0}={1}", n.Name, n.InnerText));
                    row[j] = sb.ToString();
                    string name = nodeList.Item(j-1).Name;
                    if (!columnSet.Contains(name))
                    {
                        columnSet.Add(name);
                        dm.Columns.Add(new MatrixColumn() { Name = name });
                        columnList.AddLast(name);
                    }
                }
                dm.Rows.Add(row);
            }
            
            return dm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputResult"></param>
        /// <returns></returns>
        public static string RefactorResults(string inputResult)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(inputResult);

            XmlDocument newDoc = new XmlDocument();
            XmlNode resultNode = doc.GetElementsByTagName("result").Item(0);
            XmlNodeList nodes = resultNode.ChildNodes;
            XmlNode outer;
            XmlNode newResult = newDoc.CreateElement("Result");

            foreach (XmlNode node in nodes)
            {
                outer = newDoc.CreateElement("Object");
                    XmlNode importedNode = newDoc.ImportNode(node, true);
                    outer.AppendChild(importedNode);
                    newResult.AppendChild(outer);
            }
            XmlNode root = newDoc.CreateElement("root");
            root.AppendChild(newResult);
            string newResultStr = root.InnerXml;

            return newResultStr;
        }

        public void OnImportsSatisfied()
        {
            fileService.Value.FileClosed += new EventHandler<FileEventArgs>(fileService_FileClosed);
            fileService.Value.FileCreated += new EventHandler<FileEventArgs>(fileService_FileCreated);

            connectionService.DatabasesChanged += new EventHandler(connectionService_DatabasesChanged);
            connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
            connectionService.IsExecutingChanged += new EventHandler<IsExecutingEventArgs>(connectionService_IsExecutingChanged);
        }

        private bool isExecuting = false;

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
    }

    [Export(CompositionPoints.Workbench.Commands.ExecuteSelectedQuery, typeof(ICustomCommand))]
    public class ExecuteSelectedQueryCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        

        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        private ITaskService taskService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(CompositionPoints.Workbench.ExecuteToolbar.DatabasesComboBox, typeof(DatabasesToolBarComboBox))]
        private DatabasesToolBarComboBox databasesComboBox { get; set; }

        [Import(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
        private IResultsService resultsService { get; set; }

        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        private int m_SelectedLine;
        private int m_SelectedColumn;
        private FileName m_SelectedFileName;
        private string m_lastQuery;

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (F5)
                return new KeyBinding(this, new KeyGesture(Key.F5, ModifierKeys.Control));
            }
        }

        public ExecuteSelectedQueryCommand()
        {
            EnableCondition = new ConcreteCondition(false);
            ExecuteCommand += new ExecuteHandler(ExecuteQuery);
        }

        private void ExecuteQuery()
        {
            string dbName = databasesComboBox.SelectedItem as string;
            int line;
            FileName filename;
            int column;
            string query = GetSelectedQuery(out filename, out line, out column);
            this.m_lastQuery = query;
            if (!string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(query))
            {
                m_SelectedFileName = filename;
                m_SelectedLine = line;
                m_SelectedColumn = column;
                SetEnableCondition(false);
                resultsService.Clear();
                applicationStatus.SetStatus("Executing selected query...", true);
                connectionService.ExecuteQuery(new DatabaseInfo{Name=dbName}, new SbqlQuery(query), ExecuteQueryCompleted);
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

        private void ExecuteQueryCompleted(ExecuteQueryStatus status, IQueryResult result)
        {
            taskService.RemoveAll(task => (task.FileName == m_SelectedFileName && task.Line == m_SelectedLine));
            SetEnableCondition(true);
            switch (status)
            {
                case ExecuteQueryStatus.Canceled:
                    applicationStatus.SetStatus("Query canceled", false);
                    break;
                case ExecuteQueryStatus.Error:
                    applicationStatus.SetStatus("Query failure", false);
                    break;
                case ExecuteQueryStatus.Done:
                default:

                    if (result != null)
                    {
                        applicationStatus.SetStatus("Selected query executed", false);
                        resultsService.ShowStringResult(result.StringOutput);

                        try
                        {
                            string refactoredResults = ExecuteQueryCommand.RefactorResults(result.StringOutput);
                            DataMatrix dm = ExecuteQueryCommand.CreateDataMatrix(refactoredResults);
                            resultsService.ShowDataResult(dm);
                        }
                        catch (XmlException)
                        {
                            //show errors
                            Task task = ExecuteQueryCommand.CheckQueryError(m_SelectedFileName, result.StringOutput, m_lastQuery, m_SelectedLine, m_SelectedColumn);
                            if (task != null)
                            {
                                taskService.Add(task);
                            }
                        }
                    }
                    break;
            }
            m_SelectedFileName = null;
            m_SelectedLine = 0;
        }
        
        public void OnImportsSatisfied()
        {
            fileService.Value.FileClosed += new EventHandler<FileEventArgs>(fileService_FileClosed);
            fileService.Value.FileCreated += new EventHandler<FileEventArgs>(fileService_FileCreated);
            fileService.Value.TextSelectionChanged += new EventHandler(Value_TextSelectionChanged);

            connectionService.DatabasesChanged += new EventHandler(connectionService_DatabasesChanged);
            connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
            connectionService.IsExecutingChanged += new EventHandler<IsExecutingEventArgs>(connectionService_IsExecutingChanged);
        }

        private bool isExecuting = false;

        void connectionService_IsExecutingChanged(object sender, IsExecutingEventArgs e)
        {
            isExecuting = e.IsExecuting;
            CheckEnableCondition();
        }

        void Value_TextSelectionChanged(object sender, EventArgs e)
        {
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

        void CheckEnableCondition()
        {
            int line;
            int column;
            FileName filename;

            bool enable = connectionService.IsConnected &&
                            connectionService.Databases != null &&
                            connectionService.Databases.Count > 0 &&
                            fileService.Value.OpenedFiles.Count > 0 &&
                            !isExecuting &&
                            !string.IsNullOrWhiteSpace(GetSelectedQuery(out filename, out line, out column));
            SetEnableCondition(enable);
        }

        private void SetEnableCondition(bool value)
        {
            (EnableCondition as ConcreteCondition).SetCondition(value);
        }
    }


    [Export(CompositionPoints.Workbench.Commands.CancelExecuteQuery, typeof(ICustomCommand))]
    public class CancelExecuteQueryCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (F5 + Shift)
                return new KeyBinding(this, new KeyGesture(Key.F5, ModifierKeys.Shift));
            }
        }

        public CancelExecuteQueryCommand()
        {
            EnableCondition = new ConcreteCondition(false);
            ExecuteCommand += new ExecuteHandler(CancelExecuteQuery);
        }

        private void CancelExecuteQuery()
        {
            connectionService.CancelExecutingQuery();
            SetEnableCondition(false);
            applicationStatus.SetStatus("Canceling query...", true);
        }
        
        public void OnImportsSatisfied()
        {
            connectionService.IsExecutingChanged += new EventHandler<IsExecutingEventArgs>(connectionService_IsExecutingChanged);
        }

        void connectionService_IsExecutingChanged(object sender, IsExecutingEventArgs e)
        {
            SetEnableCondition(e.IsExecuting);
        }

        private void SetEnableCondition(bool value)
        {
            (EnableCondition as ConcreteCondition).SetCondition(value);
        }
    }
}
