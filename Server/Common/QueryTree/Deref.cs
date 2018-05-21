using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    public class Deref : AbstractComposite
    {
        public Deref() : base(ElementType.DEREF) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var selectStatement = SingleElement();
            var selectResult = selectStatement.Execute(parameters);
            var classToGet = selectResult.QueryClass;
            var objs = selectResult.QueryObjects;

            var derefDto = new QueryDTO
            {
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.Default,
                    StringOutput = ToXml(objs, classToGet).OuterXml
                },
                QueryClass = classToGet,
                QueryObjects = objs
            };
            return derefDto;
        }

        private XmlDocument ToXml(IEnumerable<IStorable> toSave, Class @class)
        {
            var doc = new XmlDocument();
            var root = (XmlElement)doc.AppendChild(doc.CreateElement("result"));
            foreach (var obj in toSave)
            {
                var el = (XmlElement)root.AppendChild(doc.CreateElement(@class.Name));
                el.AppendChild(doc.CreateElement("Oid")).InnerText = obj.Oid.Id.ToString();
                foreach (var p in obj.Properties)
                {
                    String value = null;
                    if (p.Key.IsArray)
                    {
                        List<Object> array = (List<Object>)p.Value;
                        value = String.Join(", ", array);
                    }
                    else
                    {
                        value = p.Value.ToString();
                    }
                    el.AppendChild(doc.CreateElement(p.Key.Name)).InnerText = value;
                }
                    
            }
            return doc;
        }
    }
}
