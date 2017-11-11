using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core
{
    public delegate OpenedFile CreateFileForExtension(string extension, string fileName, bool isUntitled);

    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(FileFactory))]
    public class FileFactory
    {
        [ImportMany(typeof(CreateFileForExtension))]
        private IEnumerable<CreateFileForExtension> createFileMethods { get; set; }

        /// <summary>
        /// Invoke all imported factory methods. If one creates file then returns this object.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>OpenedFile for extension</returns>
        public OpenedFile CreateFileForExtension(string extension, string fileName, bool isUntitled)
        {
            OpenedFile file;

            foreach (CreateFileForExtension method in createFileMethods)
            {
                file = method(extension, fileName, isUntitled);
                if (file != null)
                {
                    return file;
                }
            }

            return new OpenedFile(FileName.Create(fileName), isUntitled);
        }
    }
}
