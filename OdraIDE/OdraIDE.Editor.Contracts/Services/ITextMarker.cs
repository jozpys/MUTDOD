using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using System.ComponentModel;

namespace OdraIDE.Editor
{
    /// <summary>
    /// Represents a text marker.
    /// </summary>
    public interface ITextMarker : ISegment
    {
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets/Sets the background color.
        /// </summary>
        Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets/Sets the foreground color.
        /// </summary>
        Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets/Sets the type of the marker. Use TextMarkerType.None for normal markers.
        /// </summary>
        TextMarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets/Sets the color of the marker.
        /// </summary>
        Color MarkerColor { get; set; }

        /// <summary>
        /// Gets/Sets an object with additional data for this text marker.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets/Sets an object that will be displayed as tooltip in the text editor.
        /// </summary>
        object ToolTip { get; set; }
    }

    public enum TextMarkerType
    {
        /// <summary>
        /// Use no marker
        /// </summary>
        None,
        /// <summary>
        /// Use squiggly underline marker
        /// </summary>
        SquigglyUnderline
    }
}
