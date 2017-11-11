using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core
{
    public abstract class AbstractControl : AbstractExtension, IControl
    {
        public AbstractControl()
        {
        }

        #region " ToolTip "
        /// <summary>
        /// This is the tool tip displayed when the mouse hovers over the control.
        /// Best practice is to set this in the constructor of the derived
        /// class.
        /// </summary>
        public string ToolTip
        {
            get
            {
                return m_ToolTip;
            }

            protected set
            {
                string formattedValue = string.Empty;
                if (value != null)
                {
                    formattedValue = value.Replace("\\n", Environment.NewLine);
                }
                if (m_ToolTip != formattedValue)
                {
                    if (formattedValue == string.Empty)
                    {
                        m_ToolTip = null;
                    }
                    else
                    {
                        m_ToolTip = formattedValue;
                    }
                    NotifyPropertyChanged(m_ToolTipArgs);
                }
            }
        }
        private string m_ToolTip = null;
        static readonly PropertyChangedEventArgs m_ToolTipArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractControl>(o => o.ToolTip);
        #endregion

        #region " Visible "
        /// <summary>
        /// Defaults to true. Set to false to make the control disappear.
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            private set
            {
                if (m_Visible != value)
                {
                    m_Visible = value;
                    NotifyPropertyChanged(m_VisibleArgs);
                }
            }
        }
        private bool m_Visible = true;
        static readonly PropertyChangedEventArgs m_VisibleArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractControl>(o => o.Visible);
        #endregion

        #region " VisibleCondition "
        /// <summary>
        /// Set this to any ICondition object, and it will control
        /// the Visible property.
        /// </summary>
        public ICondition VisibleCondition
        {
            get
            {
                return m_VisibleCondition;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(m_VisibleConditionName);
                }
                if (m_VisibleCondition != value)
                {
                    if (m_VisibleCondition != null)
                    {
                        //remove the old event handler
                        m_VisibleCondition.ConditionChanged -= OnVisibleConditionChanged;
                    }
                    m_VisibleCondition = value;
                    //add the new event handler
                    m_VisibleCondition.ConditionChanged += OnVisibleConditionChanged;
                    Visible = m_VisibleCondition.Condition;

                    NotifyPropertyChanged(m_VisibleConditionArgs);
                }
            }
        }
        private ICondition m_VisibleCondition = null;
        static readonly PropertyChangedEventArgs m_VisibleConditionArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractControl>(o => o.VisibleCondition);
        static readonly string m_VisibleConditionName =
            NotifyPropertyChangedHelper.GetPropertyName<AbstractControl>(o => o.VisibleCondition);

        private void OnVisibleConditionChanged(object sender, EventArgs e)
        {
            Visible = m_VisibleCondition.Condition;
        }
        #endregion

    }
}
