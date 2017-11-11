using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Tasks
{
    [Export(CompositionPoints.Workbench.Commands.ErrorTaskFilter, typeof(ICustomCommand))]
    public class ErrorTaskFilterCommand : BaseCommand
    {
        [Import(typeof(TaskFilterToolBarErrorsItem))]
        private TaskFilterToolBarErrorsItem button { get; set; }

        [Import(OdraIDE.Tasks.CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
        private GridTasksPad gridTasksPad { get; set; }

        public ErrorTaskFilterCommand()
        {
            ExecuteCommand += new ExecuteHandler(DoFilter);
        }

        void DoFilter()
        {
            gridTasksPad.FilterTaskList(TaskType.Error, button.IsChecked);
        }
    }

    [Export(CompositionPoints.Workbench.Commands.WarningTaskFilter, typeof(ICustomCommand))]
    public class WarningTaskFilterCommand : BaseCommand
    {
        [Import(typeof(TaskFilterToolBarWarningsItem))]
        private TaskFilterToolBarWarningsItem button { get; set; }

        [Import(OdraIDE.Tasks.CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
        private GridTasksPad gridTasksPad { get; set; }

        public WarningTaskFilterCommand()
        {
            ExecuteCommand += new ExecuteHandler(DoFilter);
        }

        void DoFilter()
        {
            gridTasksPad.FilterTaskList(TaskType.Warning, button.IsChecked);
        }
    }

    [Export(CompositionPoints.Workbench.Commands.MessageTaskFilter, typeof(ICustomCommand))]
    public class MessageTaskFilterCommand : BaseCommand
    {
        [Import(typeof(TaskFilterToolBarMessagesItem))]
        private TaskFilterToolBarMessagesItem button { get; set; }

        [Import(OdraIDE.Tasks.CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
        private GridTasksPad gridTasksPad { get; set; }

        public MessageTaskFilterCommand()
        {
            ExecuteCommand += new ExecuteHandler(DoFilter);
        }

        void DoFilter()
        {
            gridTasksPad.FilterTaskList(TaskType.Message, button.IsChecked);
        }
    }
}
