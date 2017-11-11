using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace OdraIDE.Core
{
    public interface IMenuItem : IControl
    {
       // ICommand Command { get; set; }
        string Header { get; }
        IEnumerable<IMenuItem> Items { get; }
        object Icon { get; }
        bool IsCheckable { get; }
        bool IsChecked { get; set; }
        bool IsSeparator { get; }
        object Context { get; set; } // only used for context menus
    }
}
