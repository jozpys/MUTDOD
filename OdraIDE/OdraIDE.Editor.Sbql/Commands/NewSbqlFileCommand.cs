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
    [Export(CompositionPoints.Workbench.Commands.NewSbqlFile, typeof(ICustomCommand))]
    public class NewSbqlFileCommand : BaseCommand
    {
        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private IFileService fileService { get; set; }

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Export shortcut for this command (Ctrl + N)
                return new KeyBinding(this, new KeyGesture(Key.N, ModifierKeys.Control));
            }
        }

        public NewSbqlFileCommand()
        {
            ExecuteCommand += new ExecuteHandler(ShowSourceEditor);
        }

        void ShowSourceEditor()
        {
            fileService.NewFile(".sbql");
        }

    }
}
