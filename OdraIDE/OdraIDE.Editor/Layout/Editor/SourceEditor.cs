using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.ComponentModel;
using ICSharpCode.AvalonEdit;
using System.Windows.Media;
using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Windows.Forms;
using AvalonDock;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Document;
using IDocument = OdraIDE.Core.IDocument;

namespace OdraIDE.Editor
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Documents, typeof(IDocument))]
    [Export(CompositionPoints.Workbench.Documents.SourceEditor, typeof(SourceEditor))]
    [Document(Name = SourceEditor.DOCUMENT_NAME)] 
    public class SourceEditor : AbstractDocument, ISourceEditor, IPartImportsSatisfiedNotification
    {
        public const string DOCUMENT_NAME = "SourceEditor";

        public event EventHandler QuerySelectionChanged;

        public SourceEditor()
        {
            Name = DOCUMENT_NAME;
            Title = "Source Editor";
            m_textEditor = new TextEditor();
        }

        [Import(OdraIDE.Editor.Services.Markers.TextMarkerService, typeof(ITextMarkerService))]
        public ITextMarkerService MarkerService { get; set; }

        [Import(typeof(Highlightings))]
        private Highlightings highlightings { get; set; }

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private ILayoutManager layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Options.SourceEditorGeneralOptionsPad, typeof(SourceEditorGeneralOptionsPad), RequiredCreationPolicy=CreationPolicy.Shared)]
        private SourceEditorGeneralOptionsPad optionsPad { get; set; }

        [Import(OdraIDE.Core.Services.Logging.LoggingService, typeof(ILoggingService))]
        private ILoggingService loggingService { get; set; }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }
        
        [ImportMany(ExtensionPoints.SourceEditor.FileClosingCommands, typeof(IFileClosingCommand))]
        private IEnumerable<IFileClosingCommand> fileClosingCommands { get; set; }

        [ImportMany(ExtensionPoints.SourceEditor.CompletionData, typeof(ICompletionData))]
        private IEnumerable<ICompletionData> completionDatas { get; set; }

        #region StatusBar Counters

        [Import(CompositionPoints.Workbench.StatusBar.LineCounterHeading, typeof(LineCounterHeading))]
        private LineCounterHeading lineCounterHeading { get; set; }

        [Import(CompositionPoints.Workbench.StatusBar.LineCounterText, typeof(LineCounterText))]
        private LineCounterText lineCounterText { get; set; }

        [Import(CompositionPoints.Workbench.StatusBar.ColumnCounterHeading, typeof(ColumnCounterHeading))]
        private ColumnCounterHeading columnCounterHeading { get; set; }

        [Import(CompositionPoints.Workbench.StatusBar.ColumnCounterText, typeof(ColumnCounterText))]
        private ColumnCounterText columnCounterText { get; set; }

        #endregion

        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        [ImportMany(ExtensionPoints.SourceEditor.FoldingStrategy, typeof(AbstractFoldingStrategy))]
        private IEnumerable<AbstractFoldingStrategy> foldingStrategies { get; set; }

        public override void OnGotFocus(object sender, RoutedEventArgs e)
        {
            layoutManager.SetMainWindowTitle(File.FileName);
            applicationStatus.Ready();
            base.OnGotFocus(sender, e);
        }

        public override void OnClosing(object sender, CancelEventArgs e)
        {
            ICollection<IFileClosingCommand> commands = extensionService.Sort(fileClosingCommands);

            foreach (IFileClosingCommand command in commands)
            {
                if (!command.OnClosing(File))
                {
                    e.Cancel = true;
                    return;
                }
            }
            
        }

        bool isLoading;

        public override void Load(OpenedFile file)
        {
            isLoading = true;
            layoutManager.SetMainWindowTitle(file.FileName);
            Title = file.GetFileNameWithoutPath();
            File.IsDirtyChanged += new EventHandler(File_IsDirtyChanged);
            File.IsDirty = true;
            m_textEditor.SyntaxHighlighting =
                    HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(file.FileName));
            isLoading = false;
        }

        public override void Load(OpenedFile file, Stream stream)
        {
            isLoading = true;
            File.IsDirty = true;
            applicationStatus.Busy();
            layoutManager.SetMainWindowTitle(file.FileName);
            Title = file.GetFileNameWithoutPath();
            try
            {
                m_textEditor.SyntaxHighlighting =
                    HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(file.FileName));

                m_textEditor.Load(stream);
            }
            finally
            {
                File.IsDirty = false;
                isLoading = false;
                File.IsDirtyChanged += new EventHandler(File_IsDirtyChanged);
                applicationStatus.Ready();
            }
        }

        private void File_IsDirtyChanged(object sender, EventArgs e)
        {
            OpenedFile file = sender as OpenedFile;
            if (file.IsDirty)
            {
                Title = Title + "*";
            }
            else
            {
                Title = file.GetFileNameWithoutPath();
            }
        }

        private FoldingManager foldingManager;

        [Import(typeof(PainterFactory))]
        private PainterFactory PainterFactory { get; set; }

        public void OnImportsSatisfied()
        {
            PainterFactory.CreatePainterForDocument(this);

            // Install folding strategies
            if (foldingManager == null)
                foldingManager = FoldingManager.Install(m_textEditor.TextArea);
            foreach (AbstractFoldingStrategy item in foldingStrategies)
            {
                item.UpdateFoldings(foldingManager, m_textEditor.Document);
            }

            optionsPad.PropertyChanged += new PropertyChangedEventHandler(RefreshTextEditor);
            InitTextEditor();
        }

        private void RefreshTextEditor(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName.Equals("ShowLineNumbers")) 
            {
                m_textEditor.ShowLineNumbers = optionsPad.ShowLineNumbers;
            }
            else if (propertyName.Equals("WordWrap"))
            {
                m_textEditor.WordWrap = optionsPad.WordWrap;
            }
            else if (propertyName.Equals("FontFamily"))
            {
                m_textEditor.FontFamily = optionsPad.FontFamily;
            }
            else if (propertyName.Equals("FontSize"))
            {
                m_textEditor.FontSize = optionsPad.FontSize;
            }
        }

        #region TextEditor

        /// <summary>
        /// 
        /// </summary>
        public TextEditor TextEditor 
        {
            get
            {
                return m_textEditor;
            }
        }
        private TextEditor m_textEditor;

        private void InitTextEditor()
        {
            if (MarkerService != null && MarkerService is ITextMarkerService)
            {
                m_textEditor.TextArea.TextView.BackgroundRenderers.Add(MarkerService as TextMarkerService);
                m_textEditor.TextArea.TextView.LineTransformers.Add(MarkerService as TextMarkerService);
            }
           
            m_textEditor.FontFamily = optionsPad.FontFamily;
            m_textEditor.FontSize = optionsPad.FontSize;
            m_textEditor.ShowLineNumbers = optionsPad.ShowLineNumbers;
            m_textEditor.WordWrap = optionsPad.WordWrap;
            m_textEditor.TextChanged += new EventHandler(TextEditorTextChanged);
            m_textEditor.TextChanged += new EventHandler(UpdateFoldingsOnTextChanged);
            m_textEditor.TextArea.SelectionChanged += QuerySelectionChanged;
            m_textEditor.TextArea.SelectionChanged += new EventHandler(TextArea_SelectionChanged);
            m_textEditor.TextArea.TextEntered += new TextCompositionEventHandler(TextArea_TextEntered);
            m_textEditor.TextArea.TextEntering += new TextCompositionEventHandler(TextArea_TextEntering);
            m_textEditor.TextArea.Caret.PositionChanged += new EventHandler(CaretPositionChanged);

            m_textEditor.TextArea.Caret.PositionChanged += new EventHandler(CaretPositionChanged1);

            //Tooltips
            m_textEditor.MouseHover += MouseHover;
            m_textEditor.MouseHoverStopped += MouseHoverStopped;

            
        }

        #region ---<<< Tooltips >>>---
        
        System.Windows.Controls.ToolTip toolTip;

        private void MouseHoverStopped(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (toolTip != null)
            {
                toolTip.IsOpen = false;
                e.Handled = true;
            }
        }

        private void ToolTipClosed(object sender, RoutedEventArgs e)
        {
            toolTip = null;
        }

        private void MouseHover(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var pos = m_textEditor.GetPositionFromPoint(e.GetPosition(m_textEditor));
            if (pos.HasValue) 
            {
                var markersAtOffset = MarkerService.GetMarkersAtOffset(m_textEditor.Document, m_textEditor.Document.GetOffset(new TextLocation(pos.Value.Line, pos.Value.Column)));
                if (markersAtOffset == null) return;
                ITextMarker markerWithToolTip = markersAtOffset.FirstOrDefault(marker => marker.ToolTip != null);

                if (markerWithToolTip != null && markerWithToolTip.ToolTip != null)
                {
                    if (toolTip == null)
                    {
                        toolTip = new System.Windows.Controls.ToolTip();
                        toolTip.Closed += ToolTipClosed;
                    }
                    toolTip.PlacementTarget = m_textEditor; // required for property inheritance
                    toolTip.Content = markerWithToolTip.ToolTip;
                    toolTip.IsOpen = true;
                    e.Handled = true;
                }
            }
        }

        #endregion

        private void CaretPositionChanged1(object sender, EventArgs e)
        {
            Caret caret = sender as Caret;
            
        }

        private void UpdateFoldingsOnTextChanged(object sender, EventArgs e)
        {
            foreach (AbstractFoldingStrategy item in foldingStrategies)
            {
                item.UpdateFoldings(foldingManager, m_textEditor.Document);
            }
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private CompletionWindow completionWindow;

        private bool ValidPreviousChar()
        {
            int offset = m_textEditor.TextArea.Caret.Offset;

            if (offset <= 1) return true;
            if (offset >= 2)
            {
                char beforeChar = m_textEditor.Document.GetCharAt(offset - 2);
                if (beforeChar == '\n') return true;
                if (beforeChar != ' ') return false;
            }
            return true;
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (completionWindow == null)
            {
                bool showCompletion = false;
                if (ValidPreviousChar()) 
                {
                    foreach (ICompletionData data in completionDatas)
                    {
                        if ((data.Content as string).StartsWith(e.Text))
                        {
                            showCompletion = true;
                            break;
                        }
                    }
                }

                if (showCompletion)
                {
                    completionWindow = new CompletionWindow(m_textEditor.TextArea);
                    IList<ICompletionData> dataList = completionWindow.CompletionList.CompletionData;
                    foreach (ICompletionData data in completionDatas)
                    {
                        dataList.Add(data);
                    }
                    completionWindow.Show();
                    completionWindow.Closed += delegate
                    {
                        completionWindow = null;
                    };
                    //completionWindow.CompletionList.
                    completionWindow.StartOffset = completionWindow.StartOffset - 1;
                    completionWindow.CompletionList.IsFiltering = true;
                    completionWindow.CompletionList.SelectItem(e.Text);
                }
            }
            
        }

        private void TextArea_SelectionChanged(object sender, EventArgs e)
        {
            File.FireSelectionChanged(sender, e);
        }

        private void CaretPositionChanged(object sender, EventArgs e)
        {
            Caret caret = sender as Caret;
            lineCounterText.SetLineNumber(caret.Line);
            lineCounterHeading.Show();
            columnCounterHeading.Show();
            columnCounterText.SetColumnNumber(caret.Column);
        }


        private void TextEditorTextChanged(object sender, EventArgs e)
        {
            if (!File.IsDirty)
            {
                File.MakeDirty();
            }
        }

        #endregion

        public override void OnLostFocus(object sender, RoutedEventArgs e)
        {
            lineCounterHeading.Clear();
            lineCounterText.Clear();
            columnCounterHeading.Clear();
            columnCounterText.Clear();
        }

        public override void Save(OpenedFile file, Stream stream)
        {
            applicationStatus.Busy();
            m_textEditor.Save(stream);
            Title = file.GetFileNameWithoutPath();
            m_textEditor.SyntaxHighlighting =
                    HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(file.FileName));
            applicationStatus.SetStatus(Resources.Strings.Workbench_Status_Item_Saved, false);
        }

        public string SourceCode
        {
            get { return m_textEditor.Text; }
        }

        public string SelectedSourceCode
        {
            get { return m_textEditor.SelectedText; }
        }
    }
}
