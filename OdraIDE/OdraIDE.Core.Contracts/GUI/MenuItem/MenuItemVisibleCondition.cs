using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Utilities;

namespace OdraIDE.Core
{
    /// <summary>
    /// Controls the visibility of a MenuItem based on whether or
    /// not there are any items in its submenu.
    /// </summary>
    public sealed class MenuItemVisibleCondition : AbstractCondition
    {
        public MenuItemVisibleCondition(IMenuItem menuItem)
        {
            m_menuItem = menuItem;
            SetCondition();

            // Register a callback for when the menuItem Items 
            // property changes
            menuItem.PropertyChanged +=
                new System.ComponentModel.PropertyChangedEventHandler(
                    OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == m_ItemsName)
            {
                SetCondition();
            }
        }
        static readonly string m_ItemsName =
            NotifyPropertyChangedHelper.GetPropertyName<IMenuItem>(o => o.Items);

        IMenuItem m_menuItem = null;

        private void SetCondition()
        {
            bool visible = false;
            if (m_menuItem != null)
            {
                if (m_menuItem.Items == null)
                {
                    // not supposed to have a submenu
                    visible = true;
                }
                else
                {
                    // it's supposed to have a submenu, see if any exist
                    foreach (IMenuItem item in m_menuItem.Items)
                    {
                        visible = true;
                        break;
                    }
                }
            }
            Condition = visible;
        }
    }
}
