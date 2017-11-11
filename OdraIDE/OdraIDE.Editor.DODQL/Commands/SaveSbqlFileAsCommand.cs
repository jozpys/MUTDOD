using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.Windows;
using System.ComponentModel.Composition;
using System.Windows.Input;
using OdraIDE.Editor.DODQL.Commands;

namespace OdraIDE.Editor.Sbql
{
    [Export(CompositionPoints.Workbench.Commands.SaveSbqlFileAs, typeof(ICustomCommand))]
    public class SaveSbqlFileAsCommand : AbstractSaveDODQLFile, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.FileDialog.FileDialogService, typeof(IFileDialogService))]
        private Lazy<IFileDialogService> fileDialogService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        protected override Lazy<IFileService> fileService { get; set; }

        public override IFileDialogService FileDialogService
        {
            get { return fileDialogService.Value; }
        }

        public override IFileService FileService
        {
            get { return fileService.Value; }
        }

        public SaveSbqlFileAsCommand()
        {

            EnableCondition = new ConcreteCondition(false);
            ExecuteCommand += new ExecuteHandler(SaveFile);
        }

        public void OnImportsSatisfied()
        {
            fileService.Value.FileClosed += new EventHandler<FileEventArgs>(fileService_FileClosed);
            fileService.Value.FileCreated += new EventHandler<FileEventArgs>(fileService_FileCreated);
        }

        void fileService_FileCreated(object sender, FileEventArgs e)
        {
            CheckCondition();
        }

        void fileService_FileClosed(object sender, FileEventArgs e)
        {
            CheckCondition();
        }

        void CheckCondition()
        {
            (EnableCondition as ConcreteCondition).SetCondition(fileService.Value.OpenedFiles.Count > 0);
        }
    }
}
