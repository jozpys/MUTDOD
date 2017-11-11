using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Utilities;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using AvalonDock;

namespace OdraIDE.Core
{
    public abstract class AbstractPad : AbstractLayoutItem, IPad
    {
        #region " Location "
        /// <summary>
        /// 
        /// </summary>
        public PadLocation Location
        {
            get
            {
                return m_Location;
            }
            protected set
            {
                if (m_Location != value)
                {
                    m_Location = value;
                    NotifyPropertyChanged(m_LocationArgs);
                }
            }
        }
        private PadLocation m_Location = PadLocation.Bottom;
        static readonly PropertyChangedEventArgs m_LocationArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractPad>(o => o.Location);

        #endregion

        #region " Icon "
        /// <summary>
        /// Optional icon that can be displayed in the button.
        /// </summary>
        public object Icon
        {
            get
            {
                    return m_Icon;
            }

            protected set
            {
                if (m_Icon != value)
                {
                    m_Icon = value;
                    NotifyPropertyChanged(m_IconArgs);
                }
            }
        }

        private object m_Icon = null;
        static readonly PropertyChangedEventArgs m_IconArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractButton>(o => o.Icon);

        /// <summary>
        /// This is a helper function so you can assign the Icon directly
        /// from a Bitmap, such as one from a resources file.
        /// </summary>
        /// <param name="value"></param>
        protected void SetIconFromBitmap(System.Drawing.Bitmap value)
        {
            BitmapSource b = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                value.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Icon = b;
        }

        #endregion

        #region " DesideredState "
        /// <summary>
        /// 
        /// </summary>
        public DockableContentState DesideredState
        {
            get
            {
                return m_DesideredState;
            }
            protected set
            {
                if (m_DesideredState != value)
                {
                    m_DesideredState = value;
                    NotifyPropertyChanged(m_DesideredStateArgs);
                }
            }
        }
        private DockableContentState m_DesideredState = DockableContentState.Docked;
        static readonly PropertyChangedEventArgs m_DesideredStateArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractPad>(o => o.DesideredState);

        #endregion
    }
}
