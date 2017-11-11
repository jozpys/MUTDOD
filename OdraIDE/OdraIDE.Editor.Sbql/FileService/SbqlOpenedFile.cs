using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;

namespace OdraIDE.Editor.Sbql
{
    public class SbqlOpenedFile : OpenedFile
    {
        public SbqlOpenedFile(FileName fileName)
            : base(fileName)
        {

        }

        public SbqlOpenedFile(FileName fileName, bool isUntitled)
            : base(fileName, isUntitled)
        {

        }

        public string Query
        {
            get
            {
                return (Document as ISourceEditor).SourceCode;
            }
        }

        public string SelectedQuery
        {
            get
            {
                return (Document as ISourceEditor).SelectedSourceCode;
            }
        }
    }
}
