using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor.Sbql
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Self, typeof(IToolBar))]
    public class EditorToolBar : AbstractToolBar, IPartImportsSatisfiedNotification
    {
        public EditorToolBar()
        {
            Name = Resources.Strings.Workbench_EditorToolBar;
            VisibleCondition = new ConcreteCondition(false);
        }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.ToolBars.Editor, typeof(IToolBarItem), AllowRecomposition = true)]
        private IEnumerable<IToolBarItem> importedItems { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(importedItems);
            fileService.Value.FileClosed += new EventHandler<FileEventArgs>(fileService_FileClosed);
            fileService.Value.FileCreated += new EventHandler<FileEventArgs>(fileService_FileCreated);
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
            bool visible = fileService.Value.OpenedFiles.Count > 0;
            (VisibleCondition as ConcreteCondition).SetCondition(visible);
        }
    }

    [Export(ExtensionPoints.Workbench.ToolBars.Editor, typeof(IToolBarItem))]
    public class EditorToolbarCommentLines : AbstractToolBarButton, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.Commands.CommentLines, typeof(ICustomCommand))]
        private ICustomCommand command { get; set; }

        public EditorToolbarCommentLines()
        {
            ID = "EditorToolbarCommentLines";
            ToolTip = "Comment out / Uncomment the selected lines (Ctrl + /)";
            SetIconFromBitmap(Resources.Images.Comment);
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            Command = command;
        }

        #endregion
    }


}
