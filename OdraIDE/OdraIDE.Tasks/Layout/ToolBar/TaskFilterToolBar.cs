using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Tasks
{
    [Export(ExtensionPoints.ToolBars.Self, typeof(IToolBar))]
    public class TaskFilterToolBar : AbstractToolBar, IPartImportsSatisfiedNotification
    {
        public TaskFilterToolBar()
        {
            Name = "Task Filter ToolBar";
        }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.ToolBars.TaskFilter, typeof(IToolBarItem), AllowRecomposition = true)]
        private IEnumerable<IToolBarItem> importedItems { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(importedItems);
        }
    }

    #region Toolbar buttons

    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(TaskFilterToolBarErrorsItem))]
    [Export(ExtensionPoints.ToolBars.TaskFilter, typeof(IToolBarItem))]
    public class TaskFilterToolBarErrorsItem : AbstractToolBarToggleButton, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Tasks.CompositionPoints.Workbench.Commands.ErrorTaskFilter, typeof(ICustomCommand))]
        private ICustomCommand command;

        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        private ITaskService TaskService { get; set; }

        public TaskFilterToolBarErrorsItem()
        {
            ID = "TaskFilterToolBarErrorsItem";
            ToolTip = "Show Errors";
            Text = " Errors";
            IsChecked = true;
            SetIconFromBitmap(Resources.Images.Error);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
            TaskService.Added += new TaskEventHandler(TaskService_Added);
            TaskService.Removed += new TaskEventHandler(TaskService_Removed);
            TaskService.Cleared += new EventHandler(TaskService_Cleared);
        }

        void TaskService_Added(object sender, TaskEventArgs e)
        {
            RefreshCount();
        }

        void TaskService_Removed(object sender, TaskEventArgs e)
        {
            RefreshCount();
        }

        void TaskService_Cleared(object sender, EventArgs e)
        {
            RefreshCount();
        }

        private void RefreshCount()
        {
            Text = " (" + TaskService.GetCount(TaskType.Error).ToString() + ") Errors";
        }
    }

    [Export(ExtensionPoints.ToolBars.TaskFilter, typeof(IToolBarItem))]
    public class FilterToolbarSeparator1 : AbstractToolBarSeparator
    {
        public FilterToolbarSeparator1()
        {
            ID = "FilterToolbarSeparator1";
        }
    }

    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(TaskFilterToolBarWarningsItem))]
    [Export(ExtensionPoints.ToolBars.TaskFilter, typeof(IToolBarItem))]
    public class TaskFilterToolBarWarningsItem : AbstractToolBarToggleButton, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Tasks.CompositionPoints.Workbench.Commands.WarningTaskFilter, typeof(ICustomCommand))]
        private ICustomCommand command;

        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        private ITaskService TaskService { get; set; }

        public TaskFilterToolBarWarningsItem()
        {
            ID = "TaskFilterToolBarWarningsItem";
            ToolTip = "Show Warnings";
            Text = " Warnings";
            SetIconFromBitmap(Resources.Images.Warning);
            IsChecked = true;
        }

        public void OnImportsSatisfied()
        {
            Command = command;
            TaskService.Added += new TaskEventHandler(TaskService_Added);
            TaskService.Removed += new TaskEventHandler(TaskService_Removed);
            TaskService.Cleared += new EventHandler(TaskService_Cleared);
        }

        void TaskService_Added(object sender, TaskEventArgs e)
        {
            RefreshCount();
        }

        void TaskService_Removed(object sender, TaskEventArgs e)
        {
            RefreshCount();
        }

        void TaskService_Cleared(object sender, EventArgs e)
        {
            RefreshCount();
        }

        private void RefreshCount()
        {
            Text = " (" + TaskService.GetCount(TaskType.Warning).ToString() + ") Warnings";
        }
    }

    [Export(ExtensionPoints.ToolBars.TaskFilter, typeof(IToolBarItem))]
    public class FilterToolbarSeparator2 : AbstractToolBarSeparator
    {
        public FilterToolbarSeparator2()
        {
            ID = "FilterToolbarSeparator2";
        }
    }

    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(TaskFilterToolBarMessagesItem))]
    [Export(ExtensionPoints.ToolBars.TaskFilter, typeof(IToolBarItem))]
    public class TaskFilterToolBarMessagesItem : AbstractToolBarToggleButton, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Tasks.CompositionPoints.Workbench.Commands.MessageTaskFilter, typeof(ICustomCommand))]
        private ICustomCommand command;

        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        private ITaskService TaskService { get; set; }

        public TaskFilterToolBarMessagesItem()
        {
            ID = "TaskFilterToolBarMessagesItem";
            ToolTip = "Show Messages";
            Text = " Messages";
            IsChecked = true;
            SetIconFromBitmap(Resources.Images.Info);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
            TaskService.Added += new TaskEventHandler(TaskService_Added);
            TaskService.Removed += new TaskEventHandler(TaskService_Removed);
            TaskService.Cleared += new EventHandler(TaskService_Cleared);
        }

        void TaskService_Added(object sender, TaskEventArgs e)
        {
            RefreshCount();
        }

        void TaskService_Removed(object sender, TaskEventArgs e)
        {
            RefreshCount();
        }

        void TaskService_Cleared(object sender, EventArgs e)
        {
            RefreshCount();
        }

        private void RefreshCount()
        {
            Text = " (" + TaskService.GetCount(TaskType.Message).ToString() + ") Messages";
        }
    }
    
    #endregion
}
