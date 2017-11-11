using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace OdraIDE.Editor.Sbql
{
    [Export(CompositionPoints.Workbench.Commands.CommentLines, typeof(ICustomCommand))]
    public class CommentLinesCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (Ctrl + /)
                return new KeyBinding(this, new KeyGesture(Key.OemQuestion, ModifierKeys.Control));
            }
        }

        public CommentLinesCommand()
        {
            EnableCondition = new ConcreteCondition(true);
            ExecuteCommand += new ExecuteHandler(CommentOutLinesCommand_ExecuteCommand);
        }

        void CommentOutLinesCommand_ExecuteCommand()
        {
            string comment = "//";

            OpenedFile file = fileService.Value.GetActiveFile();
            if (file != null && file is SbqlOpenedFile)
            {
                SbqlOpenedFile sbqlFile = file as SbqlOpenedFile;
                TextEditor editor = (sbqlFile.Document as ISourceEditor).TextEditor;
                var selection = editor.TextArea.Selection as Selection;
                editor.Document.UndoStack.StartUndoGroup();
                if (selection.IsMultiline/*(editor.Document)*/)
                {
                    int startOffset = editor.TextArea.Document.GetOffset(selection.StartPosition.Location);
                    int endOffset = editor.TextArea.Document.GetOffset(selection.EndPosition.Location);
                    DocumentLine startline; 
                    DocumentLine endline;
                    if (startOffset < endOffset)
                    {
                        startline = editor.Document.GetLineByOffset(startOffset);
                        endline = editor.Document.GetLineByOffset(endOffset);
                    }
                    else
                    {
                        startline = editor.Document.GetLineByOffset(endOffset);
                        endline = editor.Document.GetLineByOffset(startOffset);
                    }

                    DocumentLine currentLine = startline;
                    do
                    {
                        CommentLine(currentLine, editor, comment);
                        currentLine = currentLine.NextLine;
                    } while (currentLine != null && currentLine.LineNumber != (endline.LineNumber + 1));
                }
                else
                {
                    DocumentLine line = editor.Document.GetLineByOffset(editor.CaretOffset);
                    CommentLine(line, editor, comment);
                }
                editor.Document.UndoStack.EndUndoGroup();
            }
        }

        private void CommentLine(DocumentLine line, TextEditor editor, string comment)
        {
            string lineText = editor.Document.GetText(line);
            bool removeComment = lineText.Trim().StartsWith(comment);
            if (removeComment)
            {
                editor.Document.Remove(line.Offset + lineText.IndexOf(comment), comment.Length);
            }
            else
            {
                editor.Document.Insert(line.Offset, comment);
            }
        }

        public void OnImportsSatisfied()
        {
            
        }
    }
}
