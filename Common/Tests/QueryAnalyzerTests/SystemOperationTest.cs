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
    public class SystemOperationTest : AbstractQueryAnalizerTest
    {
        [TestMethod]
        public void GetSystemInfoTest()
        {
            DTOQuery inputQuery = new DTOQuery();
            IDatabaseSchema databaseSchema = CreateEmptyDatabaseSchema();

            inputQuery.QueryText = "@SystemInfo;";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            IQueryTree queryTree = analizer.ParseQuery(inputQuery, databaseSchema);
            String query = PrintComponentInfo(queryTree);
            //Assert.AreEqual("STATEMENT SYSTEM_OPERATION GET_SYSTEM_INFO PARAM value=@ SYS_INFO value=SystemInfo SEMICOLON value=; ", query);
            Assert.AreEqual("STATEMENT SYSTEM_OPERATION GET_SYSTEM_INFO SEMICOLON value=; ", query);
            System.Diagnostics.Debug.WriteLine(query);

        }
    }
}
