using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using OdraIDE.Utilities;
using System.IO;

namespace OdraIDE.Core
{
    public abstract class AbstractDocument : AbstractLayoutItem, IDocument
    {
        /// <summary>
        /// Override this in the derived class to take an action
        /// when the document is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnOpened(object sender, EventArgs e) { }

        /// <summary>
        /// Override this in the derived class to take an action
        /// before the document is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClosing(object sender, CancelEventArgs e) { }

        /// <summary>
        /// Override this in the derived class to take an action
        /// after the document is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClosed(object sender, EventArgs e) 
        {
            File.RaiseFileClosed();
        }

        public virtual void OnGotFocus(object sender, RoutedEventArgs e)
        {
            base.OnGotFocus(sender, e);
        }
        public virtual void OnLostFocus(object sender, RoutedEventArgs e)
        {
            base.OnLostFocus(sender, e);
        }

        #region IDocument Members

        public OpenedFile File
        {
            get
            {
                return m_file;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("File");
                }
                m_file = value;
                File.ForceInitializeDocument(this);
            }
        }

        private OpenedFile m_file;

        public virtual void Save(OpenedFile file, Stream stream)
        {
        }

        public virtual void Load(OpenedFile file)
        {
        }

        public virtual void Load(OpenedFile file, Stream stream)
        {
        }

        #endregion
    }
}
