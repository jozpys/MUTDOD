using System.Collections.Concurrent;
using System.Xml.Linq;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaObject
    {
        Oid Oid { get; }
        string Namespace { get; }
        string LocalName { get; }
        string Name { get; }

        ConcurrentDictionary<string, IMetaLocation> Locations { get; }
        ConcurrentDictionary<string, IMetaRelationship> Relationships { get; }

        bool AddMetaLocation(ref IMetaLocation metaLocation);
        bool RemoveMetaLocation(string metaLocationName);
        bool IsInMetaLocation(string metaLocationName);

        bool Remove(string metaObjectName);
        bool UpdateData(ref IMetaObject metaObject);

        XElement SaveToXml();
        XElement SaveToShortXml();
    }
}
