using System.Collections.Generic;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage
{
    public class Storable : IStorable
    {
        public Storable()
        {
            if (Properties == null)
            {
                Properties = new Dictionary<Property, object>();
            }
        }
        public Oid Oid { get; set; }
        public Dictionary<Property, object> Properties { get; set; }
        //public Dictionary<IMethod, string> Methods { get; set; }
    }
}
