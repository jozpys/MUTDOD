using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OdraIDE.Core.Layout
{
    public class DocumentItem
    {
        [XmlAttribute("Name")]
        public string name;
        [XmlAttribute("Memento")]
        public string memento;

        public DocumentItem()
        {
        }

        public DocumentItem(string Name, string Memento)
        {
            name = Name;
            memento = Memento;
        }
    }
}
