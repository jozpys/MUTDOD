using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Server.Common.EBNFQueryAnalyzer;

namespace QueryAnalyzerTests
{
    [TestClass]
    public class SystemOperationTest
    {
        [TestMethod]
        public void GetSystemInfoTest()
        {
            DTOQuery inputQuery = new DTOQuery();
            IDatabaseSchema databaseSchema = new EmptyDatabaseSchema();

            inputQuery.QueryText = "@SystemInfo;";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            IQueryTree queryTree = analizer.ParseQuery(inputQuery, databaseSchema);
            String query = printComponentInfo(queryTree);
            //Assert.AreEqual("STATEMENT SYSTEM_OPERATION GET_SYSTEM_INFO PARAM value=@ SYS_INFO value=SystemInfo SEMICOLON value=; ", query);
            Assert.AreEqual("STATEMENT SYSTEM_OPERATION GET_SYSTEM_INFO SEMICOLON value=; ", query);
            System.Diagnostics.Debug.WriteLine(query);

        }

        private String printComponentInfo(IQueryTree queryTree)
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
                    info += printComponentInfo(item);
                }
            }
            return info;
        }

        private class EmptyDatabaseSchema : IDatabaseSchema
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
        }
    }
}
