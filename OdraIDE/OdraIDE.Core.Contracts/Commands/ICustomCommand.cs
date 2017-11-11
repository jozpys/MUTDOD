using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace OdraIDE.Core
{
    public interface ICustomCommand : ICommand
    {
        ICondition EnableCondition { get; set; }
    }
}
