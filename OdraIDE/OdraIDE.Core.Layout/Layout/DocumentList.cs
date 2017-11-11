using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace OdraIDE.Core.Layout
{
    [XmlRoot("DocumentList")]
    public class DocumentList
    {
        private ArrayList listOfDocs = new ArrayList();

        [XmlElement("Item")]
        public DocumentItem[] Items
        {
            get
            {
                DocumentItem[] items = new DocumentItem[listOfDocs.Count];
                listOfDocs.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null) return;
                DocumentItem[] items = (DocumentItem[])value;
                listOfDocs.Clear();
                foreach (DocumentItem item in items)
                    listOfDocs.Add(item);
            }
        }

        public int AddItem(DocumentItem item)
        {
            return listOfDocs.Add(item);
        }
    }
}
