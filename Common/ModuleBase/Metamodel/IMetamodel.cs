using System.Collections.Generic;
using System.Xml.Linq;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;

namespace MUTDOD.Common.ModuleBase.Metamodel
{
    public interface IMetamodel
    {
        IDictionary<string, IMetaObject> MetaObjects { get; }
        IDictionary<string, IMetaType> MetaTypes { get; }
        IDictionary<string, IMetaClass> MetaClasses { get; }
        IDictionary<string, IMetaInterface> MetaInterfaces { get; }
        IDictionary<string, IMetaLocation> MetaLocations{ get; }

        bool AddMetaObject(ref IMetaObject metaObject);
        bool AddMetaObjectLocation(string metaObjectName, string metaLocationName);
        bool UpdateMetaObject(ref IMetaObject metaObject);
        bool UpdateMetaObjectMetaData(ref IMetaObject metaObject);
        bool UpdateMetaObjectMetaLocation(string metaObjectName, ref IMetaLocation metaLocation);
        bool RemoveMetaObject(string metaObjectName);
        bool RemoveMetaObjectLocation(string metaObjectName, string metaLocationName);
        bool HasMetaObject(string metaObjectName);
        IMetaObject GetMetaObject(string metaObjectName);
        bool UpdateMetaClassMetaData(ref IMetaClass metaObject);
        bool HasMetaClass(string className);
        IMetaClass GetMetaClass(string className);
        bool HasMetaClassMetaAttribute(string className, string attributeName);
        IMetaAttribute GetMetaClassMetaAttribute(string className, string attributeName);
        bool HasMetaClassMetaOperation(string className, string operationName);
        IMetaOperation GetMetaClassMetaOperation(string className, string operationName);
        bool HasMetaClassMetaRelationship(string classRelationship, string relationshipName);
        IMetaRelationship GetMetaClassMetaRelationship(string classRelationship, string relationshipName);
        bool AddMetaType(ref IMetaType metaType);
        bool UpdateMetaType(ref IMetaType metaType);
        bool RemoveMetaType(string metaTypeName);
        bool UpdateMetaTypeMetaData(ref IMetaType metaType);
        bool HasMetaType(string metaTypeName);
        IMetaType GetMetaType(string metaTypeName);
        bool AddMetaTypeMetaAttribute(string metaTypeName, ref IMetaAttribute metaAttribute);
        bool UpdateMetaTypeMetaAttribute(string metaTypeName, ref IMetaAttribute metaAttribute);
        bool RemoveMetaTypeMetaAttribute(string metaTypeName, string metaAttribute);
        bool UpdateMetaTypeMetaAttributeMetaData(string metaTypeName, ref IMetaAttribute metaAttribute);
        bool HasMetaTypeMetaAttribute(string className, string attributeName);
        IMetaAttribute GetMetaTypeMetaAttribute(string typeName, string attributeName);
        bool AddMetaTypeMetaOperation(string metaTypeName, ref IMetaOperation metaOperation);
        bool UpdateMetaTypeMetaOperation(string metaTypeName, ref IMetaOperation metaOperation);
        bool RemoveMetaTypeMetaOperation(string metaTypeName, string metaOperationName);
        bool HasMetaTypeMetaOperation(string className, string operationName);
        IMetaOperation GetMetaTypeMetaOperation(string metaTypeName, string metaOperationName);
        bool AddMetaTypeMetaRelationship(string metaTypeName, ref IMetaRelationship metaRelationship);
        bool UpdateMetaTypeMetaRelationship(string metaTypeName, ref IMetaRelationship metaRelationship);
        bool RemoveMetaTypeMetaRelationship(string metaTypeName, string metaRelationshipName);
        bool HasMetaTypeMetaRelationship(string metaTypeName, string metaRelationshipName);
        IMetaRelationship GetMetaTypeMetaRelationship(string metaTypeName, string metaRelationshipName);
        bool UpdateFromXml(string metamodelXml);
        string SaveToXml();
        bool LoadFromXml(XElement metamodelElement);
        bool Init(ref ICore odbms);
    }
}