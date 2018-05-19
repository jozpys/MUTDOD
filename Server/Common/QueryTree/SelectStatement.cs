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
        public SelectStatement() : base(ElementType.SELECT) { }

        public Dictionary<int, string> index { get; set; }
        public override bool IsOpenStackScope => true;

        public override QueryDTO Execute(QueryParameters parameters)
        {

            IQueryElement classNameElement = Element(ElementType.CLASS_NAME);
            QueryDTO classResult = classNameElement.Execute(parameters);
            if (classResult.Result != null)
            {
                return classResult;
            }
            var classToGet = classResult.QueryClass;
            var classParameter = parameters.Database.Schema.ClassProperties(classToGet);
            var objs = parameters.Storage.GetAll(parameters.Database.DatabaseId);
            objs = objs.Where(s => s.Properties.All(p => classParameter.Any(cp => cp.PropertyId.Id == p.Key.PropertyId.Id)));
            var selectDto = new QueryDTO { QueryClass = classToGet, QueryObjects = objs };
            parameters.Subquery = selectDto;

            if (TryGetElement(ElementType.WHERE, out IQueryElement searchCriteria))
            {
                QueryDTO whereDto = searchCriteria.Execute(parameters);
                if(whereDto.Result?.QueryResultType == ResultType.StringResult)
                {
                    return whereDto;
                }
                objs = whereDto.QueryObjects;
                parameters.Subquery.QueryObjects = objs;
            }
            if(TryGetElement(ElementType.CLASS_PROPERTY, out IQueryElement property))
            {
                var propertyValueDto = property.Execute(parameters);
                if (propertyValueDto.Result?.QueryResultType == ResultType.StringResult)
                {
                    return propertyValueDto;
                }
                objs = propertyValueDto.QueryObjects;
            }

            var getDto = new DTOQueryResult
            {
                NextResult = null,
                QueryResultType = ResultType.ReferencesOnly,
                QueryResults = objs.Select(o => o.Oid).ToList()
            };
            selectDto.Result = getDto;
            selectDto.QueryClass = classToGet;
            selectDto.QueryObjects = objs;
            return selectDto;
        }
    }
}
