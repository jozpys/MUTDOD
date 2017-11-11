using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.Windows;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace OdraIDE.Editor.Sbql
{
    [Export(CompositionPoints.Workbench.Commands.SaveSbqlFile, typeof(ICustomCommand))]
    public class SaveSbqlFileCommand : AbstractSaveFileCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.FileDialog.FileDialogService, typeof(IFileDialogService))]
        private Lazy<IFileDialogService> fileDialogService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (Ctrl + S)
                return new KeyBinding(this, new KeyGesture(Key.S, ModifierKeys.Control));
            }
        }

        public override IFileDialogService FileDialogService
        {
            get { return fileDialogService.Value; }
        }

        public override IFileService FileService
        {
            get { return fileService.Value; }
        }
        
        public SaveSbqlFileCommand()
        {
            
            EnableCondition = new ConcreteCondition(false);
            ExecuteCommand += new ExecuteHandler(SaveFile);
        }

        public void SaveFile()
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.Add("sbql", "SBQL Files");

            OpenedFile activeFile = fileService.Value.GetActiveFile();
            if (activeFile != null)
            {
                SaveFile(activeFile, "sbql", @"d:\", filters);
            }            
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
