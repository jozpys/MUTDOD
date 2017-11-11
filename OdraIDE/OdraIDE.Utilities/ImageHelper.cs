using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

namespace OdraIDE.Utilities
{
    public static class ImageHelper
    {
        public static Image GetImageFromResources(System.Drawing.Bitmap value)
        {
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(
                                value.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

            Image img = new Image();
            img.Source = bs;

            return img;
        }

    }
}
