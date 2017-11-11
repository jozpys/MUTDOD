using System.Collections.Generic;

namespace OdraIDE.Core
{
    public interface IFileDialogService
    {
        /// <summary>
        /// Asks the user to select a file
        /// </summary>
        /// <param name="defaultExtension">Example: "txt"</param>
        /// <param name="initialDirectory">Example: @"C:\"</param>
        /// <param name="filters">Examples: 
        ///     "txt", "Text Documents" 
        ///     "bmp", "Bitmaps" 
        ///     (Note: automatically inserts the "All Files" option)
        ///     </param>
        /// <param name="title">Example: "Open file..."</param>
        /// <param name="addExtension">Set to true to auto-add the extension</param>
        /// <param name="checkFileExists">Warn if file doesn't exist.</param>
        /// <param name="checkPathExists">Warn if path doesn't exist.</param>
        /// <returns>null if user cancels, otherwise the filename</returns>
        string OpenFileDialog(
            string defaultExtension,
            string initialDirectory,
            Dictionary<string, string> filters,
            string title,
            bool addExtension,
            bool checkFileExists,
            bool checkPathExists);

        string SaveFileDialog(
            string fileName,
            string defaultExtension,
            string initialDirectory,
            Dictionary<string, string> filters,
            string title,
            bool addExtension,
            bool checkFileExists,
            bool checkPathExists);

    }
}
