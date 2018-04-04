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
            if (classResult.Result != null)
            {
                return classResult;
            }

            Class classToDrop = classResult.QueryClass;

            if (!parameters.Database.Schema.Classes.TryRemove(classToDrop.ClassId, out Class dropedClass))
            {
                parameters.Log("Could not define new", MessageLevel.Error);
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
