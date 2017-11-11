using System;
using System.Collections.Generic;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core
{
    public abstract class AbstractToolBar : AbstractControl, IToolBar
    {
        #region " Header "
        /// <summary>
        /// This is the name of the toolbar. It's displayed in a label
        /// as the first item in the toolbar.
        /// Best to set this property in the derived class's constructor.
        /// </summary>
        public string Header
        {
            get
            {
                return m_Header;
            }
            protected set
            {
                if (m_Header != value)
                {
                    m_Header = value;
                    NotifyPropertyChanged(m_HeaderArgs);
                }
            }
        }
        private string m_Header = null;
        static readonly PropertyChangedEventArgs m_HeaderArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractToolBar>(o => o.Header);
        #endregion

        #region " Name "
        /// <summary>
        /// This is the name of the toolbar. It's displayed in a drop
        /// down list when the user selects/deselects visible toolbars.
        /// Best to set this property in the derived class's constructor.
        /// </summary>
        public string Name
        {
            get
            {
                if (m_Name == null)
                {
                    return ID;
                }
                else
                {
                    return m_Name;
                }
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(m_NameName);
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException(m_NameName);
                }
                if (m_Name != value)
                {
                    m_Name = value;
                    NotifyPropertyChanged(m_NameArgs);
                }
            }
        }
        private string m_Name = null;
        static readonly PropertyChangedEventArgs m_NameArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractToolBar>(o => o.Name);
        static readonly string m_NameName =
            NotifyPropertyChangedHelper.GetPropertyName<AbstractToolBar>(o => o.Name);
        #endregion

        #region " Items "
        /// <summary>
        /// Replace this
        /// with a collection of the tool bar items in the toolbar.
        /// </summary>
        public IEnumerable<IToolBarItem> Items
        {
            get
            {
                return m_Items;
            }
            protected set
            {
                if (m_Items != value)
                {
                    m_Items = value;
                    NotifyPropertyChanged(m_ItemsArgs);
                }
            }
        }
        private IEnumerable<IToolBarItem> m_Items = null;
        static readonly PropertyChangedEventArgs m_ItemsArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractToolBar>(o => o.Items);
        #endregion

    }
}
