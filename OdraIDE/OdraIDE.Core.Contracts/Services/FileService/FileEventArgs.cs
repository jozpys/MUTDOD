using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public class FileEventArgs : EventArgs
    {
        string fileName = null;

        bool isDirectory;

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return isDirectory;
            }
        }

        public FileEventArgs(string fileName, bool isDirectory)
        {
            this.fileName = fileName;
            this.isDirectory = isDirectory;
        }
    }

    public class FileNameChangeEventArgs : EventArgs
    {
        FileName oldFileName = null;

        public FileName OldFileName
        {
            get
            {
                return oldFileName;
            }
        }

        FileName newFileName = null;

        public FileName NewFileName
        {
            get
            {
                return newFileName;
            }
        }

        public FileNameChangeEventArgs(FileName oldFileName, FileName newFileName)
        {
            this.oldFileName = oldFileName;
            this.newFileName = newFileName;
        }
    }
}
