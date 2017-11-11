using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    class MetaAttribute : MetaObject,IMetaAttribute
    {
        public MetaAttribute(Oid oid, string localName, string name, string nameSpace = "") : base(oid, localName, name, nameSpace)
        {
        }

        public IMetaType Owner { get; set; }
        public IMetaType Type { get; set; }
    }
}
