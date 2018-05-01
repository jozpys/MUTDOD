using System;
using MUTDOD.Common.Types;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Common.ModuleBase
{
    public interface IServerSchemaStats
    {
        void PropertyValue(Property property, Object value);
        int GetClassObjectNumber(Did databaseId, ClassId classId);
        int PropertyValueNumber(Property property);
        bool RecalculateStats();
    }
}