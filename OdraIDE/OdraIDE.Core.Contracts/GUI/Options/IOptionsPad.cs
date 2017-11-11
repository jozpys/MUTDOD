using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IOptionsPad : IPad
    {
        void Commit();
        void Cancel();

        event EventHandler OptionChanged;
    }
}
