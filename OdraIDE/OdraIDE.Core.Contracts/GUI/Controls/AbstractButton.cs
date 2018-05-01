using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core 
{
    public abstract class AbstractButton : AbstractCommandControl, IButton
    {
        public AbstractButton()
            : base()
        {
            CanExecuteChanged += delegate
            {
                NotifyPropertyChanged(m_IconArgs);
            };
        }

        #region " Text "
        /// <summary>
        /// This is the text displayed in the button itself.
        /// Best to set this property in the derived class's constructor.
        /// </summary>
        public string Text
        {
            get
            {
                return m_Text;
            }
            protected set
            {
                if (m_Text != value)
                {
                    m_Text = value;
                    NotifyPropertyChanged(m_TextArgs);
                }
            }
        }
        private string m_Text = null;
        static readonly PropertyChangedEventArgs m_TextArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractButton>(o => o.Text);
        #endregion

        #region " Icon "
        /// <summary>
        /// Optional icon that can be displayed in the button.
        /// </summary>
        public BitmapSource Icon
        {
            get
            {
                if (Command.CanExecute(this))
                {
                    return IconFull;
                }
                else
                {
                    return IconGray;
                }
            }
            protected set
            {
                if (IconFull != value)
                {
                    IconFull = value;
                    NotifyPropertyChanged(m_IconArgs);
                }
            }
        }
        static readonly PropertyChangedEventArgs m_IconArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractButton>(o => o.Icon);

        private BitmapSource IconFull
        {
            get
            {
                return m_IconFull;
            }
            set
            {
                if (m_IconFull != value)
                {
                    m_IconFull = value;
                    if (m_IconFull != null)
                    {
                        IconGray = ConvertFullToGray(m_IconFull);
                    }
                    else
                    {
                        IconGray = null;
                    }
                }
            }
        }
        private BitmapSource m_IconFull = null;
        private BitmapSource IconGray { get; set; }

        private BitmapSource ConvertFullToGray(BitmapSource full)
        {
            FormatConvertedBitmap gray = new FormatConvertedBitmap();

            gray.BeginInit();
            gray.Source = full;
            gray.DestinationFormat = PixelFormats.Gray32Float;
            gray.EndInit();

            return gray;
        }

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

    }
}
