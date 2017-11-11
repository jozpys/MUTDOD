using System;
using System.Collections.Generic;

namespace MUTDOD.Common.ModuleBase.Storage.Core.Metadata
{
    [Serializable]
    public class Class
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public ClassId ClassId { get; set; }
    }
    [Serializable]
    public class ClassId : IEquatable<ClassId>
    {
        public string Name { get; set; }
        public long Id { get; set; }

        public bool Equals(ClassId other)
        {
            if (other.Id != 0 && other.Id.Equals(Id))
            {
                return true;
            }
            if (other.Name != null && other.Name.Equals(Name))
            {
                return true;
            }
            return false;

        }
    }
}