using System;
using System.Runtime.Serialization;

namespace MUTDOD.Common.Types
{
    [DataContract]
    public class Oid : IComparable
    {
        public Oid(Guid id, uint oli)
        {
            Id = id;
            Oli = oli;
        }
        /// <summary>
        /// Object location ID
        /// </summary>
        [DataMember]
        public uint Oli { get; private set; }

        /// <summary>
        /// Unique id
        /// </summary>
        [DataMember]
        public Guid Id { get; private set; }

        public static bool operator ==(Oid obj1, Oid obj2)
        {
            if (object.ReferenceEquals(obj1, null))
                return object.ReferenceEquals(obj2, null);
            else if (object.ReferenceEquals(obj2, null))
                return false;
            return obj1.Id == obj2.Id
                && obj1.Oli == obj2.Oli;
        }

        public static bool operator !=(Oid obj1, Oid obj2)
        {
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            return string.Format("[{0}]{1}", Oli, Id);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Oid oid = obj as Oid;
            if (oid != null)
                return this.CompareTo(oid);
            else
                throw new ArgumentException("Object is not a Oid");
        }

        public int CompareTo(Oid obj)
        {
            if (Oli == obj.Oli)
                return Id.CompareTo(obj.Id);

            return Oli.CompareTo(obj.Oli);
        }
    }
}
