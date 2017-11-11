using System;

namespace MUTDOD.Common.Types
{
    public abstract class DynamicRole : Oid
    {
        protected DynamicRole(Guid id, uint oli) : base(id,oli)
        {
        }
    }
}
