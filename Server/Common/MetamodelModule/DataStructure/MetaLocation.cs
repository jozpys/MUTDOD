using System;
using System.Collections.Concurrent;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    class MetaLocation : MetaObject, IMetaLocation
    {
        public ConcurrentDictionary<string, IMetaObject> MetaObjects { get; set; }

        public MetaLocation(Oid oid, string localName, string nameSpace = "")
            : base(oid, localName, nameSpace)
        {
            MetaObjects = new ConcurrentDictionary<string, IMetaObject>();
        }

        public bool AddMetaObject(ref IMetaLocation metaObject)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaObject(string metaObjectName)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaObject(string metaObjectName)
        {
            throw new NotImplementedException();
        }
    }
}
