using System;
using MUTDOD.Common.Types;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace ServerStats
{
    interface IStats
    {
        void propertyValue(Property property, Object value);
        int getClassObjectNumber(Did databaseId, ClassId classId);
        int propertyValueNumber(Property property);
        bool recalculateStats();
    }
}
