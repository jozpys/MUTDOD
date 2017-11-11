using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using OdraIDE.Utilities;
using AvalonDock;
using ICSharpCode.TreeView;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace OdraIDE.SolutionExplorer
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(CompositionPoints.Workbench.Pads.SolutionExplorer, typeof(SolutionExplorer))]
    [Export(CompositionPoints.Workbench.Pads.SolutionExplorer, typeof(ISolutionExplorer))]
    [Pad(Name = SolutionExplorer.SE_NAME)]
    public class SolutionExplorer : AbstractPad, ISolutionExplorer, IPartImportsSatisfiedNotification
    {
        public const string SE_NAME = "SolutionExplorer";

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.ToolBars.Self, typeof(IToolBar), AllowRecomposition = true)]
        private IEnumerable<IToolBar> toolBars { get; set; }

        [Import(CompositionPoints.Workbench.Pads.PropertyGrid, typeof(PropertyGrid))]
        private PropertyGrid propertyGrid { get; set; }

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        public SolutionExplorer()
        {
            Name = SE_NAME;
            Title = "Solution Explorer";
            Location = PadLocation.TopLeft;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.SolutionExplorer);
        }

        #region Tree View

        public SharpTreeView TreeView
        {
            get
            {
                return m_TreeView;
            }
        }

        private SharpTreeView m_TreeView = new SharpTreeView();

        #endregion


        #region "ToolBars"

        public ToolBarTray ToolBarTray
        {
            get
            {
                return m_ToolBarTray;
            }
        }

        private ToolBarTray m_ToolBarTray = new ToolBarTray();

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
            NotifyPropertyChangedHelper.CreateArgs<SolutionExplorer>(o => o.ToolBars);

        public void OnImportsSatisfied()
        {
            ToolBars = extensionService.Sort(toolBars);

            foreach (IToolBar toolBarViewModel in ToolBars)
            {
                ToolBar toolBar = new ToolBar();
                toolBar.DataContext = toolBarViewModel;

                // Bind the Header Property
                Binding headerBinding = new Binding("Header");
                toolBar.SetBinding(ToolBar.HeaderProperty, headerBinding);

                // Bind the Items Property
                Binding itemsBinding = new Binding("Items");
                toolBar.SetBinding(ToolBar.ItemsSourceProperty, itemsBinding);

                // Bind the Visible Property
                Binding visibleBinding = new Binding("Visible");
                visibleBinding.Converter = new BooleanToVisibilityConverter();
                toolBar.SetBinding(ToolBar.VisibilityProperty, visibleBinding);

                // Bind the ToolTip Property
                Binding toolTipBinding = new Binding("ToolTip");
                toolBar.SetBinding(ToolBar.ToolTipProperty, toolTipBinding);

                ToolBarTray.ToolBars.Add(toolBar);
            }
        }

        #endregion


        public void ShowPropertiesFor(object value)
        {
            propertyGrid.SetSelectedObject(value);
            layoutManager.Value.ShowPad(propertyGrid);
        }
    }
}
