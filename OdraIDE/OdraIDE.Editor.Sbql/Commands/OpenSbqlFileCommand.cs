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
    [Export(CompositionPoints.Workbench.Commands.OpenSbqlFile, typeof(ICustomCommand))]
    public class OpenSbqlFileCommand : AbstractOpenFileCommand
    {
        [Import(OdraIDE.Core.Services.FileDialog.FileDialogService, typeof(IFileDialogService))]
        private Lazy<IFileDialogService> fileDialogService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        public override IFileDialogService FileDialogService
        {
            get { return fileDialogService.Value; }
        }

        public override IFileService FileService
        {
            get { return fileService.Value; }
        }

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Export shortcut for this command (Ctrl + O)
                return new KeyBinding(this, new KeyGesture(Key.O, ModifierKeys.Control));
            }
        }

        public OpenSbqlFileCommand()
        {
            ExecuteCommand += new ExecuteHandler(OpenSbqlFileCommand_ExecuteCommand);
        }

        void OpenSbqlFileCommand_ExecuteCommand()
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.Add("sbql", "SBQL Files");
            OpenFile("sbql", @"d:\", filters);
        }
    }
}
