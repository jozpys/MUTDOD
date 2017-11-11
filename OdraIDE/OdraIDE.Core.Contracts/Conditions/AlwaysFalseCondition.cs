using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public sealed class AlwaysFalseCondition : ICondition
    {
        public event EventHandler ConditionChanged = delegate { };
        public bool Condition { get { return false; } }
    }
}
