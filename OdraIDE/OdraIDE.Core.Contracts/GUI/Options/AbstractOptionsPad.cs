using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public abstract class AbstractOptionsPad : AbstractPad, IOptionsPad
    {
        #region " Commit "

        /// <summary>
        /// If overriding this method, make sure to call base.Commit first.
        /// </summary>
        public virtual void Commit()
        {
            foreach (var commitAction in CommitActions)
            {
                commitAction();
            }
            CommitActions.Clear();
            CancelActions.Clear();
        }

        protected IList<Action> CommitActions
        {
            get
            {
                return m_commitActions;
            }
        }
        private readonly IList<Action> m_commitActions = new List<Action>();

        #endregion

        #region " Cancel "

        /// <summary>
        /// If overriding this method, make sure to call base.Cancel first.
        /// </summary>
        public virtual void Cancel()
        {
            foreach (var cancelAction in CancelActions)
            {
                cancelAction();
            }
            CancelActions.Clear();
            CommitActions.Clear();
        }

        protected IList<Action> CancelActions
        {
            get
            {
                return m_cancelActions;
            }
        }
        private readonly IList<Action> m_cancelActions = new List<Action>();

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
    }
}
