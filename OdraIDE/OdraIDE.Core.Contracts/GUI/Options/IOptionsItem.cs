using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IOptionsItem : IControl
    {
        string Header { get; }
        IEnumerable<IOptionsItem> Items { get; }
        IOptionsPad Pad { get; }
        void CommitChanges();
        void CancelChanges();
        event EventHandler OptionChanged;
    }
}
