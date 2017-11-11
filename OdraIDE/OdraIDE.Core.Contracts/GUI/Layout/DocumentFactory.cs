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
    public class DocumentFactory : IPartImportsSatisfiedNotification
    {
        public DocumentFactory()
        {
        }

        //static readonly DocumentFactory instance = new DocumentFactory();

        //public static DocumentFactory Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}
        
        [ImportMany(typeof(CreateDocumentForFile))]
        private IEnumerable<CreateDocumentForFile> createDocumentMethods { get; set; }

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
    
        #region IPartImportsSatisfiedNotification Members

        public void  OnImportsSatisfied()
        {

        }

        #endregion
    }
}
