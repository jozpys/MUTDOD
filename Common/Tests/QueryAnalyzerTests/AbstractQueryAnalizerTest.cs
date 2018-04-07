using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAnalyzerTests
{
    public abstract class AbstractQueryAnalizerTest
    {
        internal String PrintComponentInfo(IQueryTree queryTree)
        {
            String info = String.Empty;
            info += queryTree.TokenName + " ";
            if (!String.IsNullOrEmpty(queryTree.TokenValue))
            {
                info += "value=" + queryTree.TokenValue + " ";
            }

            if (queryTree.ProductionsList != null)
            {
                foreach (var item in queryTree.ProductionsList)
                {
                    info += PrintComponentInfo(item);
                }
            }
            return info;
        }

        internal EmptyDatabaseSchema CreateEmptyDatabaseSchema()
        {
            return null;
        }

        internal class EmptyDatabaseSchema : IDatabaseSchema
        {
            public EmptyDatabaseSchema()
            {
                DatabaseId = Did.CreateNew();
                Classes = new ConcurrentDictionary<ClassId, Class>();
                Properties = new ConcurrentDictionary<PropertyId, Property>();
                Methods = new ConcurrentDictionary<ClassId, List<string>>();
            }
            public Did DatabaseId { get; set; }
            public ConcurrentDictionary<ClassId, Class> Classes { get; set; }
            public ConcurrentDictionary<PropertyId, Property> Properties { get; set; }
            public ConcurrentDictionary<ClassId, List<string>> Methods { get; set; }

            public List<Property> ClassProperties(Class className)
            {
                return Enumerable.Empty<Property>().ToList();
            }
        }
    }
}
