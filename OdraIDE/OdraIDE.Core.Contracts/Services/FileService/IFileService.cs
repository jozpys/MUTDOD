using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IFileService
    {
        event EventHandler<FileEventArgs> FileCreated;
        event EventHandler<FileEventArgs> FileClosed;
        event EventHandler TextSelectionChanged;

        ICollection<OpenedFile> OpenedFiles { get; }
        OpenedFile GetActiveFile();

        IDocument OpenFile(string fileName);
        IDocument OpenFile(string fileName, bool switchToOpenedDocument);
        IDocument NewFile();
        IDocument NewFile(string extension);

        OpenedFile GetOpenedFile(FileName fileName);

        void SaveActiveFile();

        bool RenameFile(string oldName, string newName, bool isDirectory);
        bool CopyFile(string oldName, string newName, bool isDirectory, bool overwrite);

        void RemoveFile(string fileName, bool isDirectory);
    }
}
