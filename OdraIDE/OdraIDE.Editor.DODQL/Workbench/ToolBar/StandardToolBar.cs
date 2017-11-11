using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor.DODQL.Workbench.ToolBar
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarNewFileItem : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.NewSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public StandardToolbarNewFileItem()
        {
            ID = "ToolbarNewFile";
            ToolTip = "Create new DODQL file...";
            InsertRelativeToID = "ToolbarSeparator1";
            BeforeOrAfter = RelativeDirection.Before;
            SetIconFromBitmap(Resources.Images.New_file);
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            Command = command;
        }

        #endregion
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarOpenItem : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.OpenSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public StandardToolbarOpenItem()
        {
            ID = "ToolbarOpen";
            InsertRelativeToID = "ToolbarNewFile";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Open DODQL file...";
            SetIconFromBitmap(Resources.Images.Open);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarSaveItem : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.SaveSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public StandardToolbarSaveItem()
        {
            ID = "ToolbarSave";
            InsertRelativeToID = "ToolbarOpen";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Save file...";
            SetIconFromBitmap(Resources.Images.Save);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem))]
    class StandardToolbarSaveAllItem : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.SaveAllSbqlFiles, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public StandardToolbarSaveAllItem()
        {
            ID = "ToolbarSaveAll";
            InsertRelativeToID = "ToolbarSave";
            BeforeOrAfter = RelativeDirection.After;
            ToolTip = "Save all files...";
            SetIconFromBitmap(Resources.Images.SaveAll);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }
}
