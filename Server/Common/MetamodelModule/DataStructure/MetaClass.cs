using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Linq;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    internal class MetaClass : IMetaClass
    {
        public MetaClass(Oid oid, string name, string localName, string @namespace, IDictionary<string, IMetaOperation> operations, IDictionary<string, IMetaAttribute> attributes)
        {
            Oid = oid;
            Namespace = @namespace;
            LocalName = localName;
            Name = name;
            Operations = operations;
            Attributes = attributes;
        }

        public Oid Oid { get; private set; }
        public string Namespace { get; private set; }
        public string LocalName { get; private set; }
        public string Name { get; private set; }

        public IDictionary<string, IMetaOperation> Operations { get; private set; }
        public IDictionary<string, IMetaInterface> Derivments { get; private set; }
        public IDictionary<string, IMetaInterface> Inheritances { get; private set; }
        public ConcurrentDictionary<string, IMetaLocation> Locations { get; private set; }
        public ConcurrentDictionary<string, IMetaRelationship> Relationships { get; private set; }
        public IDictionary<string, IMetaAttribute> Attributes { get; private set; }
        public IMetaClass ExtendedClass { get; set; }
        public IDictionary<string, IMetaClass> Extensions { get; private set; }

        public bool AddMetaLocation(ref IMetaLocation metaLocation)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveMetaLocation(string metaLocationName)
        {
            throw new System.NotImplementedException();
        }

        public bool IsInMetaLocation(string metaLocationName)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(string metaObjectName)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateData(ref IMetaObject metaObject)
        {
            throw new System.NotImplementedException();
        }

        public XElement SaveToXml()
        {
            throw new System.NotImplementedException();
        }

        public XElement SaveToShortXml()
        {
            throw new System.NotImplementedException();
        }

        public bool AddMetaOperation(ref IMetaOperation metaOperaton)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveMetaOperation(string metaAttributeName)
        {
            throw new System.NotImplementedException();
        }

        public bool AddDerivement(ref IMetaInterface metaInterface)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveDerivement(string metaInterfaceName)
        {
            throw new System.NotImplementedException();
        }

        public bool AddInheritance(ref IMetaInterface metaInterface)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveInheritance(string metaInterfaceName)
        {
            throw new System.NotImplementedException();
        }


        public bool AddMetaAttribute(ref IMetaAttribute metaAttribute)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveMetaAttribute(string metaAttributeName)
        {
            throw new System.NotImplementedException();
        }

        public bool AddExtension(ref IMetaClass metaClass)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveExtension(string metaClassName)
        {
            throw new System.NotImplementedException();
        }
    }
}
