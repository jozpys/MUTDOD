using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUTDOD.Common.Types
{
    /// <summary>
    /// Database Identifier
    /// </summary>
    [Serializable]
    public class Did : IEquatable<Did>, IComparable
    {
        /// <summary>
        /// Database location ID
        /// </summary>
        public uint Dli { get; set; }

        /// <summary>
        /// Database unique identifier
        /// </summary>
        public ulong Duid { get; set; }

        public bool Equals(Did other)
        {
            if (other.Dli == Dli && other.Duid == Duid)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            var did = obj as Did;
            if (did == null)
            {
                return false;
            }
            if (did.Dli == Dli && did.Duid == Duid)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Dli.GetHashCode()*Duid.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Dli, Duid);
        }

        public static Did CreateNew()
        {
            //TODO: podawać dobre parametry
            return new Did {Dli = 0, Duid = 1};
        }


        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Did did = obj as Did;
            if (did != null)
                return this.CompareTo(did);
            else
                throw new ArgumentException("Object is not a Did");
        }

        public int CompareTo(Did obj)
        {
            if (Dli == obj.Dli)
                return Duid.CompareTo(obj.Duid);

            return Dli.CompareTo(obj.Dli);
        }
    }
}
