

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Linq;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Metamodel;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;

namespace MUTDOD.Server.Common.MetamodelModule
{
    public class OMetamodel : IMetamodel
    {
        #region Metamodel Members Lists

        public IDictionary<string, IMetaObject> MetaObjects { get; private set; }

        public IDictionary<string, IMetaType> MetaTypes { get; private set; }

        public IDictionary<string, IMetaClass> MetaClasses { get; private set; }

        public IDictionary<string, IMetaInterface> MetaInterfaces { get; private set; }

        public IDictionary<string, IMetaLocation> MetaLocations { get; private set; }

        #endregion

        private void Init()
        {
            MetaObjects = new ConcurrentDictionary<string, IMetaObject>();
            MetaTypes = new ConcurrentDictionary<string, IMetaType>();
            MetaClasses = new ConcurrentDictionary<string, IMetaClass>();
            MetaInterfaces = new ConcurrentDictionary<string, IMetaInterface>();
            MetaLocations = new ConcurrentDictionary<string, IMetaLocation>();
        }

        public OMetamodel()
        {
            Init();
        }

        public bool AddMetaObject(ref IMetaObject metaObject)
        {
            throw new NotImplementedException();
        }

        public bool AddMetaObjectLocation(string metaObjectName, string metaLocationName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaObject(ref IMetaObject metaObject)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaObjectMetaData(ref IMetaObject metaObject)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaObjectMetaLocation(string metaObjectName, ref IMetaLocation metaLocation)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaObject(string metaObjectName)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaObjectLocation(string metaObjectName, string metaLocationName)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaObject(string metaObjectName)
        {
            throw new NotImplementedException();
        }

        public IMetaObject GetMetaObject(string metaObjectName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaClassMetaData(ref IMetaClass metaClass)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaClass(string className)
        {
            throw new NotImplementedException();
        }

        public IMetaClass GetMetaClass(string className)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaClassMetaAttribute(string className, string attributeName)
        {
            throw new NotImplementedException();
        }

        public IMetaAttribute GetMetaClassMetaAttribute(string className, string attributeName)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaClassMetaOperation(string className, string operationName)
        {
            throw new NotImplementedException();
        }

        public IMetaOperation GetMetaClassMetaOperation(string className, string operationName)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaClassMetaRelationship(string classRelationship, string relationshipName)
        {
            throw new NotImplementedException();
        }

        public IMetaRelationship GetMetaClassMetaRelationship(string classRelationship, string relationshipName)
        {
            throw new NotImplementedException();
        }

        public bool AddMetaType(ref IMetaType metaType)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaType(ref IMetaType metaType)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaType(string metaTypeName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaTypeMetaData(ref IMetaType metaType)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaType(string metaTypeName)
        {
            throw new NotImplementedException();
        }

        public IMetaType GetMetaType(string metaTypeName)
        {
            throw new NotImplementedException();
        }

        public bool AddMetaTypeMetaAttribute(string metaTypeName, ref IMetaAttribute metaAttribute)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaTypeMetaAttribute(string metaTypeName, ref IMetaAttribute metaAttribute)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaTypeMetaAttribute(string metaTypeName, string metaAttribute)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaTypeMetaAttributeMetaData(string metaTypeName, ref IMetaAttribute metaAttribute)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaTypeMetaAttribute(string className, string attributeName)
        {
            throw new NotImplementedException();
        }

        public IMetaAttribute GetMetaTypeMetaAttribute(string typeName, string attributeName)
        {
            throw new NotImplementedException();
        }

        public bool AddMetaTypeMetaOperation(string metaTypeName, ref IMetaOperation metaOperation)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaTypeMetaOperation(string metaTypeName, ref IMetaOperation metaOperation)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaTypeMetaOperation(string metaTypeName, string metaOperationName)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaTypeMetaOperation(string className, string operationName)
        {
            throw new NotImplementedException();
        }

        public IMetaOperation GetMetaTypeMetaOperation(string metaTypeName, string metaOperationName)
        {
            throw new NotImplementedException();
        }

        public bool AddMetaTypeMetaRelationship(string metaTypeName, ref IMetaRelationship metaRelationship)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMetaTypeMetaRelationship(string metaTypeName, ref IMetaRelationship metaRelationship)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaTypeMetaRelationship(string metaTypeName, string metaRelationshipName)
        {
            throw new NotImplementedException();
        }

        public bool HasMetaTypeMetaRelationship(string metaTypeName, string metaRelationshipName)
        {
            throw new NotImplementedException();
        }

        public IMetaRelationship GetMetaTypeMetaRelationship(string metaTypeName, string metaRelationshipName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFromXml(string metamodelXml)
        {
            throw new NotImplementedException();
        }

        public string SaveToXml()
        {
            throw new NotImplementedException();
        }

        public bool LoadFromXml(XElement metamodelElement)
        {
            throw new NotImplementedException();
        }

        public bool Init(ref ICore odbms)
        {
            throw new NotImplementedException();
        }
    }
}