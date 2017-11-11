using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace OdraIDE.Core
{
    public interface IButton : IControl
    {
        string Text { get; }
        BitmapSource Icon { get; }
    }
}
