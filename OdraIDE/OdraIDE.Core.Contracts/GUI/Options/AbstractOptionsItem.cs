using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core
{
    public abstract class AbstractOptionsItem : AbstractControl, IOptionsItem
    {

        public AbstractOptionsItem()
            : base()
        {
        }

        #region " IOptionsItem Implementation "

        #region " Header "
        /// <summary>
        /// This is the text displayed in the options tree view for this item.
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
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (m_Header != value)
                {
                    m_Header = value;
                    NotifyPropertyChanged(m_HeaderArgs);
                }
            }
        }
        private string m_Header = string.Empty;
        static readonly PropertyChangedEventArgs m_HeaderArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractOptionsItem>(o => o.Header);
        #endregion

        #region " Items "
        /// <summary>
        /// If the item being defined has a subtree, then replace this
        /// with a collection of the option items in the subtree.
        /// </summary>
        public IEnumerable<IOptionsItem> Items
        {
            get
            {
                return m_Items;
            }
            protected set
            {
                if (m_Items != value)
                {
                    if (m_Items != null)
                    {
                        foreach (IOptionsItem item in m_Items)
                        {
                            item.OptionChanged -= Items_OptionChanged;
                        }
                    }
                    m_Items = value;
                    if (m_Items != null)
                    {
                        foreach (IOptionsItem item in m_Items)
                        {
                            item.OptionChanged += new EventHandler(Items_OptionChanged);
                        }
                    }
                    NotifyPropertyChanged(m_ItemsArgs);
                }
            }
        }
        private IEnumerable<IOptionsItem> m_Items = new Collection<IOptionsItem>();
        static readonly PropertyChangedEventArgs m_ItemsArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractOptionsItem>(o => o.Items);

        void Items_OptionChanged(object sender, EventArgs e)
        {
            NotifyOptionChanged();
        }

        #endregion

        #region " Pad "
        /// <summary>
        /// This is the pad displayed in the options dialog's
        /// content control.
        /// </summary>
        public IOptionsPad Pad
        {
            get
            {
                return m_Pad;
            }
            protected set
            {
                if (m_Pad != value)
                {
                    if (m_Pad != null)
                    {
                        m_Pad.OptionChanged -= m_Pad_OptionChanged;
                    }
                    m_Pad = value;
                    if (m_Pad != null)
                    {
                        m_Pad.OptionChanged += new EventHandler(m_Pad_OptionChanged);
                    }
                    NotifyPropertyChanged(m_PadArgs);
                }
            }
        }
        private IOptionsPad m_Pad = null;
        static readonly PropertyChangedEventArgs m_PadArgs =
            NotifyPropertyChangedHelper.CreateArgs<AbstractOptionsItem>(o => o.Pad);

        void m_Pad_OptionChanged(object sender, EventArgs e)
        {
            NotifyOptionChanged();
        }

        #endregion

        public event EventHandler OptionChanged;

        protected void NotifyOptionChanged()
        {
            var evt = OptionChanged;
            if (evt != null)
            {
                evt(this, new EventArgs());
            }
        }

        #endregion

        /// <summary>
        /// If overriding this, you should still call base.CommitChanges()
        /// </summary>
        public virtual void CommitChanges()
        {
            if (Pad != null)
            {
                Pad.Commit();
            }
            foreach (IOptionsItem item in Items)
            {
                item.CommitChanges();
            }
        }


        /// <summary>
        /// If overriding this, you should still call base.CancelChanges()
        /// </summary>
        public virtual void CancelChanges()
        {
            if (Pad != null)
            {
                Pad.Cancel();
            }
            foreach (IOptionsItem item in Items)
            {
                item.CancelChanges();
            }
        }

    }
}
