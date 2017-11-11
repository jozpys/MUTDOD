using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using AvalonDock;

namespace OdraIDE.Core
{
    public interface IPad : ILayoutItem
    {
        DockableContentState DesideredState { get; }
        PadLocation Location { get; }
        object Icon { get; }
    }

    public enum PadLocation
    {
        TopLeft,
        TopRight,
        Bottom
    }
}
