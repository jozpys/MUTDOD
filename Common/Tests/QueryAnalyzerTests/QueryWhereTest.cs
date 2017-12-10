﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Server.Common.EBNFQueryAnalyzer;
using MUTDOD.Common;

namespace QueryAnalyzerTests
{
    [TestClass]
    public class QueryWhereTest : AbstractQueryAnalizerTest
    {
        [TestMethod]
        public void TestIsNull()
        {
            DTOQuery inputQuery = new DTOQuery();
            IDatabaseSchema databaseSchema = CreateEmptyDatabaseSchema();

            inputQuery.QueryText = "Student where name is null;";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            IQueryTree queryTree = analizer.ParseQuery(inputQuery, databaseSchema);
            String query = PrintComponentInfo(queryTree);
            //Assert.AreEqual("STATEMENT SYSTEM_OPERATION GET_SYSTEM_INFO SEMICOLON value=; ", query);
            System.Diagnostics.Debug.WriteLine(query);
        }

        public void TestIsNotNull()
        {
            DTOQuery inputQuery = new DTOQuery();
            IDatabaseSchema databaseSchema = CreateEmptyDatabaseSchema();

            inputQuery.QueryText = "Student where name is not null;";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            IQueryTree queryTree = analizer.ParseQuery(inputQuery, databaseSchema);
            String query = PrintComponentInfo(queryTree);
            //Assert.AreEqual("STATEMENT SYSTEM_OPERATION GET_SYSTEM_INFO SEMICOLON value=; ", query);
            System.Diagnostics.Debug.WriteLine(query);
        }
    }
}
