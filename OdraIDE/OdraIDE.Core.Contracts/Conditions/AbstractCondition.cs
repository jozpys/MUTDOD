using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public abstract class AbstractCondition : ICondition
    {
        public event EventHandler ConditionChanged = delegate { };
        public bool Condition
        {
            get
            {
                return m_Condition;
            }
            protected set
            {
                if (m_Condition != value)
                {
                    m_Condition = value;
                    ConditionChanged(this, new EventArgs());
                }
            }
        }
        private bool m_Condition = false;
    }
}
