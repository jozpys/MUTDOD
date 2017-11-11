using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor.DODQL.Workbench.MainMenu
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.NewMenu, typeof(IMenuItem))]
    class FileMenuNewFile : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.NewSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public FileMenuNewFile()
        {
            ID = "NewFile";
            Header = Resources.Strings.Workbench_MainMenu_File_New_File;
            SetIconFromBitmap(Resources.Images.New_file);
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            Command = command;
        }

        #endregion
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.OpenMenu, typeof(IMenuItem))]
    class FileMenuOpenFile : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.OpenSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public FileMenuOpenFile()
        {
            ID = "OpenFile";
            Header = Resources.Strings.Workbench_MainMenu_File_Open;
            SetIconFromBitmap(Resources.Images.Open);
        }

        public void OnImportsSatisfied()
        {
            Command = command;

        }
    }

    //TODO dorobic separator w menu
   // [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuSeparator : AbstractSeparator
    {
        public FileMenuSeparator()
        {
            ID = "Separator1";
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = "OpenFile";
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuSaveFile : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.SaveSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public FileMenuSaveFile()
        {
            ID = Sbql.Extensions.Workbench.MainMenu.FileMenu.Save;
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = OdraIDE.Core.Extensions.Workbench.MainMenu.FileMenu.Open;
            Header = Resources.Strings.Workbench_MainMenu_File_Save;
            SetIconFromBitmap(Resources.Images.Save);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuSaveFileAs : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.SaveSbqlFileAs, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public FileMenuSaveFileAs()
        {
            ID = Sbql.Extensions.Workbench.MainMenu.FileMenu.SaveAs;
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = Sbql.Extensions.Workbench.MainMenu.FileMenu.Save;
            Header = Resources.Strings.Workbench_MainMenu_File_Save_As;
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuSaveAllFiles : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        [Import(Sbql.CompositionPoints.Workbench.Commands.SaveAllSbqlFiles, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public FileMenuSaveAllFiles()
        {
            ID = Sbql.Extensions.Workbench.MainMenu.FileMenu.SaveAll;
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = Sbql.Extensions.Workbench.MainMenu.FileMenu.SaveAs;
            Header = Resources.Strings.Workbench_MainMenu_File_Save_All;
            SetIconFromBitmap(Resources.Images.SaveAll);
        }

        public void OnImportsSatisfied()
        {
            Command = command;
        }
    }
}
