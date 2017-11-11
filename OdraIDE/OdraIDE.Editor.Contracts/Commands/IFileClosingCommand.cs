using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;

namespace OdraIDE.Editor
{
    public interface IFileClosingCommand : IExtension
    {
        /// <summary>
        /// Methods 
        /// </summary>
        /// <param name="file"></param>
        /// <returns>false if wants stop closing file</returns>
        bool OnClosing(OpenedFile file);
    }
}
