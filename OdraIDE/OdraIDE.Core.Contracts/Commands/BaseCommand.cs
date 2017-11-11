using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using OdraIDE.Utilities;
using System.ComponentModel;

namespace OdraIDE.Core
{
    public delegate void ExecuteHandler();

    public class BaseCommand : ICustomCommand
    {
        public event ExecuteHandler ExecuteCommand = delegate() {  };

        #region " ICommand Implementation "

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return EnableCondition.Condition;
        }

        public void Execute(object parameter)
        {
            ExecuteCommand();
        }

        public void OnCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged(sender, e);
        }

        #endregion

        #region " EnableCondition "
        /// <summary>
        /// Defaults to AlwaysTrueCondition.
        /// Set this to any IOdraIDECondition object, and it will control
        /// the CanExecute property from the ICommand interface, and 
        /// will raise the CanExecuteChanged event when appropriate.
        /// </summary>
        public ICondition EnableCondition
        {
            get
            {
                if (m_EnableCondition == null)
                {
                    // Lazy initialize this property.
                    // We could do this in the constructor, but 
                    // I like having it all contained in one
                    // section of code.
                    EnableCondition = new AlwaysTrueCondition();
                }
                return m_EnableCondition;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (m_EnableCondition != value)
                {
                    if (m_EnableCondition != null)
                    {
                        //remove the old event handler
                        m_EnableCondition.ConditionChanged -= OnEnableConditionChanged;
                    }
                    m_EnableCondition = value;
                    //add the new event handler
                    m_EnableCondition.ConditionChanged += OnEnableConditionChanged;
                    CanExecuteChanged(this, new EventArgs());
                }
            }
        }
        private ICondition m_EnableCondition = null;

        private void OnEnableConditionChanged(object sender, EventArgs e)
        {
            CanExecuteChanged(sender, e);
        }
        #endregion
        
    }
}
