using System;
using System.Collections.Concurrent;
using System.Xml.Linq;
using MUTDOD.Common.ModuleBase.Metamodel.DataStructure;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.ServerException;

namespace MUTDOD.Server.Common.MetamodelModule.DataStructure
{
    public abstract class MetaObject : IMetaObject
    {
        private string _nameSpace;

        private string _localName;

        private string _name;

        public Oid Oid { get; protected set; }

        public string Namespace
        {
            get { return _nameSpace; }
            protected set { _nameSpace = value; SetName(); }
        }

        public string LocalName
        {
            get { return _localName; }
            protected set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ServerExeptionMessage.MetaObjectLocalNameCannotBeEmpty);
                }
                _localName = value;
                SetName();
            }
        }

        public string Name { get; protected set; }

        public ConcurrentDictionary<string, IMetaLocation> Locations { get; protected set; }

        public ConcurrentDictionary<string, IMetaRelationship> Relationships { get; protected set; }

        protected MetaObject(Oid oid, string localName, string name, string nameSpace = "")
        {
            Oid = oid;
            _name = name;
            LocalName = localName.Trim();
            Namespace = nameSpace.Trim();

            Locations = new ConcurrentDictionary<string, IMetaLocation>();
            Relationships = new ConcurrentDictionary<string, IMetaRelationship>();
        }

        protected void SetName()
        {
            string name = String.Empty;

            if (!String.IsNullOrWhiteSpace(LocalName))
            {
                if (!String.IsNullOrWhiteSpace(Namespace))
                {
                    name = Namespace;
                    if (!Namespace.EndsWith(MetaObjectConstant.NamespaceSeparator))
                    {
                        name += MetaObjectConstant.NamespaceSeparator;
                    }
                }
                name += LocalName;
            }

            Name = name;
        }

        public bool AddMetaLocation(ref IMetaLocation metaLocation)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMetaLocation(string metaLocationName)
        {
            throw new NotImplementedException();
        }

        public virtual bool Remove(string metaObjectName)
        {
            throw new NotImplementedException();
        }

        public virtual bool UpdateData(ref IMetaObject metaObject)
        {
            throw new NotImplementedException();
        }

        public bool IsInMetaLocation(string metaLocationName)
        {
            return Locations.ContainsKey(metaLocationName);
        }

        public virtual XElement SaveToXml()
        {
            XElement objectElement = SaveToShortXml();

            XElement locationsElement = new XElement(XName.Get(MetaObjectConstant.LocationsElementName, String.Empty));
            objectElement.Add(locationsElement);

            foreach (var location in Locations.Values)
            {
                locationsElement.AddFirst(location.SaveToShortXml());
            }

            XElement relationshipsElement = new XElement(XName.Get(MetaObjectConstant.RelationshipsElementName, String.Empty));
            objectElement.Add(relationshipsElement);

            foreach (var relationship in Relationships.Values)
            {
                relationshipsElement.AddFirst(relationship.SaveToShortXml());
            }

            return objectElement;
        }

        public virtual XElement SaveToShortXml()
        {
            XName xname = XName.Get(GetType().Name, String.Empty);
            XElement xElement = new XElement(xname);

            return xElement;
        }

        protected static class MetaObjectConstant
        {
            public const string NamespaceSeparator = ".";

            public const string LocationsElementName = "Locations";
            public const string RelationshipsElementName = "Relationships";
        }
    }
}
