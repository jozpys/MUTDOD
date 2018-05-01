using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUTDOD.Server.Common.EBNFQueryAnalyzer;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.Communication;
using MUTDOD.Server.Common.QueryTree;

namespace QueryOptimizerTest
{
    [TestClass]
    public class QueryElementView
    {
        [TestMethod]
        public void TestQueryElementView()
        {
            DTOQuery inputQuery = new DTOQuery();
            inputQuery.QueryText = "Student WHERE name IS NULL;";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            var queryTree = analizer.ParseQuery(inputQuery);
            Console.WriteLine(queryTree.ToString());
            //SelectStatement query_elements = queryTree.GetComposite();
            SelectStatement s = new SelectStatement();
            Console.WriteLine("DUPA");
            
        }
    }
}
