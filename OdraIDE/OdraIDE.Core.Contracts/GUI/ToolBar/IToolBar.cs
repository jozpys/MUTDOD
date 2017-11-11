using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IToolBar : IControl
    {
        string Header { get; } // displayed on the left of the toolbar
        string Name { get; }   // displayed on a list of toolbars for selecting/deselecting
        IEnumerable<IToolBarItem> Items { get; }
    }
}
