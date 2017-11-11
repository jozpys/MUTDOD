using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace OdraIDE.Core
{
    public interface IDocument : ILayoutItem
    {
        OpenedFile File { get; set; }

        /// <summary>
        /// Saves the specified file to the stream
        /// </summary>
        void Save(OpenedFile file, Stream stream);

        /// <summary>
        /// Load the specified file to document. New file without content.
        /// </summary>
        void Load(OpenedFile file);

        /// <summary>
        /// Load or reload the content of the specified file from the stream.
        /// </summary>
        void Load(OpenedFile file, Stream stream);

        void OnOpened(object sender, EventArgs e);
        void OnClosing(object sender, CancelEventArgs e);
        void OnClosed(object sender, EventArgs e);
    }
}
