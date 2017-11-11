using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    abstract class MetaProperty : MetaObject, IMetaProperty
    {
        protected MetaProperty(Oid oid, string localName, string nameSpace) 
            : base(oid, localName, nameSpace)
        {
        }
    }
}
