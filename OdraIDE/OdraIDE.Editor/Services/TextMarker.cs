using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Editor
{
    public class TextMarker : TextSegment, ITextMarker
    {
        private Color? m_BackgroundColor;
        private Color? m_ForegroundColor;
        private TextMarkerType m_MarkerType;
        private Color m_markerColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public TextMarker(int startOffset, int length)
        {
            this.StartOffset = startOffset;
            this.Length = length;
            this.m_MarkerType = TextMarkerType.None;
        }

        public Color? BackgroundColor
        {
            get { return m_BackgroundColor; }
            set
            {
                if (m_BackgroundColor != value)
                {
                    m_BackgroundColor = value;
                    NotifyPropertyChanged(m_BackgroundColorArgs);
                }
            }
        }
        static readonly PropertyChangedEventArgs m_BackgroundColorArgs = 
            NotifyPropertyChangedHelper.CreateArgs<TextMarker>(o => o.BackgroundColor);

        public Color? ForegroundColor
        {
            get { return m_ForegroundColor; }
            set
            {
                if (m_ForegroundColor != value)
                {
                    m_ForegroundColor = value;
                    NotifyPropertyChanged(m_ForegroundColorArgs);
                }
            }
        }
        static readonly PropertyChangedEventArgs m_ForegroundColorArgs =
            NotifyPropertyChangedHelper.CreateArgs<TextMarker>(o => o.ForegroundColor);

        public object Tag { get; set; }

        public TextMarkerType MarkerType
        {
            get { return m_MarkerType; }
            set
            {
                if (m_MarkerType != value)
                {
                    m_MarkerType = value;
                    NotifyPropertyChanged(m_MarkerTypeArgs);
                }
            }
        }

        static readonly PropertyChangedEventArgs m_MarkerTypeArgs =
            NotifyPropertyChangedHelper.CreateArgs<TextMarker>(o => o.MarkerType);


        public Color MarkerColor
        {
            get { return m_markerColor; }
            set
            {
                if (m_markerColor != value)
                {
                    m_markerColor = value;
                    NotifyPropertyChanged(m_MarkerColorArgs);
                }
            }
        }

        static readonly PropertyChangedEventArgs m_MarkerColorArgs =
            NotifyPropertyChangedHelper.CreateArgs<TextMarker>(o => o.MarkerColor);


        public object ToolTip { get; set; }

        public void Redraw()
        {
            NotifyPropertyChanged(null);
        }

        protected void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            var evt = PropertyChanged;
            if (evt != null)
            {
                evt(this, e);
            }
        }
    }
}
