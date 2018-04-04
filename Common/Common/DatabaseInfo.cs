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
        public List<Field> Fields { get; set; }
        public List<string> Methods { get; set; }
    }

    [Serializable]
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
