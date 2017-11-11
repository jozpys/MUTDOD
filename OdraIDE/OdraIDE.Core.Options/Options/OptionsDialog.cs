using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.Composition;

namespace OdraIDE.Core.Options
{
    /// <summary>
    /// ViewModel for the Options Dialog
    /// Inheriting from AbstractOptionsItem is just a hack
    /// because it makes it easy to set the Items property.
    /// </summary>
    [Export(CompositionPoints.Options.OptionsDialog, typeof(OptionsDialog))]
    class OptionsDialog : AbstractOptionsItem, IPartImportsSatisfiedNotification
    {
        public OptionsDialog()
        {
            OptionChanged += new EventHandler(OptionsDialog_OptionChanged);
        }

        void OptionsDialog_OptionChanged(object sender, EventArgs e)
        {
            m_dirtyCondition.SetCondition(true);
        }

        [Import(Services.Logging.LoggingService, typeof(ILoggingService))]
        private ILoggingService logger { get; set; }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Options.OptionsDialog.OptionsItems, typeof(IOptionsItem), AllowRecomposition = true)]
        private IEnumerable<IOptionsItem> items { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(items);
        }

        [Import(CompositionPoints.Host.MainWindow)]
        private Lazy<Window> mainWindowExport { get; set; }

        /// <summary>
        /// Displays the Options Dialog as modal
        /// </summary>
        public void ShowDialog()
        {
            Window mainWindow = mainWindowExport.Value;
            Window optionsDialog = new OptionsDialogView();
            optionsDialog.Owner = mainWindow;
            optionsDialog.DataContext = this;
            logger.Info("Showing options dialog...");
            m_dirtyCondition.SetCondition(false);
            optionsDialog.ShowDialog();
            ReloadSavedValues();
            logger.Info("Options dialog closed.");
        }

        private ICondition dirtyCondition
        {
            get
            {
                return m_dirtyCondition;
            }
        }
        private ConcreteCondition m_dirtyCondition = new ConcreteCondition(false);

        #region " OK Button "
        public IControl OKButton
        {
            get
            {
                if (m_OKButton == null)
                {
                    m_OKButton = new CommitChangesButton(this);
                }
                return m_OKButton;
            }
        }
        private IControl m_OKButton = null;

        private class CommitChangesButton : AbstractButton
        {
            public CommitChangesButton(OptionsDialog dlg)
            {
                m_OptionsDialog = dlg;
                EnableCondition = dlg.dirtyCondition;
            }

            private OptionsDialog m_OptionsDialog = null;

            protected override void Run()
            {
                foreach (IOptionsItem item in m_OptionsDialog.Items)
                {
                    item.CommitChanges();
                }
            }
        }
        #endregion

        #region " Cancel Button "
        public IControl CancelButton
        {
            get
            {
                if (m_CancelButton == null)
                {
                    m_CancelButton = new CancelChangesButton(this);
                }
                return m_CancelButton;
            }
        }
        private IControl m_CancelButton = null;

        private class CancelChangesButton : AbstractButton
        {
            public CancelChangesButton(OptionsDialog dlg)
            {
                m_OptionsDialog = dlg;
            }

            private OptionsDialog m_OptionsDialog = null;

            protected override void Run()
            {
                foreach (IOptionsItem item in m_OptionsDialog.Items)
                {
                    item.CancelChanges();
                }
            }
        }
        #endregion

        private void ReloadSavedValues()
        {
            foreach (IOptionsItem item in Items)
            {
                item.CancelChanges();
            }
        }

    }
}
