using System;
using System.Collections.Generic;

namespace MUTDOD.Common
{
    [Serializable]
    public class DatabaseInfo
    {
        public string Name { get; set; }
        public List<DatabaseClass> Classes { get; set; }

        public override string ToString()
        {
            return String.Format("[DB: {0}]", Name);
        }
    }

    [Serializable]
    public class DatabaseClass
    {
        public string Name { get; set; }
        public bool Interface { get; set; }
        public List<string> ParentClasses { get; set; }
        public List<Field> Fields { get; set; }
        public List<ClassMethod> Methods { get; set; }
    }

    [Serializable]
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Reference { get; set; }
        public bool IsArray { get; set; }
    }

    [Serializable]
    public class ClassMethod
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<ClassMethodParam> Params { get; set; }

    }

    [Serializable]
    public class ClassMethodParam
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
