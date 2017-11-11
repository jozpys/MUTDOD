using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IRadioButton : IToggleButton
    {
        string GroupName { get; }
    }
}
