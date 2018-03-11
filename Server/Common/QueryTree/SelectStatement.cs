using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using System.Xml;
using MUTDOD.Common.Types;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using System.Runtime.Serialization;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class SelectStatement : AbstractComposite
    {
        public SelectStatement() : base(ElementType.SELECT)
        {
            Deref = false;
        }
        [DataMember]
        public Boolean Deref { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {

            IQueryElement classNameElement = elements[ElementType.CLASS_NAME];
            QueryDTO classResult = classNameElement.Execute(parameters);
            if (classResult.Result != null)
            {
                return classResult;
            }
            var classToGet = classResult.QueryClass;
            var objs = parameters.Storage.GetAll(parameters.Database.DatabaseId);
            objs = objs.Where(s => s.Properties.All(p => p.Key.ParentClassId == classToGet.ClassId.Id));
            var selectDto = new QueryDTO { QueryClass = classToGet, QueryObjects = objs };

            if (elements.TryGetValue(ElementType.WHERE, out IQueryElement seachCriteria))
            {
                parameters.Subquery = selectDto;
                QueryDTO whereDto = seachCriteria.Execute(parameters);
                if(whereDto.Result?.QueryResultType == ResultType.StringResult)
                {
                    return whereDto;
                }
                objs = whereDto.QueryObjects;
            }

            if (Deref) {
                var derefResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.Default,
                    StringOutput = ToXml(objs, classToGet).OuterXml
                };
                selectDto.Result = derefResult;
                return selectDto;
            }

            var getDto = new DTOQueryResult
            {
                NextResult = null,
                QueryResultType = ResultType.ReferencesOnly,
                QueryResults = objs.Select(o => o.Oid).ToList()
            };
            selectDto.Result = getDto;
            return selectDto;
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
                    el.AppendChild(doc.CreateElement(p.Key.Name)).InnerText = p.Value.ToString();
            }
            return doc;
        }
    }
}
