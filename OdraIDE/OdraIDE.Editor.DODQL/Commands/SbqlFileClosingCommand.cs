using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Editor;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace OdraIDE.Editor.Sbql
{
    [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.FileClosingCommands, typeof(IFileClosingCommand))]
    [Export(typeof(SbqlFileClosingCommand))]
    public class SbqlFileClosingCommand : AbstractFileClosingCommand
    {
        [Import(CompositionPoints.Workbench.Commands.SaveSbqlFile, typeof(ICustomCommand))]
        private ICustomCommand saveCommand { get; set; }

        [Import(OdraIDE.Core.Services.Messaging.MessagingService, typeof(IMessagingService))]
        private Lazy<IMessagingService> messagingService { get; set; }

        public override bool OnClosing(OpenedFile file)
        {
            if (file.IsDirty)
            {
                string msg = file.FileName + " has been modified. Save changes?";
                DialogResult result = messagingService.Value.ShowDialog(msg, "Save file", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.None:
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        (saveCommand as SaveSbqlFileCommand).SaveFile();
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }


}
