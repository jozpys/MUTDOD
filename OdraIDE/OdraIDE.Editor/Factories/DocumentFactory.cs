using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using OdraIDE.Editor;

namespace OdraIDE.Editor
{
    public class DocumentFactory
    {
        [Import(CompositionPoints.Workbench.Documents.SourceEditor)]
        private ExportFactory<SourceEditor> Factory { get; set; }

        [Export(typeof(CreateDocumentForFile))]
        public IDocument CreateDocumentForFile(OpenedFile file)
        {
            if (file.IsUntitled)
            {
                SourceEditor se = Factory.CreateExport().Value;
                se.File = file;
                return se;
            }
            else
            {
                string ext = Path.GetExtension(file.FileName);
                //TODO import dla jakich rozszerzen ten edytor
                if (ext == ".sbql")
                {
                    SourceEditor se = Factory.CreateExport().Value;
                    se.File = file;
                    return se;
                }
            }
            
            return null;
        }       
    }
}
