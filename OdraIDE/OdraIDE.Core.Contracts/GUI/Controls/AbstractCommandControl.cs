using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OdraIDE.Utilities;
using System.Windows.Input;

namespace OdraIDE.Core
{
    public abstract class AbstractCommandControl : AbstractControl, IControl
    {
        public event EventHandler CanExecuteChanged;

        public AbstractCommandControl()
        {
            CanExecuteChanged += new EventHandler(CommandCanExecuteChanged);
        }

        public ICommand Command 
        {
            get
            {
                if (m_Command == null)
                {
                    m_Command = new BaseCommand();
                    (m_Command as BaseCommand).ExecuteCommand += new ExecuteHandler(Run);
                    (m_Command as BaseCommand).EnableCondition = EnableCondition;
                }
                return m_Command;
            }

            set
            {
                Console.WriteLine(value);
                if (value == null)
                {
                    throw new ArgumentNullException(m_CommandName);
                }
                m_Command = value;
                m_Command.CanExecuteChanged += CanExecuteChanged;

                NotifyPropertyChanged(m_CommandArgs);
            }
        }

        void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (sender is BaseCommand)
            {
                BaseCommand c = sender as BaseCommand;
                EnableCondition = c.EnableCondition;
            }

        }

        private ICommand m_Command = null;

        static readonly PropertyChangedEventArgs m_CommandArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractCommandControl>(o => o.Command);

        static readonly string m_CommandName =
            NotifyPropertyChangedHelper.GetPropertyName<AbstractCommandControl>(o => o.Command);

        /// <summary>
        /// This method is called when the command is executed.
        /// Override this in the derived class to actually do something.
        /// </summary>
        protected virtual void Run() { }

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
                    m_EnableCondition = new AlwaysTrueCondition();
                    return m_EnableCondition;
                }

                if (Command is ICustomCommand)
                {
                    return (Command as ICustomCommand).EnableCondition;
                }
                else
                {
                    return m_EnableCondition;
                }                
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(m_EnableConditionName);
                }
                if (m_EnableCondition != value)
                {
                    if (Command is ICustomCommand)
                    {
                        (Command as ICustomCommand).EnableCondition = value;
                    }
                    else
                    {
                        m_EnableCondition = value;
                    }
                    NotifyPropertyChanged(m_EnableConditionArgs);
                }
            }
        }
        private ICondition m_EnableCondition = null;

        static readonly PropertyChangedEventArgs m_EnableConditionArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractCommandControl>(o => o.EnableCondition);

        static readonly string m_EnableConditionName =
            NotifyPropertyChangedHelper.GetPropertyName<AbstractCommandControl>(o => o.EnableCondition);

        #endregion
    }
}
