using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvalonDock;
using System.IO;

namespace OdraIDE.Core
{
    public interface ILayoutManager
    {
        event EventHandler Loaded;
        event EventHandler LayoutUpdated;

        ReadOnlyCollection<IPad> Pads { get; }
        ReadOnlyCollection<IDocument> Documents { get; }

        void ShowPad(IPad pad, DockableContentState desideredState = DockableContentState.Docked);
        void HidePad(IPad pad);
        void HideAllPads();
        IDocument ShowDocument(IDocument document, bool switchToOpenedDocument);
        IDocument GetActiveDocument();
        void CloseDocument(IDocument document);
        void CloseAllDocuments();

        bool IsVisible(IPad pad);
        bool IsVisible(IDocument document);

        void SetMainWindowTitle(string title);

        /// <summary>
        /// Called on recomposition by the Workbench
        /// </summary>
        void SetAllPadsDocuments(
            IEnumerable<Lazy<IPad, IPadMeta>> AllPads,
            IEnumerable<Lazy<IDocument, IDocumentMeta>> AllDocuments);

        void SaveLayout(StreamWriter sw); // returns a blob
        void RestoreLayout(StreamReader sr);
    }
}
