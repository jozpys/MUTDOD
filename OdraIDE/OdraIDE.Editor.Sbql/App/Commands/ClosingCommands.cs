using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor.Sbql
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ClosingCommands, typeof(IExecutableCommand))]
    public class ClosingCommand : AbstractExtension, IExecutableCommand
    {
        [Import(CompositionPoints.Workbench.Commands.SaveSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand saveCommand { get; set; }

        [Import(OdraIDE.Core.Services.Messaging.MessagingService, typeof(IMessagingService))]
        private Lazy<IMessagingService> messagingService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Import(typeof(SbqlFileClosingCommand))]
        private SbqlFileClosingCommand fileClosingCommand { get; set; }

        public void Run(params object[] args)
        {
            foreach (OpenedFile file in fileService.Value.OpenedFiles)
            {
                fileClosingCommand.OnClosing(file);
            }
        }
    }
}
