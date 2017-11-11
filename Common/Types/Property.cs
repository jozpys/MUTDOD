using System;

namespace MUTDOD.Common.Types
{
    [Serializable]
    public class Property : IEquatable<Property>
    {
        public string Type { get; set; }

        public Type DotNetType
        {
            get
            {
                switch (Type.ToLower())
                {
                    case "float":
                        return typeof (double);
                    case "byte":
                        return typeof (byte);
                    case "short":
                        return typeof (short);
                    case "int":
                        return typeof (int);
                    case "long":
                        return typeof (long);
                    case "double":
                        return typeof (double);
                    case "char":
                        return typeof (char);
                    case "string":
                        return typeof (string);
                    case "bool":
                        return typeof (bool);
                    case "guid":
                        return typeof (Guid);
                    default:
                        throw new Exception("Unknown value type: " + Type);
                }

            }
        }

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
