using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;

namespace OdraIDE.Editor
{
    public interface ITextMarkerService
    {
        /// <summary>
        /// Creates a new text marker. The text marker will be invisible at first,
        /// you need to set one of the Color properties to make it visible.
        /// </summary>
        ITextMarker Create(ISourceEditor editor, int startOffset, int length);

        /// <summary>
        /// Gets the list of text markers for text document.
        /// </summary>
        IEnumerable<ITextMarker> TextMarkersFor(TextDocument document);

        /// <summary>
        /// Removes the specified text marker.
        /// </summary>
        void Remove(TextDocument document, ITextMarker marker);

        /// <summary>
        /// Removes all text markers that match the condition.
        /// </summary>
        void RemoveAll(Predicate<ITextMarker> predicate);

        IEnumerable<ITextMarker> GetMarkersAtOffset(TextDocument document, int offset);
    }
}
