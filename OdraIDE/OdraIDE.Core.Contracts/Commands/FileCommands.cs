using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace OdraIDE.Core
{
    /* ======================================================= OPEN ======================================================= */


    public abstract class AbstractOpenFileCommand : BaseCommand
    {
        public abstract IFileDialogService FileDialogService { get; }

        public abstract IFileService FileService { get; }

        public virtual void OpenFile(string defaultExtension, string initialDirectory, Dictionary<string, string> filters,
            string dialogTitle = "Open File", bool addExtension = true, bool checkFileExists = true, bool checkPathExists = true)
        {
            string fileName = FileDialogService.OpenFileDialog(
                    defaultExtension, initialDirectory, filters,
                    dialogTitle,
                    addExtension, checkFileExists, checkPathExists);
            if (fileName != null)
            {
                FileService.OpenFile(fileName);
            }
        }
    }

    /* ======================================================= SAVE ======================================================= */

    public abstract class AbstractSaveFileAsCommand : BaseCommand
    {
        public abstract IFileDialogService FileDialogService { get; }

        public abstract IFileService FileService { get; }

        public virtual void SaveFileAs(OpenedFile file, string defaultExtension, string initialDirectory, Dictionary<string, string> filters, 
            string dialogTitle = "Save File", bool addExtension = true, bool checkFileExists = false, bool checkPathExists = true)
        {
            string fileName = FileDialogService.SaveFileDialog(
                                file.FileName,
                                defaultExtension, initialDirectory, filters,
                                dialogTitle,
                                addExtension, checkFileExists, checkPathExists);

            if (fileName != null)
            {
                file.FileName = FileName.Create(fileName);
                file.SaveToDisk();
            }
        }
    }

    public abstract class AbstractSaveFileCommand : AbstractSaveFileAsCommand
    {
        public virtual void SaveFile(OpenedFile file, string defaultExtension, string initialDirectory, Dictionary<string, string> filters,
            string dialogTitle = "Save File", bool addExtension = true, bool checkFileExists = false, bool checkPathExists = true)
        {
            if (file.IsUntitled)
            {
                SaveFileAs(file, defaultExtension, initialDirectory, filters, dialogTitle, addExtension, checkFileExists, checkPathExists);
            }
            else
            {
                file.SaveToDisk();
            }
        }
    }

}
