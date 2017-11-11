using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Utilities;
using OdraIDE.Core;
using System.IO;

using System.ComponentModel.Composition;

namespace OdraIDE.Core.FileService
{
    [Export(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
    public class FileService : IFileService
    {
        public FileService()
        {
            FileCreated += new EventHandler<FileEventArgs>(OnFileCreated);
        }

        [Import(OdraIDE.Core.Services.Logging.LoggingService, typeof(ILoggingService))]
        private Lazy<ILoggingService> loggingService { get; set; }

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        public ILayoutManager LayoutManager { get; set; }

        [Import(typeof(DocumentFactory))]
        public DocumentFactory DocumentFactory { get; set; }

        [Import(typeof(FileFactory))]
        public FileFactory FileFactory { get; set; }

        #region IFileService Members

        /// <summary>
        /// Opens a view content for the specified file and switches to the opened view
        /// or switches to and returns the existing view content for the file if it is already open.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <returns>The existing or opened <see cref="IViewContent"/> for the specified file.</returns>
        public IDocument OpenFile(string fileName)
        {
            return OpenFile(fileName, true);
        }

        /// <summary>
        /// Opens a view content for the specified file
        /// or returns the existing view content for the file if it is already open.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="switchToOpenedView">Specifies whether to switch to the view for the specified file.</param>
        /// <returns>The existing or opened <see cref="IViewContent"/> for the specified file.</returns>
        public IDocument OpenFile(string fileName, bool switchToOpenedDocument)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("File not found", fileName);
            }
            fileName = FileHelper.NormalizePath(fileName);
            loggingService.Value.Info("Open file " + fileName);

            OpenedFile file = GetOpenedFile(fileName);
            if (file == null )
            {
                file = GetOrCreateOpenedFile(fileName);
                file.TextSelectionChanged += TextSelectionChanged;
                DocumentFactory.CreateDocumentForFile(file);
            }
            
            if (file.Document != null)
            {
                LayoutManager.ShowDocument(file.Document, switchToOpenedDocument);
            }

            return file.Document;
        }

        public IDocument NewFile()
        {
            return NewFile(string.Empty);
        }

        public IDocument NewFile(string extension)
        {
            string defaultName;
            int counter = 0;
            do
            {
                defaultName = "Untitled" + counter + (extension ?? string.Empty);
                counter++;
            }
            while (GetOpenedFile(defaultName) != null);

            OpenedFile file = CreateUntitledOpenedFile(extension, defaultName);
            file.TextSelectionChanged += TextSelectionChanged;
            DocumentFactory.CreateDocumentForFile(file);

            if (file.Document != null)
            {
                LayoutManager.ShowDocument(file.Document, true);
            }

            return file.Document;
        }

        public void SaveActiveFile()
        {
            OpenedFile activeFile = GetActiveFile();
            SaveFile(activeFile);
        }

        private void SaveFile(OpenedFile file)
        {
            if (file != null && file.IsDirty)
            {
                file.SaveToDisk();
            }
        }

        public bool RenameFile(string oldName, string newName, bool isDirectory)
        {
            throw new NotImplementedException();
        }

        public bool CopyFile(string oldName, string newName, bool isDirectory, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string fileName, bool isDirectory)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region OpenedFile

        private Dictionary<FileName, OpenedFile> openedFileDict = new Dictionary<FileName, OpenedFile>();

        /// <summary>
        /// Gets a collection containing all currently opened files.
        /// The returned collection is a read-only copy of the currently opened files -
        /// it will not reflect future changes of the list of opened files.
        /// </summary>
        public ICollection<OpenedFile> OpenedFiles
        {
            get
            {
                return openedFileDict.Values.ToArray();
            }
        }

        /// <summary>
        /// Gets an opened file, or returns null if the file is not opened.
        /// </summary>
        public OpenedFile GetOpenedFile(string fileName)
        {
            return GetOpenedFile(FileName.Create(fileName));
        }

        /// <summary>
        /// Gets an opened file, or returns null if the file is not opened.
        /// </summary>
        public OpenedFile GetOpenedFile(FileName fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            OpenedFile file;
            openedFileDict.TryGetValue(fileName, out file);
            return file;
        }

        /// <summary>
        /// Gets or creates an opened file.
        /// Warning: the opened file will be a file without any views attached.
        /// Make sure to attach a view to it, or call CloseIfAllViewsClosed on the OpenedFile to
        /// unload the OpenedFile instance if no views were attached to it.
        /// </summary>
        public OpenedFile GetOrCreateOpenedFile(string fileName)
        {
            return GetOrCreateOpenedFile(FileName.Create(fileName));
        }

        /// <summary>
        /// Gets or creates an opened file.
        /// Warning: the opened file will be a file without any views attached.
        /// Make sure to attach a view to it, or call CloseIfAllViewsClosed on the OpenedFile to
        /// unload the OpenedFile instance if no views were attached to it.
        /// </summary>
        public OpenedFile GetOrCreateOpenedFile(FileName fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            OpenedFile file;
            if (!openedFileDict.TryGetValue(fileName, out file))
            {
                openedFileDict[fileName] = file = FileFactory.CreateFileForExtension(Path.GetExtension(fileName), fileName, false);
                FileCreated(file, new FileEventArgs(file.FileName, false));
            }
            return file;
        }

        /// <summary>
        /// Creates a new untitled OpenedFile.
        /// </summary>
        public OpenedFile CreateUntitledOpenedFile(string extension, string defaultName)
        {
            if (defaultName == null)
                throw new ArgumentNullException("defaultName");

            OpenedFile file = FileFactory.CreateFileForExtension(extension, defaultName, true);

            openedFileDict[file.FileName] = file;
            FileCreated(file, new FileEventArgs(file.FileName, false));
            return file;
        }

        /// <summary>
        /// Called by OpenedFile.set_FileName to update the dictionary.
        /// </summary>
        internal void OpenedFileFileNameChange(OpenedFile file, FileName oldName, FileName newName)
        {
            if (oldName == null) return; // File just created with NewFile where name is being initialized.

            //LoggingService.Debug("OpenedFileFileNameChange: " + oldName + " => " + newName);

            if (openedFileDict[oldName] != file)
                throw new ArgumentException("file must be registered as oldName");
            if (openedFileDict.ContainsKey(newName))
                throw new ArgumentException("there already is a file with the newName");

            openedFileDict.Remove(oldName);
            openedFileDict[newName] = file;
        }

        /// <summary>
        /// Gets an opened file, or returns null if the file is not opened.
        /// </summary>
        public OpenedFile GetActiveFile()
        {
            if (LayoutManager.GetActiveDocument() == null)
            {
                return null;
            }
            return LayoutManager.GetActiveDocument().File;
        }

        #endregion

        #region Events

        public event EventHandler TextSelectionChanged;
        public event EventHandler<FileEventArgs> FileCreated;
        public event EventHandler<FileEventArgs> FileClosed;

        private void OnFileClosed(object sender, EventArgs e)
        {
            OpenedFile file = sender as OpenedFile;
            openedFileDict.Remove(file.FileName);
            FileClosed(file, new FileEventArgs(file.FileName, false));
        }

        private void OnFileCreated(object sender, FileEventArgs e)
        {
            OpenedFile file = sender as OpenedFile;
            file.FileClosed += new EventHandler(OnFileClosed);
            file.FileNameChanged += new EventHandler<FileNameChangeEventArgs>(OnFileNameChanged);
        }

        private void OnFileNameChanged(object sender, FileNameChangeEventArgs e)
        {
            OpenedFileFileNameChange(sender as OpenedFile, e.OldFileName, e.NewFileName);
        }

        #endregion
    }
}
