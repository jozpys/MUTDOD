using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace OdraIDE.Core
{
    public abstract class AbstractViewModel : DispatcherObject, IViewModel
    {
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
