using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace OdraIDE.Core
{
    public class OpenedFile
    {
        public OpenedFile(FileName fileName)
        {
            this.FileName = fileName;
        }

        public OpenedFile(FileName fileName, bool isUntitled)
        {
            this.FileName = fileName;
            this.IsUntitled = isUntitled;
        }

        protected IDocument m_document;
        bool inLoadOperation;
        bool inSaveOperation;
        public event EventHandler TextSelectionChanged = delegate { };

        public void FireSelectionChanged(object sender, EventArgs e)
        {
            TextSelectionChanged(sender, e);
        }

        /// <summary>
        /// Gets the document that currently edits this file.
        /// </summary>
        public IDocument Document
        {
            get { return m_document; }
            //set
            //{
            //    if (value == null)
            //    {
            //        throw new ArgumentNullException("Document");
            //    }
            //    m_document = value;
            //}
        }

        /// <summary>
        /// Opens the file for reading.
        /// </summary>
        public virtual Stream OpenRead()
        {
            return new FileStream(FileName, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Gets if the file is untitled. Untitled files show a "Save as" dialog when they are saved.
        /// </summary>
        public bool IsUntitled
        {
            get { return m_isUntitled; }
            protected set { m_isUntitled = value; }
        }

        private bool m_isUntitled;


        #region IsDirty

        private bool m_isDirty;
        public event EventHandler IsDirtyChanged;

        /// <summary>
        /// Gets/sets if the file is has unsaved changes.
        /// </summary>
        public bool IsDirty
        {
            get { return m_isDirty; }
            set
            {
                if (m_isDirty != value)
                {
                    m_isDirty = value;

                    if (IsDirtyChanged != null)
                    {
                        IsDirtyChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Marks the file as dirty.
        /// </summary>
        public virtual void MakeDirty()
        {
            this.IsDirty = true;
        }
        #endregion

        public event EventHandler FileClosed;
        public event EventHandler DocumentInitialized;

        public void RaiseFileClosed()
        {
            if (FileClosed != null)
            {
                FileClosed(this, EventArgs.Empty);
            }
        }

        public void RaiseDocumentInitialized()
        {
            if (DocumentInitialized != null)
            {
                DocumentInitialized(this, EventArgs.Empty);
            }
        }

        private FileName m_fileName;

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public FileName FileName
        {
            get { return m_fileName; }
            set
            {
                if (m_fileName != value)
                {
                    ChangeFileName(value);
                    IsUntitled = false;
                }
            }
        }

        protected virtual void ChangeFileName(FileName newValue)
        {
            FileName oldFileName = m_fileName;
            m_fileName = newValue;

            if (FileNameChanged != null)
            {
                FileNameChanged(this, new FileNameChangeEventArgs(oldFileName, newValue));
            }
        }

        /// <summary>
        /// Occurs when the file name has changed.
        /// </summary>
        public event EventHandler<FileNameChangeEventArgs> FileNameChanged;

        public virtual void Save()
        {

        }


        /// <summary>
        /// Use this method to save the file to disk using a new name.
        /// </summary>
        protected void SaveToDisk(string newFileName)
        {
            this.FileName = new FileName(newFileName);
            this.IsUntitled = false;
            SaveToDisk();
        }

        /// <summary>
        /// Save the file to disk using the current name.
        /// </summary>
        public virtual void SaveToDisk()
        {
            if (IsUntitled)
                throw new InvalidOperationException("Cannot save an untitled file to disk!");

            string saveAs = FileName;
            using (FileStream fs = new FileStream(saveAs, FileMode.Create, FileAccess.Write))
            {
                Document.Save(this, fs);
            }

            IsDirty = false;
        }

        /// <summary>
        /// Forces initialization of the specified view.
        /// </summary>
        public virtual void ForceInitializeDocument(IDocument doc)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            if (m_document != doc)
            {
                m_document = doc;
                try
                {
                    inLoadOperation = true;
                    if (IsUntitled)
                    {
                        doc.Load(this);
                    }
                    else
                    {
                        using (Stream sourceStream = OpenRead())
                        {
                            doc.Load(this, sourceStream);
                        }
                    }
                }
                finally
                {
                    inLoadOperation = false;
                }
                RaiseDocumentInitialized();
            }
        }

        public string GetFileNameWithoutPath()
        {
            return Path.GetFileName(FileName);
        }

    }
}
