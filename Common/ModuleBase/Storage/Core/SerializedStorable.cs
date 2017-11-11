using System;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage.Core
{
    public class SerializedStorable : IComparable
    {
        public byte[] Data { get; set; }
        public Oid Oid { get; set; }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            var ss = obj as SerializedStorable;
            if (ss != null)
                return this.CompareTo(ss);
            else
                throw new ArgumentException("Object is not a SerializedStorable");
        }

        public int CompareTo(SerializedStorable obj)
        {
            return Oid.CompareTo(obj);
        }
    }
}