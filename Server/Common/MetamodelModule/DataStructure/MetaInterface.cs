using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    public class MetaInterface : MetaType, IMetaInterface
    {
        public IDictionary<string, IMetaOperation> Operations { get; protected set; }
        public IDictionary<string, IMetaInterface> Derivments { get; protected set; }
        public IDictionary<string, IMetaInterface> Inheritances { get; protected set; }

        public MetaInterface(Oid oid, string localName, string nameSpace)
            : base(oid, localName, nameSpace)
        {
            Operations = new ConcurrentDictionary<string, IMetaOperation>();
            Derivments = new ConcurrentDictionary<string, IMetaInterface>();
            Inheritances = new ConcurrentDictionary<string, IMetaInterface>();
        }
        
        public bool AddMetaOperation(ref IMetaOperation metaOperaton)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaOperation(string metaAttributeName)
        {
            throw new NotImplementedException();
        }

        public bool AddDerivement(ref IMetaInterface metaInterface)
        {
            throw new NotImplementedException();
        }

        public bool RemoveDerivement(string metaInterfaceName)
        {
            throw new NotImplementedException();
        }

        public bool AddInheritance(ref IMetaInterface metaInterface)
        {
            throw new NotImplementedException();
        }

        public bool RemoveInheritance(string metaInterfaceName)
        {
            throw new NotImplementedException();
        }
    }
}
