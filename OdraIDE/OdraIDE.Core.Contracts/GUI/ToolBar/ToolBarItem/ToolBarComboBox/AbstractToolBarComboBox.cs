using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core
{
    public abstract class AbstractToolBarComboBox : AbstractControl, IToolBarItem
    {
        #region Items

        private IList m_Items;

        public IList Items
        {
            get
            {
                return m_Items;
            }

            set
            {
                m_Items = value;
                NotifyPropertyChanged(m_ItemsArgs);
            }
        }

        static readonly PropertyChangedEventArgs m_ItemsArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractToolBarComboBox>(o => o.Items);

        #endregion

        #region Selected Item

        

        public object SelectedItem
        {
            get
            {
                return m_selectedItem;
            }

            set
            {
                if (m_selectedItem != value)
                {
                    m_selectedItem = value;
                    NotifyPropertyChanged(m_SelectedItemArgs);
                }
            }
        }

        private object m_selectedItem;
        static readonly PropertyChangedEventArgs m_SelectedItemArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractToolBarComboBox>(o => o.SelectedItem);

        public int SelectedIndex
        {
            get
            {
                return m_selectedIndex;
            }

            set
            {
                if (m_selectedIndex != value)
                {
                    m_selectedIndex = value;
                    NotifyPropertyChanged(m_SelectedIndexArgs);
                }
            }
        }

        private int m_selectedIndex;
        static readonly PropertyChangedEventArgs m_SelectedIndexArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractToolBarComboBox>(o => o.SelectedIndex);

        #endregion
    }
         
}
