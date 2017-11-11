using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.IO;

namespace OdraIDE.Results
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand))]
    public class StartupCommand : AbstractExtension, IExecutableCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        //private IList<string> filesToOpen = new List<string>();

        public void Run(params object[] args)
        {
            
        }

        public void OnImportsSatisfied()
        {
            string[] clArgs = Environment.GetCommandLineArgs();
            for (int i = 1; i < clArgs.Length; i++)
            {
                string filePath = clArgs[i];
                if (File.Exists(filePath))
                {
                    //filesToOpen.Add(filePath);
                    fileService.Value.OpenFile(filePath);
                }
                else
                {
                    //TODO komunikat ze plik nie istnieje
                }
            }
        }
    }
}
