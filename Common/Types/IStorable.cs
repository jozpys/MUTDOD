using System.Collections.Generic;

namespace MUTDOD.Common.Types
{
    public interface IStorable
    {
        Oid Oid { get; set; }
        Dictionary<Property, object> Properties { get; set; }      
    }
}
