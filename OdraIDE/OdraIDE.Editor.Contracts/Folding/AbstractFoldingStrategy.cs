using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Editor
{
    public abstract class AbstractFoldingStrategy : ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy, IExtension, IViewModel
    {
        private string m_ID = Guid.NewGuid().ToString();
        static readonly PropertyChangedEventArgs m_IDArgs = NotifyPropertyChangedHelper.CreateArgs<AbstractExtension>(o => o.ID);
        static readonly string m_IDName = NotifyPropertyChangedHelper.GetPropertyName<AbstractExtension>(o => o.ID);

        /// <summary>
        /// This is a unique string used to identify the extension.
        /// The point of this is so that other extensions can insert themselves
        /// before or after this item in the list of extensions.  Example: "File"
        /// Should *always* be set in the derived class's constructor.
        /// </summary>
        public string ID
        {
            get
            {
                return m_ID;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(m_IDName);
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException(m_IDName);
                }
                if (m_ID != value)
                {
                    m_ID = value;
                    NotifyPropertyChanged(m_IDArgs);
                }
            }
        }



        private string m_InsertRelativeToID = null;
        static readonly PropertyChangedEventArgs m_InsertRelativeToIDArgs = NotifyPropertyChangedHelper.CreateArgs<AbstractExtension>(o => o.InsertRelativeToID);

        /// <summary>
        /// If specified, the extension list will try to insert this extension 
        /// before or after the extension with this ID.
        /// </summary>
        public string InsertRelativeToID
        {
            get
            {
                return m_InsertRelativeToID;
            }
            protected set
            {
                if (m_InsertRelativeToID != value)
                {
                    m_InsertRelativeToID = value;
                    NotifyPropertyChanged(m_InsertRelativeToIDArgs);
                }
            }
        }

        private RelativeDirection m_BeforeOrAfter = RelativeDirection.Before;
        static readonly PropertyChangedEventArgs m_BeforeOrAfterArgs = NotifyPropertyChangedHelper.CreateArgs<AbstractExtension>(o => o.BeforeOrAfter);

        /// <summary>
        /// If specified, the extension list will try to insert this extension 
        /// before or after the extension with this ID.
        /// </summary>
        public RelativeDirection BeforeOrAfter
        {
            get
            {
                return m_BeforeOrAfter;
            }
            protected set
            {
                if (m_BeforeOrAfter != value)
                {
                    m_BeforeOrAfter = value;
                    NotifyPropertyChanged(m_BeforeOrAfterArgs);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Call this method to raise the PropertyChanged event when
        /// a property changes.  Note that you should use the
        /// NotifyPropertyChangedHelper class to create a cached
        /// copy of the PropertyChangedEventArgs object to pass
        /// into this method.  Usage:
        /// 
        /// static readonly PropertyChangedEventArgs m_$PropertyName$Args = 
        ///     NotifyPropertyChangedHelper.CreateArgs<$ClassName$>(o => o.$PropertyName$);
        /// 
        /// In your property setter:
        ///     PropertyChanged(this, m_$PropertyName$Args)
        /// 
        /// </summary>
        /// <param name="e">A cached event args object</param>
        protected void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            var evt = PropertyChanged;
            if (evt != null)
            {
                evt(this, e);
            }
        }
    }
}
