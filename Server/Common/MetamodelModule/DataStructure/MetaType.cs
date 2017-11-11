using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    public abstract class MetaType : MetaObject, IMetaType
    {
        protected MetaType(Oid oid, string localName, string nameSpace) 
            : base(oid, localName, nameSpace)
        {
        }
    }
}
