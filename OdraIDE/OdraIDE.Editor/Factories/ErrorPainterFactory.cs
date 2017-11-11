using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.Editor
{
    public class ErrorPainterFactory
    {
        [Import(CompositionPoints.Workbench.Painters.ErrorPainter)]
        private ExportFactory<ErrorPainter> Factory { get; set; }

        [Export(typeof(CreatePainterForDocument))]
        public IPainter CreatePainterForDocument(IDocument document)
        {
            if (document is ISourceEditor)
            {
                ErrorPainter ep = Factory.CreateExport().Value;
                ep.Document = document;

                return ep;
            }
            return null;
        }       
    }
}