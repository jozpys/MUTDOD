using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MUTDOD.Common.Types
{
    [Serializable]
    public class Property : IEquatable<Property>
    {
        public const String FLOAT = "float";
        public const String BYTE = "byte";
        public const String SHORT = "short";
        public const String INT = "int";
        public const String LONG = "long";
        public const String DOUBLE = "double";
        public const String CHAR = "char";
        public const String STRING = "string";
        public const String BOOL = "bool";

        public static readonly ISet<String> numericTypes = ImmutableHashSet.Create(INT, SHORT, LONG, FLOAT, DOUBLE);
        public string Type { get; set; }

        public Type DotNetType
        {
            get
            {
                if (!IsValueType)
                {
                    return typeof(Guid);
                }

                switch (Type.ToLower())
                {
                    case FLOAT:
                        return typeof (double);
                    case BYTE:
                        return typeof (byte);
                    case SHORT:
                        return typeof (short);
                    case INT:
                        return typeof (int);
                    case LONG:
                        return typeof (long);
                    case DOUBLE:
                        return typeof (double);
                    case CHAR:
                        return typeof (char);
                    case STRING:
                        return typeof (string);
                    case BOOL:
                        return typeof (bool);
                    default:
                        throw new Exception("Unknown value type: " + Type);
                }

            }
        }

        public bool IsNumericType {
            get
            {
                return numericTypes.Contains(Type.ToLower());
            }
        }

        public string Name { get; set; }
        public long ParentClassId { get; set; }
        public PropertyId PropertyId { get; set; }
        public bool IsValueType { get; set; }
        public bool IsArray { get; set; }
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
