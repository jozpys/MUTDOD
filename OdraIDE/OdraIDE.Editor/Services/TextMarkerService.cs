using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System.Windows.Media;
using System.Windows;

namespace OdraIDE.Editor
{
    [Export(OdraIDE.Editor.Services.Markers.TextMarkerService, typeof(ITextMarkerService))]
    public class TextMarkerService : DocumentColorizingTransformer, IBackgroundRenderer,  ITextMarkerService
    {
        Dictionary<TextDocument, TextSegmentCollection<TextMarker>> markersMap = new Dictionary<TextDocument, TextSegmentCollection<TextMarker>>();

        public ITextMarker Create(ISourceEditor editor, int startOffset, int length)
        {
            int textLength = editor.TextEditor.Document.TextLength;
            if (startOffset < 0 || startOffset > textLength)
                throw new ArgumentOutOfRangeException("startOffset", startOffset, "Value must be between 0 and " + textLength);
            if (length < 0 || startOffset + length > textLength)
                throw new ArgumentOutOfRangeException("length", length, "length must not be negative and startOffset+length must not be after the end of the document");

            TextMarker m = new TextMarker(startOffset, length);
            
            if(!markersMap.ContainsKey(editor.TextEditor.Document))
            {
                markersMap.Add(editor.TextEditor.Document, new TextSegmentCollection<TextMarker>(editor.TextEditor.Document));
            }
            markersMap[editor.TextEditor.Document].Add(m);
            
            // no need to mark segment for redraw: the text marker is invisible until a property is set
            return m;
        }

        public void Remove(TextDocument doc, ITextMarker marker)
        {
            if (marker == null)
                throw new ArgumentNullException("marker");
            TextMarker m = marker as TextMarker;
            TextSegmentCollection<TextMarker> markers;
            if (markersMap.TryGetValue(doc, out markers))
            {
                markers.Remove(m);
                m.Redraw();
            }
        }

        public void RemoveAll(Predicate<ITextMarker> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            foreach (TextDocument doc in markersMap.Keys)
            {
                TextSegmentCollection<TextMarker> markers;
                if(markersMap.TryGetValue(doc, out markers))
                {
                    foreach (TextMarker m in markers)
                    {
                        if (predicate(m))
                        {
                            markers.Remove(m);
                            m.Redraw();
                        }
                    }
                }
            }
        }

        public IEnumerable<ITextMarker> TextMarkers(ISourceEditor editor)
        {
            TextSegmentCollection<TextMarker> markers;
            if (markersMap.TryGetValue(editor.TextEditor.Document, out markers))
            {
                return markers;
            }
            else
            {
                return null;
            }
        }

        public KnownLayer Layer
        {
            get
            {
                // draw behind selection
                return KnownLayer.Selection;
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (textView == null)
                throw new ArgumentNullException("textView");
            if (drawingContext == null)
                throw new ArgumentNullException("drawingContext");
            if (markersMap == null || !textView.VisualLinesValid)
                return;
            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
                return;
            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.Offset + visualLines.Last().LastDocumentLine.Length;

            TextSegmentCollection<TextMarker> markers;
            if (!markersMap.TryGetValue(textView.Document, out markers))
            {
                return;
            }

            foreach (TextMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart))
            {
                if (marker.BackgroundColor != null)
                {
                    BackgroundGeometryBuilder geoBuilder = new BackgroundGeometryBuilder();
                    geoBuilder.AlignToWholePixels = true;
                    geoBuilder.CornerRadius = 3;
                    geoBuilder.AddSegment(textView, marker);
                    Geometry geometry = geoBuilder.CreateGeometry();
                    if (geometry != null)
                    {
                        Color color = marker.BackgroundColor.Value;
                        SolidColorBrush brush = new SolidColorBrush(color);
                        brush.Freeze();
                        drawingContext.DrawGeometry(brush, null, geometry);
                    }
                }
                if (marker.MarkerType != TextMarkerType.None)
                {
                    foreach (Rect r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
                    {
                        Point startPoint = r.BottomLeft;
                        Point endPoint = r.BottomRight;

                        Pen usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
                        usedPen.Freeze();
                        switch (marker.MarkerType)
                        {
                            case TextMarkerType.SquigglyUnderline:
                                double offset = 2.5;

                                int count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);

                                StreamGeometry geometry = new StreamGeometry();

                                using (StreamGeometryContext ctx = geometry.Open())
                                {
                                    ctx.BeginFigure(startPoint, false, false);
                                    ctx.PolyLineTo(CreatePoints(startPoint, endPoint, offset, count).ToArray(), true, false);
                                }

                                geometry.Freeze();

                                drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
                                break;
                        }
                    }
                }
            }
        }

        IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
                yield return new Point(start.X + i * offset, start.Y - ((i + 1) % 2 == 0 ? offset : 0));
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            TextSegmentCollection<TextMarker> markers;
            //TODO:
            return;
            //if (!markersMap.TryGetValue(line.Document, out markers))
            //{
            //    return;
            //}
            if (markers == null)
                return;
            int lineStart = line.Offset;
            int lineEnd = lineStart + line.Length;
            foreach (TextMarker marker in markers.FindOverlappingSegments(lineStart, line.Length))
            {
                Brush foregroundBrush = null;
                if (marker.ForegroundColor != null)
                {
                    foregroundBrush = new SolidColorBrush(marker.ForegroundColor.Value);
                    foregroundBrush.Freeze();
                }
                ChangeLinePart(
                    Math.Max(marker.StartOffset, lineStart),
                    Math.Min(marker.EndOffset, lineEnd),
                    element =>
                    {
                        if (foregroundBrush != null)
                        {
                            element.TextRunProperties.SetForegroundBrush(foregroundBrush);
                        }
                    }
                );
            }
        }

        public IEnumerable<ITextMarker> GetMarkersAtOffset(TextDocument document, int offset)
        {
            IEnumerable<ITextMarker> m = TextMarkersFor(document);
            if (m != null)
            {
                return (m as TextSegmentCollection<TextMarker>).FindSegmentsContaining(offset);
            }
            else
            {
                return null;
            }
            
        }

        public IEnumerable<ITextMarker> TextMarkersFor(TextDocument document)
        {
            TextSegmentCollection<TextMarker> markers;
            if (markersMap.TryGetValue(document, out markers))
            {
                return markers;
            }
            else
            {
                return null;
            }
        }

    }
}
