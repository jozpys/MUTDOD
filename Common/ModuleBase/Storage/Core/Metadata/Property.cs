using System;

namespace MutDood.Storage.Interfaces.Core.Metadata
{
    [Serializable]
    public class Property : IEquatable<Property>
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public long ParentClassId { get; set; }
        public PropertyId PropertyId { get; set; }
        public bool IsValueType { get; set; }
        public bool Equals(Property other)
        {
            return other.PropertyId.Equals(this.PropertyId);
        }
        public override bool Equals(object obj)
        {
            var prop = (obj as Property);
            if (prop == null)
            {
                return false;
            }
            return prop.PropertyId.Equals(this.PropertyId);
        }
        public override int GetHashCode()
        {
            return this.PropertyId.GetHashCode();
        }
    }
    [Serializable]
    public class PropertyId : IEquatable<PropertyId>
    {
        public string Name { get; set; }
        public long ParentClassId { get; set; }
        public long Id { get; set; }

        public bool Equals(PropertyId other)
        {
            if (other.Id != 0 && other.Id.Equals(Id))
            {
                return true;
            }
            if (other.Name != null && other.Name.Equals(Name))
            {
                return true;
            }
            if (other.ParentClassId != 0 && other.ParentClassId.Equals(ParentClassId))
            {
                return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            var other = obj as PropertyId;
            if (other == null)
            {
                return false;
            }
            if (other.Id != 0 && other.Id.Equals(Id))
            {
                return true;
            }
            if (other.Name != null && other.Name.Equals(Name))
            {
                return true;
            }
            if (other.ParentClassId != 0 && other.ParentClassId.Equals(ParentClassId))
            {
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}