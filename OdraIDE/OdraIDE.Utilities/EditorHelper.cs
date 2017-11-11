using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;

namespace OdraIDE.Utilities
{
    public static class EditorHelper
    {
        /// <summary>
        /// Gets the word at the specified position.
        /// </summary>
        public static string GetWordAt(TextDocument document, int offset)
        {
            if (offset < 0 || offset >= document.TextLength || !IsWordPart(document.GetCharAt(offset)))
            {
                return String.Empty;
            }
            int startOffset = offset;
            int endOffset = offset;
            while (startOffset > 0 && IsWordPart(document.GetCharAt(startOffset - 1)))
            {
                --startOffset;
            }

            while (endOffset < document.TextLength - 1 && IsWordPart(document.GetCharAt(endOffset + 1)))
            {
                ++endOffset;
            }

            //Debug.Assert(endOffset >= startOffset);
            return document.GetText(startOffset, endOffset - startOffset + 1);
        }

        static bool IsWordPart(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_';
        }
    }
}
