using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core
{
    public delegate IPainter CreatePainterForDocument(IDocument document);

    [Export(typeof(PainterFactory))]
    public class PainterFactory
    {
        [ImportMany(typeof(IPainter))]
        private IEnumerable<IPainter> painters;

        [ImportMany(typeof(CreatePainterForDocument))]
        private IEnumerable<CreatePainterForDocument> createPainterMethods { get; set; }

        /// <summary>
        /// Invoke all imported factory methods. If one creates painter then returns this painter
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>Painter for document</returns>
        public IPainter CreatePainterForDocument(IDocument doc)
        {
            IPainter painter;

            foreach (CreatePainterForDocument method in createPainterMethods)
            {
                painter = method(doc);
                if (painter != null)
                {
                    return painter;
                }
            }

            return null;
        }
    }
}
