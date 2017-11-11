using System;
using System.Collections.Concurrent;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    class MetaOperation : MetaObject, IMetaOperation
    {
        public IMetaType Owner { get; protected set; }

        public IMetaType Result { get; protected set; }

        public ConcurrentDictionary<string, IMetaType> Parameters { get; protected set; }

        public MetaOperation(Oid oid, string localName, IMetaType result, string nameSpace = "")
            : base(oid, localName, nameSpace)
        {
            if (result == null)
            {
                Result = null;
            }

            Parameters = new ConcurrentDictionary<string, IMetaType>();
        }

        public void AddParameter(ref IMetaType parameterMetaType, string parameterName)
        {
            throw new NotImplementedException();
        }

        public void RemoveParameter(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected static class MetaOperationConstant
        {

        }
    }
}
