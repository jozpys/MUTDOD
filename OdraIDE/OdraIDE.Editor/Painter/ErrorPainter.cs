using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using OdraIDE.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using System.ComponentModel;
using System.Windows.Threading;
using IDocument = OdraIDE.Core.IDocument;

namespace OdraIDE.Editor
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(CompositionPoints.Workbench.Painters.ErrorPainter, typeof(ErrorPainter))]
    public class ErrorPainter : IPainter, IPartImportsSatisfiedNotification
    {
        private IDocument m_Document;

        [Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
        public ITaskService TaskService { get; set; }

        [Import(OdraIDE.Editor.Services.Markers.TextMarkerService, typeof(ITextMarkerService))]
        public ITextMarkerService MarkerService { get; set;  }

        public ErrorPainter()
        {
        }

        public IDocument Document
        {
            get
            {
                return m_Document;
            }
            set
            {
                if (value is ISourceEditor)
                {
                    this.m_Document = value;
                }
                else
                {
                    throw new ArgumentException("Wrong type of value");                    
                }
            }
        }

        public void OnImportsSatisfied()
        {
            TaskService.Added += OnAdded;
            TaskService.Removed += OnRemoved;
            TaskService.Cleared += OnCleared;
        }

        private void OnAdded(object sender, TaskEventArgs e)
        {
            AddTask(e.Task);
        }

        private void OnRemoved(object sender, TaskEventArgs e)
        {
            MarkerService.RemoveAll(marker => marker.Tag == e.Task);
        }

        private void OnCleared(object sender, EventArgs e)
        {
            ClearErrors();
        }

        /// <summary>
        /// Clears all TextMarkers representing errors.
        /// </summary>
        void ClearErrors()
        {
            MarkerService.RemoveAll(marker => marker.Tag is Task);
        }

        private bool CheckTask(Task task)
        {
            if (Document.File == null || Document.File.FileName == null)
                return false;
            if (task.FileName == null || task.Column <= 0)
                return false;
            if (task.TaskType != TaskType.Warning && task.TaskType != TaskType.Error && task.TaskType != TaskType.Message)
                return false;
            return task.FileName.Equals(Document.File.FileName);
        }

        private void AddTask(Task task)
        {
            if (!CheckTask(task))
                return;

            ISourceEditor editor = Document as ISourceEditor;

            if (task.Line >= 1 && task.Line <= editor.TextEditor.LineCount)
            {
                int offset = editor.TextEditor.Document.GetOffset(new TextLocation(task.Line, task.Column));
                int length = task.Length == 0 ? EditorHelper.GetWordAt(editor.TextEditor.Document, offset).Length : task.Length ;

                if (length < 2)
                {
                    // marker should be at least 2 characters long, but take care that we don't make
                    // it longer than the document
                    length = Math.Min(2, editor.TextEditor.Document.TextLength - offset);
                }

                ITextMarker marker = MarkerService.Create(editor, offset, length);

                marker.PropertyChanged += (sender, args) =>
                {
                    ITextMarker m = sender as ITextMarker;
                    editor.TextEditor.TextArea.TextView.Redraw(m, DispatcherPriority.Normal);
                };

                Color markerColor = Colors.Transparent;

                switch (task.TaskType)
                {
                    case TaskType.Error:
                        markerColor = Colors.Red;
                        break;
                    case TaskType.Message:
                        markerColor = Colors.Blue;
                        break;
                    case TaskType.Warning:
                        markerColor = Colors.Orange;
                        break;
                }

                marker.MarkerColor = markerColor;
                marker.MarkerType = task.Underline ? TextMarkerType.SquigglyUnderline : TextMarkerType.None;

                marker.ToolTip = task.Description;

                marker.Tag = task;
            }
        }
    }
}
