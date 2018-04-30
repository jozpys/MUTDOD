using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUTDOD.Server.Common.EBNFQueryAnalyzer;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.Communication;
using MUTDOD.Server.Common.QueryTree;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.DummyQueryOptimizer;
using System.Collections.Generic;
using MUTDOD.Common;

namespace QueryOptimizerTest
{
    [TestClass]
    public class UnitTest1 : ILogger
    {
        [TestMethod]
        public void TestMethod1()
        {
            QueryOptimizer queryOptimizer = new QueryOptimizer();
            DTOQuery inputQuery = new DTOQuery();
            inputQuery.QueryText = "Dupa where huj == 'test';";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            var queryTree = analizer.ParseQuery(inputQuery);
            QueryPlanTree queryPlanTree = QueryOptimizer.BuildTreeSummary(queryTree);
            QueryPlan queryPlan = queryOptimizer.convertToQueryPlan(queryPlanTree);
            Console.WriteLine(queryTree.ElementType);
            // queryTree.GetComposite().elements[ElementType.SELECT];
            SelectStatement query_elements = (SelectStatement)analizer.ParseQuery(inputQuery);
            //queryTree.GetComposite().GetElement(ElementType.WHERE).GetComposite().GetElement(ElementType.COMPERISION).GetComposite().GetElement(ElementType.LEFT_OPERAND).GetComposite().GetElement(ElementType.CLASS_PROPERTY);
            var elements = query_elements.GetElements();
         //   var test = QueryOptimizer.findElementType(queryTree, ElementType.CLASS_PROPERTY);
            ClassProperty classProperty = new ClassProperty();
            Console.WriteLine(elements[ElementType.CLASS_NAME].ToString());
            SelectStatement s = new SelectStatement();
        }

        [TestMethod]
        public void TestIndex() {

            MUTDOD.Server.Common.IndexMechanism.IndexMechanism im = new MUTDOD.Server.Common.IndexMechanism.IndexMechanism(this);
            Console.WriteLine("TODO");
        }

        public void Log(string senderName, string message, MessageLevel messageLevel)
        {
            Console.WriteLine("TODO");
        }
    }
}
