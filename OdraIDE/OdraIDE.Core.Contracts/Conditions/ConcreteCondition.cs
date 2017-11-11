using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    /// <summary>
    /// Helper class to allow us to create conditions 
    /// and control them.
    /// </summary>
    public class ConcreteCondition : AbstractCondition
    {
        public ConcreteCondition()
        {
        }

        public ConcreteCondition(bool condition)
        {
            Condition = condition;
        }

        public void ToggleCondition()
        {
            Condition = !Condition;
        }

        public void SetCondition(bool value)
        {
            Condition = value;
        }
    }
}
