using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.Collections;

namespace OdraIDE.Editor.Sbql
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Self, typeof(IToolBar))]
    public class ExecuteToolBar : AbstractToolBar, IPartImportsSatisfiedNotification
    {
        public ExecuteToolBar()
        {
            Name = Resources.Strings.Workbench_ExecuteToolBar;
            VisibleCondition = new ConcreteCondition(false);
        }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.ToolBars.Execute, typeof(IToolBarItem), AllowRecomposition = true)]
        private IEnumerable<IToolBarItem> importedItems { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(importedItems);
            fileService.Value.FileClosed += new EventHandler<FileEventArgs>(fileService_FileClosed);
            fileService.Value.FileCreated += new EventHandler<FileEventArgs>(fileService_FileCreated);

            connectionService.DatabasesChanged += new EventHandler(connectionService_DatabasesChanged);
            connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
        }

        void connectionService_Disconnected(object sender, EventArgs e)
        {
            CheckVisibleCondition();
        }

        void connectionService_DatabasesChanged(object sender, EventArgs e)
        {
            CheckVisibleCondition();
        }

        void fileService_FileCreated(object sender, FileEventArgs e)
        {
            CheckVisibleCondition();
        }

        void fileService_FileClosed(object sender, FileEventArgs e)
        {
            CheckVisibleCondition();
        }

        void CheckVisibleCondition()
        {
            //TODO po dodaniu bazy danych sprawdzac
            bool visible = fileService.Value.OpenedFiles.Count > 0 && 
                            connectionService.IsConnected && 
                            connectionService.Databases.Count > 0;
            (VisibleCondition as ConcreteCondition).SetCondition(visible);
        }
    }

    [Export(ExtensionPoints.Workbench.ToolBars.Execute, typeof(IToolBarItem))]
    public class ExecuteToolbarExecuteQuery : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.Commands.ExecuteQuery, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public ExecuteToolbarExecuteQuery()
        {
            ID = "ExecuteToolbarExecuteQuery";
            ToolTip = "Execute query (F5)";
            SetIconFromBitmap(Resources.Images.Execute);
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            Command = command;
        }

        #endregion
    }

    [Export(ExtensionPoints.Workbench.ToolBars.Execute, typeof(IToolBarItem))]
    public class ExecuteSelectedToolbarExecuteQuery : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.Commands.ExecuteSelectedQuery, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public ExecuteSelectedToolbarExecuteQuery()
        {
            ID = "ExecuteSelectedToolbarExecuteQuery";
            ToolTip = "Execute selected query (F5 + Ctrl)";
            InsertRelativeToID = "ExecuteToolbarExecuteQuery";
            BeforeOrAfter = RelativeDirection.After;
            SetIconFromBitmap(Resources.Images.ExecuteSelected);
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            Command = command;
        }

        #endregion
    }

    [Export(ExtensionPoints.Workbench.ToolBars.Execute, typeof(IToolBarItem))]
    public class ExecuteToolbarCancelExecuteQuery : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.Commands.CancelExecuteQuery, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        public ExecuteToolbarCancelExecuteQuery()
        {
            ID = "ExecuteToolbarCancelExecuteQuery";
            ToolTip = "Cancel query (F5 + Shift)";
            InsertRelativeToID = "ExecuteSelectedToolbarExecuteQuery";
            BeforeOrAfter = RelativeDirection.After;
            SetIconFromBitmap(Resources.Images.Stop);
            VisibleCondition = new ConcreteCondition(false);
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            Command = command;
            connectionService.IsExecutingChanged += new EventHandler<IsExecutingEventArgs>(connectionService_IsExecutingChanged);
        }

        void connectionService_IsExecutingChanged(object sender, IsExecutingEventArgs e)
        {
            (VisibleCondition as ConcreteCondition).SetCondition(e.IsExecuting);
        }

        #endregion
    }

    [Export(ExtensionPoints.Workbench.ToolBars.Execute, typeof(IToolBarItem))]
    [Export(CompositionPoints.Workbench.ExecuteToolbar.DatabasesComboBox, typeof(DatabasesToolBarComboBox))]
    public class DatabasesToolBarComboBox : AbstractToolBarComboBox, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        public DatabasesToolBarComboBox()
        {
            ID = "DatabasesToolBarComboBox";
            ToolTip = "Databases";
            InsertRelativeToID = "ExecuteToolbarCancelExecuteQuery";
            BeforeOrAfter = RelativeDirection.After;
        }

        public void OnImportsSatisfied()
        {
            connectionService.DatabasesChanged += new EventHandler(DatabasesChanged);
            connectionService.Disconnected += new EventHandler(connectionService_Disconnected);
        }

        void connectionService_Disconnected(object sender, EventArgs e)
        {
            Items = null;
        }

        void DatabasesChanged(object sender, EventArgs e)
        {
            Items = connectionService.Databases;
            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
        }
    }
}
