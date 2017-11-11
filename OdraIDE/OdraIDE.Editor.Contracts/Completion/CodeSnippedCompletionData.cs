using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace OdraIDE.Editor
{
    public class RelativeSelection
    {
        public RelativeSelection(int start, int length)
        {
            this.Start = start;
            this.Length = length;
        }

        public int Start
        {
            get;
            private set;
        }

        public int Length
        {
            get;
            private set;
        }
    }

    public class CodeSnippetCompletionData : ICompletionData
    {
        private string m_name;
        private string m_snipped;
        private int m_caretRelativePosition;

        private object m_description;

        public CodeSnippetCompletionData(string name, string snipped, int caretRelativePosition = -0)
        {
            this.m_name = name;
            this.m_snipped = snipped;
            this.m_caretRelativePosition = caretRelativePosition;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text
        {
            get { return m_snipped; }
        }

        public object Content
        {
            get { return m_name; }
        }

        public object Description
        {
            get
            {
                return m_description ?? "Code snipped for '" + m_name + "' statement";
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Description");
                }
                m_description = value;
            }
        }

        public double Priority
        {
            get { return 1; }
        }

        public RelativeSelection RelativeSelection
        {
            get;
            set;
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
            if (RelativeSelection != null)
            {
                int offset = textArea.Caret.Offset;
                int startSegment = offset - completionSegment.Length;
                int start = startSegment + RelativeSelection.Start;
                int end = start + RelativeSelection.Length;
                textArea.Selection = Selection.Create(textArea,start, end);
                textArea.Caret.Offset = end;
            }
            else
            {
                textArea.Caret.Offset = textArea.Caret.Offset + m_caretRelativePosition;
            }

        }
    }
}
