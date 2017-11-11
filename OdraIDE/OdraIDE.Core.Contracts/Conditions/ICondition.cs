using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface ICondition
    {
        event EventHandler ConditionChanged;
        bool Condition { get; }
    }
}
