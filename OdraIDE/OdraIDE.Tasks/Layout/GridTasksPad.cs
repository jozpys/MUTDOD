using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using OdraIDE.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Media;
using OdraIDE.Editor;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Document;
using System.Threading;

namespace OdraIDE.Tasks
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(OdraIDE.Tasks.CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
    [Pad(Name = GridTasksPad.TP_NAME)]
    public class GridTasksPad : AbstractPad, IPartImportsSatisfiedNotification
    {
        public const string TP_NAME = "GridTasksPad";

        [Import(OdraIDE.Editor.Services.Markers.TextMarkerService, typeof(ITextMarkerService))]
        public ITextMarkerService MarkerService { get; set; }

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        private ITaskService TaskService { get; set; }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService ExtensionService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private IFileService FileService { get; set; }

        [ImportMany(ExtensionPoints.ToolBars.Self, typeof(IToolBar), AllowRecomposition = true)]
        private IEnumerable<IToolBar> toolBars { get; set; }

        private bool ErrorTaskVisible = true;
        private bool WarningTaskVisible = true;
        private bool MessageTaskVisible = true;

        public GridTasksPad()
        {
            Name = TP_NAME;
            Title = "Tasks";
            Location = PadLocation.Bottom;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.Tasks);

            m_ListView.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(OnDoubleClick));
        }

        private void OnDoubleClick(object sender, RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            object[] item = (object[])m_ListView.ItemContainerGenerator.ItemFromContainer(dep);

            FileName fileName = (FileName)item[3];
            int line = (int)item[4];
            int column = (int)item[5];

            OpenedFile file = FileService.GetOpenedFile(fileName);

            if (file != null)
            {
                if (file.Document is ISourceEditor)
                {
                    ISourceEditor sourceEditor = file.Document as ISourceEditor;

                    var markersAtOffset = MarkerService.GetMarkersAtOffset(sourceEditor.TextEditor.Document, sourceEditor.TextEditor.Document.GetOffset(new TextLocation(line, column)));
                    if (markersAtOffset == null) return;
                    ITextMarker marker = markersAtOffset.FirstOrDefault();
                    if (marker == null) return;
                    TextArea area = sourceEditor.TextEditor.TextArea;

                    layoutManager.Value.ShowDocument(sourceEditor, true);
                    area.Selection = Selection.Create(area, marker.Offset, marker.EndOffset);
                    area.Caret.Line = line;
                    area.Caret.Column = column;
                    sourceEditor.TextEditor.ScrollTo(line, column);
                    sourceEditor.TextEditor.Focus();
                }
            }
        }

        public void FilterTaskList(TaskType type, bool visible)
        {
            switch (type)
            {
                case TaskType.Error:
                    ErrorTaskVisible = visible;
                    break;
                case TaskType.Warning:
                    WarningTaskVisible = visible;
                    break;
                case TaskType.Message:
                    MessageTaskVisible = visible;
                    break;
            }
            RefreshView();
        }

        private DynamicListView m_ListView = new DynamicListView();

        public DynamicListView ListView
        {
            get
            {
                return m_ListView;
            }
        }

        public void ShowTasks(DataMatrix dm)
        {
            m_ListView.DataMatrix = dm;
            layoutManager.Value.ShowPad(this);
        }

        public void Clear()
        {
            m_ListView.DataMatrix = new DataMatrix(); ;
        }

        public void OnImportsSatisfied()
        {
            TaskService.Added += new TaskEventHandler(TaskService_Added);
            TaskService.Removed += new TaskEventHandler(TaskService_Removed);
            TaskService.Cleared += new EventHandler(TaskService_Cleared);

            InitToolbars();
        }

        void TaskService_Added(object sender, TaskEventArgs e)
        {
            RefreshView();
        }

        void TaskService_Removed(object sender, TaskEventArgs e)
        {
            RefreshView();
        }

        void TaskService_Cleared(object sender, EventArgs e)
        {
            Clear();
        }

        private void RefreshView()
        {
            Clear();
            ShowTasks(CreateDataMatrix(TaskService.Tasks));
        }

        private DataTemplate CreateCellTemplate()
        {
            DataTemplate dt = new DataTemplate();
            FrameworkElementFactory fef = new FrameworkElementFactory(typeof(Image));
            fef.SetBinding(Image.SourceProperty, new Binding("[0]"));
            dt.VisualTree = fef;

            return dt;
        }

        private DataMatrix CreateDataMatrix(IEnumerable<Task> tasks)
        {
            DataMatrix dm = new DataMatrix();
            dm.Columns.Add(new MatrixColumn() { Name = " ", Width = 30, CellTemplate = CreateCellTemplate() });
            dm.Columns.Add(new MatrixColumn() { Name = " ", Width = 20 });
            dm.Columns.Add(new MatrixColumn() { Name = "Description", Width = 300 });
            dm.Columns.Add(new MatrixColumn() { Name = "File", Width = 100 });
            dm.Columns.Add(new MatrixColumn() { Name = "Line", Width = 30 });
            dm.Columns.Add(new MatrixColumn() { Name = "Column", Width = 30 });

            int i = 1;
            foreach (Task task in tasks)
            {
                switch (task.TaskType)
                {
                    case TaskType.Error:
                        if (ErrorTaskVisible)
                            dm.Rows.Add(CreateRow(task, i++)); break;
                    case TaskType.Warning:
                        if (WarningTaskVisible)
                            dm.Rows.Add(CreateRow(task, i++)); break;
                    case TaskType.Message:
                        if (MessageTaskVisible)
                            dm.Rows.Add(CreateRow(task, i++)); break;
                }
            }

            return dm;
        }

        private object[] CreateRow(Task task, int i)
        {
            object[] row = new object[6];
            switch (task.TaskType)
            {
                case TaskType.Error:
                    row[0] = "/OdraIDE.Tasks;component/Resources/Error.png";
                    break;
                case TaskType.Warning:
                    row[0] = "/OdraIDE.Tasks;component/Resources/Warning.png";
                    break;
                case TaskType.Message:
                    row[0] = "/OdraIDE.Tasks;component/Resources/Info.png";
                    break;
                default:
                    row[0] = "/OdraIDE.Tasks;component/Resources/Empty.png";
                    break;
            }

            row[1] = i;
            row[2] = task.Description;
            row[3] = task.FileName;
            row[4] = task.Line;
            row[5] = task.Column;

            return row;
        }

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
            NotifyPropertyChangedHelper.CreateArgs<GridTasksPad>(o => o.ToolBars);

        private void InitToolbars()
        {
            ToolBars = ExtensionService.Sort(toolBars);

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
    }
}
