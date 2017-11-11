using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Forms;
using OdraIDE.Core.FileDialog.Resources;

namespace OdraIDE.Core.FileDialog
{
    [Export(Services.FileDialog.FileDialogService, typeof(IFileDialogService))]
    class FileDialogService : IFileDialogService
    {
        #region IFileDialogService Members

        public string OpenFileDialog(string defaultExtension, string initialDirectory,
            Dictionary<string, string> filters, string title,
            bool addExtension, bool checkFileExists, bool checkPathExists)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                PopulateFileDialog(dlg, "", defaultExtension, initialDirectory,
                    filters, title, addExtension, checkFileExists, checkPathExists);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public string SaveFileDialog(string fileName, string defaultExtension, string initialDirectory,
            Dictionary<string, string> filters, string title,
            bool addExtension, bool checkFileExists, bool checkPathExists)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                PopulateFileDialog(dlg, fileName, defaultExtension, initialDirectory,
                    filters, title, addExtension, checkFileExists, checkPathExists);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
                else
                {
                    return null;
                }
            }
        }

        private void PopulateFileDialog(System.Windows.Forms.FileDialog dlg,
            string fileName,
            string defaultExtension, string initialDirectory, Dictionary<string, string> filters,
            string title, bool addExtension, bool checkFileExists, bool checkPathExists)
        {
            dlg.DefaultExt = defaultExtension;
            dlg.InitialDirectory = initialDirectory;
            dlg.Title = title;
            dlg.AddExtension = addExtension;
            dlg.CheckFileExists = checkFileExists;
            dlg.CheckPathExists = checkPathExists;
            dlg.FileName = fileName;

            dlg.FilterIndex = 0;
            int thisIndex = 0;
            StringBuilder filtValue = new StringBuilder();
            foreach (string k in filters.Keys)
            {
                if (filtValue.Length > 0)
                {
                    filtValue.Append("|");
                }
                filtValue.Append(filters[k] + " (*." + k + ")|*." + k);
                if (k == defaultExtension)
                {
                    dlg.FilterIndex = thisIndex;
                }
                thisIndex++;
            }

            // Add an All filter
            if (filtValue.Length > 0)
            {
                filtValue.Append("|");
            }
            filtValue.Append(Strings.All_Files + " (*.*)|*.*");

            dlg.Filter = filtValue.ToString();
        }

        #endregion
    }
}
