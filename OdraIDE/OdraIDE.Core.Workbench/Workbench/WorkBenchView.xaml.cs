using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel.Composition;

namespace OdraIDE.Core.Workbench
{
    /// <summary>
    /// Interaction logic for WorkBenchView.xaml
    /// </summary>
    [Export(CompositionPoints.Host.MainWindow, typeof(Window))]
    public partial class WorkBenchView : Window, IPartImportsSatisfiedNotification
    {
        [ImportMany(typeof(KeyBinding))]
        private IEnumerable<KeyBinding> keyBindings { get; set; }

        [ImportingConstructor]
        public WorkBenchView([Import(CompositionPoints.Workbench.ViewModel)] Workbench vm)
        {
            InitializeComponent();
            
            DataContext = vm;

            // ToolBarTray.ToolBars isn't a dependency property, so we
            // have to add the Tool Bars manually
            foreach (IToolBar toolBarViewModel in vm.ToolBars)
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

                tbtToolBar.ToolBars.Add(toolBar);
            }

            // hook up the event handlers so the viewmodel knows when we're closing
            this.Closing += vm.OnClosing;
            this.Closed += vm.OnClosed;
        }

        public void OnImportsSatisfied()
        {
            foreach (KeyBinding item in keyBindings)
            {
                this.InputBindings.Add(item);
            }
        }
    }

}
