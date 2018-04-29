using System;
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            IStorable storable = obj as IStorable;
            if (storable != null)
                return Oid == storable.Oid;
            else
                throw new ArgumentException("Object is not a Storable");
        }

        public override int GetHashCode()
        {
            return Oid.GetHashCode();
        }
    }
}
