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
    public class ParentClasses : AbstractComposite
    {
        public ParentClasses() : base(ElementType.PARENT_CLASSES) { }

        public bool InterfaceOnly { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            ISet<Class> parenClasses = new HashSet<Class>();
            int numberOfClasses = 0;
            foreach ( var parentClass in AllElements(ElementType.CLASS_NAME))
            {
                QueryDTO classResult = parentClass.Execute(parameters);
                if (classResult.Result != null)
                {
                    return classResult;
                }

                if (!classResult.QueryClass.Interface)
                {
                    ++numberOfClasses;
                }
                parenClasses.Add(classResult.QueryClass);
            }

            if(InterfaceOnly && numberOfClasses > 0)
            {
                return new QueryDTO
                {
                    Result = new DTOQueryResult()
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "An interface can't inherit from a class!"
                    }
                };
            }

            if (numberOfClasses > 1)
            {
                return new QueryDTO
                {
                    Result = new DTOQueryResult()
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "A class can't inherit from more than 1 class!"
                    }
                };
            }

            return new QueryDTO { Value = parenClasses };
        }
    }
}
