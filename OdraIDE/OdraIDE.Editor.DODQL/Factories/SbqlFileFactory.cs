using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor.Sbql
{
    public class SbqlFileFactory
    {
        [Export(typeof(CreateFileForExtension))]
        public OpenedFile CreateFileForExtension(string extension, string fileName, bool isUntitled)
        {
            if (extension == ".dodql")
            {
                return new SbqlOpenedFile(FileName.Create(fileName), isUntitled);
            }

            return null;
        }
    }
}
