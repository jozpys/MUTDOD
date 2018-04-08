using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class DropInterface : AbstractComposite
    {
        public DropInterface() : base(ElementType.DROP_INTERFACE) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            if (parameters.Database == null)
            {
                parameters.Log("Database is required!", MessageLevel.Error);
                return new QueryDTO()
                {
                    Result = new DTOQueryResult()
                    {
                        NextResult = null,
                        QueryResults = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Error ocured while class droping"
                    }
                };
            }

            IQueryElement classNameElement = Element(ElementType.CLASS_NAME);
            QueryDTO classResult = classNameElement.Execute(parameters);
            if (classResult.Result != null || !classResult.QueryClass.Interface)
            {
                var noInterfaceResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No interface with name: " + classResult.QueryClass.Name
                };
                return new QueryDTO() { Result = noInterfaceResult };
            }

            Class classToDrop = classResult.QueryClass;

            var childClasses = parameters.Database.Schema.Classes.Values.Where(cl => cl.Parent != null && cl.Parent.Where(p => p.ClassId.Id == classToDrop.ClassId.Id).Any());
            if (childClasses.Any())
            {
                var noInterfaceResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Can't drop interface with childs: " + String.Join(", ", childClasses.Select( c => c.Name)) + ". Drop childs first."
                };
                return new QueryDTO() { Result = noInterfaceResult };
            }

            if (!parameters.Database.Schema.Classes.TryRemove(classToDrop.ClassId, out Class dropedClass))
            {
                parameters.Log("Could not drop interface", MessageLevel.Error);
                return new QueryDTO()
                {
                    Result = new DTOQueryResult()
                    {
                        NextResult = null,
                        QueryResults = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Error ocured while interface droping"
                    }
                };
            }
            parameters.Storage.SaveSchema(parameters.Database.Schema);

            parameters.Log("Droped interface: " + dropedClass.Name, MessageLevel.QueryExecution);
            return new QueryDTO()
            {
                Result = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Interface:" + dropedClass.Name + " droped."
                }
            };
        }
    }
}
