using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class ParentClasses : AbstractComposite
    {
        public ParentClasses() : base(ElementType.PARENT_CLASSES) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            ISet<Class> parenClasses = new HashSet<Class>();
            foreach ( var parentClass in AllElements(ElementType.CLASS_NAME))
            {
                IQueryElement parentClassElement = Element(ElementType.CLASS_NAME);
                QueryDTO classResult = parentClassElement.Execute(parameters);
                if (classResult.Result != null)
                {
                    return classResult;
                }
                parenClasses.Add(classResult.QueryClass);
            }

            return new QueryDTO { Value = parenClasses };
        }
    }
}
