using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using OdraIDE.Utilities;

namespace OdraIDE.Core.Workbench
{
    [Export(CompositionPoints.Workbench.ViewModel)]
    public class Workbench : AbstractViewModel, IPartImportsSatisfiedNotification
    {
        [Import(Services.Logging.LoggingService, typeof(ILoggingService))]
        private ILoggingService logger { get; set; }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [Import(Services.Layout.LayoutManager, typeof(ILayoutManager))]
        public ILayoutManager LayoutManager { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.Self, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        [ImportMany(ExtensionPoints.Workbench.ToolBars.Self, typeof(IToolBar), AllowRecomposition = true)]
        private IEnumerable<IToolBar> toolBars { get; set; }

        [ImportMany(ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem), AllowRecomposition = true)]
        private IEnumerable<IStatusBarItem> statusBar { get; set; }

        [ImportMany(ExtensionPoints.Workbench.Pads, typeof(IPad), AllowRecomposition = true)]
        private IEnumerable<Lazy<IPad, IPadMeta>> pads { get; set; }

        [ImportMany(ExtensionPoints.Workbench.Documents, typeof(IDocument), AllowRecomposition = true)]
        private IEnumerable<Lazy<IDocument, IDocumentMeta>> documents { get; set; }

        [ImportMany(ExtensionPoints.Workbench.ClosingCommands, typeof(IExecutableCommand))]
        private IEnumerable<IExecutableCommand> closingCommands { get; set; }

        public void OnImportsSatisfied()
        {
            // when this is called, all imports that could be satisfied have been satisfied.
            MainMenu = extensionService.Sort(menu);
            StatusBar = extensionService.Sort(statusBar);
            ToolBars = extensionService.Sort(toolBars);
            LayoutManager.SetAllPadsDocuments(pads, documents);
        }

        #region "MainMenu"

        public IEnumerable<IMenuItem> MainMenu
        {
            get
            {
                return m_MainMenu;
            }
            private set
            {
                if (m_MainMenu != value)
                {
                    m_MainMenu = value;
                    NotifyPropertyChanged(m_MainMenuArgs);
                }
            }
        }
        private IEnumerable<IMenuItem> m_MainMenu = null;
        static readonly PropertyChangedEventArgs m_MainMenuArgs =
            NotifyPropertyChangedHelper.CreateArgs<Workbench>(o => o.MainMenu);

        #endregion

        #region "StatusBar"

        public IEnumerable<IStatusBarItem> StatusBar
        {
            get
            {
                return m_StatusBar;
            }
            private set
            {
                if (m_StatusBar != value)
                {
                    m_StatusBar = value;
                    NotifyPropertyChanged(m_StatusBarArgs);
                }
            }
        }
        private IEnumerable<IStatusBarItem> m_StatusBar = null;
        static readonly PropertyChangedEventArgs m_StatusBarArgs =
            NotifyPropertyChangedHelper.CreateArgs<Workbench>(o => o.StatusBar);

        #endregion

        #region "ToolBars"

        public IEnumerable<IToolBar> ToolBars
        {
            get
            {
                return m_ToolBars;
            }
            set
            {
                if (m_ToolBars != value)
                {
                    m_ToolBars = value;
                    NotifyPropertyChanged(m_ToolBarsArgs);
                }
            }
        }
        private IEnumerable<IToolBar> m_ToolBars = null;
        static readonly PropertyChangedEventArgs m_ToolBarsArgs =
            NotifyPropertyChangedHelper.CreateArgs<Workbench>(o => o.ToolBars);

        #endregion

        public void OnClosing(object sender, CancelEventArgs e)
        {
            logger.Info("Workbench closing.");

            foreach (IExecutableCommand cmd in closingCommands)
            {
                cmd.Run(sender, e);
            }
        }

        public void OnClosed(object sender, EventArgs e)
        {
            logger.Info("Workbench closed.");
        }

    }
}
