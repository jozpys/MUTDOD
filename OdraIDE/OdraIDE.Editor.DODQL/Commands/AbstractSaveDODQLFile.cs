using System;
using System.Collections.Generic;
using OdraIDE.Core;
using OdraIDE.Editor.Sbql;

namespace OdraIDE.Editor.DODQL.Commands
{
    public abstract class AbstractSaveDODQLFile : AbstractSaveFileCommand
    {
        protected abstract Lazy<IFileService> fileService { get; set; }

        public void SaveFile()
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.Add("dodql", "DODQL Files");

            OpenedFile activeFile = fileService.Value.GetActiveFile();
            if (activeFile != null)
            {
                SaveFile(activeFile, "dodql", @"d:\", filters);
            }
        }

        public void SaveAllFiles()
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.Add("dodql", "DODQL Files");

            foreach (OpenedFile file in fileService.Value.OpenedFiles)
            {
                if (file is SbqlOpenedFile)
                {
                    SaveFile(file, "dodql", @"d:\", filters);
                }
            }
        }
    }
}
