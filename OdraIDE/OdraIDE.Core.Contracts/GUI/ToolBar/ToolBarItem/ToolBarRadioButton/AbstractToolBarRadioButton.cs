using System;
using System.Collections.Generic;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core
{
    public abstract class AbstractToolBarRadioButton : AbstractRadioButton, IToolBarItem
    {

        public AbstractToolBarRadioButton()
        {
            this.PropertyChanged += OnPropertyChanged;
        }

        #region " ToolBarItems "
        /// <summary>
        /// NOT UNIT TESTED
        /// 
        /// This set-only property and the OnPropertyChanged event handler
        /// below only exist to help fix a bug in WPF.  The problem is that
        /// if two or more radio buttons from the same group are split
        /// in a toolbar with some in the toolbar and some in the overflow
        /// box, then the user could select more than one item.
        /// The derived class should set this property to the list of 
        /// all toolbar items in the same toolbar.  It then filters out
        /// any peer radio buttons in the same group, and makes sure they
        /// are always set to IsChecked = false when this button is checked.
        /// More Info: http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=322929
        /// </summary>
        protected IEnumerable<IToolBarItem> ToolBarItems
        {
            set
            {
                if (value == null)
                {
                    m_peerRadioButtons = null;
                }
                else
                {
                    List<AbstractToolBarRadioButton> radioButtons =
                        new List<AbstractToolBarRadioButton>();
                    foreach (IToolBarItem tb in value)
                    {
                        AbstractToolBarRadioButton rb = tb as AbstractToolBarRadioButton;
                        if (rb != null)
                        {
                            if (rb != this && rb.GroupName == GroupName)
                            {
                                radioButtons.Add(rb);
                            }
                        }
                    }
                    m_peerRadioButtons = radioButtons;
                    CheckOtherRadioButtons();
                }
            }
        }

        private List<AbstractToolBarRadioButton> m_peerRadioButtons = null;

        private void CheckOtherRadioButtons()
        {
            if (IsChecked)
            {
                if (m_peerRadioButtons != null)
                {
                    foreach (AbstractToolBarRadioButton rb in m_peerRadioButtons)
                    {
                        rb.IsChecked = false;
                    }
                }
            }
        }

        #endregion

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == m_IsCheckedName)
            {
                CheckOtherRadioButtons();
            }
        }
        static readonly string m_IsCheckedName =
            NotifyPropertyChangedHelper.GetPropertyName<AbstractRadioButton>(o => o.IsChecked);
    }
}
