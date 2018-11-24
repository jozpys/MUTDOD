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
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class DropClass : AbstractComposite
    {
        public DropClass() : base(ElementType.DROP_CLASS) { }

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
            if (classResult.Result != null || classResult.QueryClass.Interface)
            {
                var noInterfaceResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No class with name: " + classResult.QueryClass.Name
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
                    StringOutput = "Can't drop class with childs: " + String.Join(", ", childClasses.Select( c => c.Name)) + ". Drop childs first."
                };
                return new QueryDTO() { Result = noInterfaceResult };
            }

            IEnumerable<Property> propeteries = parameters.Database.Schema.Properties.Values.Where(p => p.ParentClassId == classToDrop.ClassId.Id);
            foreach (var parameterToDrop in propeteries)
            {
                Boolean success = parameters.Database.Schema.Properties.TryRemove(parameterToDrop.PropertyId, out Property dropedProperty);

                if (!success)
                {
                    var errorMessage = new DTOQueryResult()
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Error while droping attribute: " + dropedProperty.Name
                    };
                    return new QueryDTO() { Result = errorMessage };
                }
            }

            parameters.Database.Schema.Methods.TryRemove(classToDrop.ClassId, out List<IMethod> dropedMethods);

            if (!parameters.Database.Schema.Classes.TryRemove(classToDrop.ClassId, out Class dropedClass))

            {
                parameters.Log("Could not drop class", MessageLevel.Error);
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

            parameters.Storage.SaveSchema(parameters.Database.Schema);

            parameters.Log("Droped class: " + dropedClass.Name, MessageLevel.QueryExecution);
            return new QueryDTO()
            {
                Result = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Class:" + dropedClass.Name + " droped."
                }
            };
        }
    }
}
