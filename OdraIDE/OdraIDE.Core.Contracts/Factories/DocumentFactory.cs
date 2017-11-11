using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core
{
    public delegate IDocument CreateDocumentForFile(OpenedFile file);

    /// <summary>
    /// 
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(DocumentFactory))]
    public class DocumentFactory
    {        
        [ImportMany(typeof(CreateDocumentForFile))]
        private IEnumerable<CreateDocumentForFile> createDocumentMethods { get; set; }

        /// <summary>
        /// Invoke all imported factory methods. If one creates document then returns this document
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Document for file</returns>
        public IDocument CreateDocumentForFile(OpenedFile file)
        {
            IDocument doc;

            foreach (CreateDocumentForFile method in createDocumentMethods)
            {
                doc = method(file);
                if (doc != null)
                {
                    return doc;
                }
            }

            return null;
        }
    }
}
