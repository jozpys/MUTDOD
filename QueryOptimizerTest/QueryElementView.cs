using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUTDOD.Server.Common.EBNFQueryAnalyzer;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.Communication;
using MUTDOD.Server.Common.QueryTree;
using MUTDOD.Common.ModuleBase.Communication;
using System.Collections.Generic;
using MUTDOD.Server.Common.QueryOptimizer;
using MUTDOD.Server.Common.IndexMechanism;
using MUTDOD.Common.Types;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Settings;
using static MTDOD.CentralServerApp.Logger;

namespace QueryOptimizerTest
{
    [TestClass]
    public class QueryElementView
    {
        [TestMethod]
        public void TestQueryElementView()
        {
            DTOQuery inputQuery = new DTOQuery();
            inputQuery.QueryText = "Student where na==2 and bc==3;";
            IQueryAnalyzer analizer = new EBNFQueryAnalyzer();
            var queryTree = analizer.ParseQuery(inputQuery);
            QueryPlanTree queryPlanTree = QueryOptimizer.BuildTreeSummary(queryTree, new QueryParameters());

           // queryTree.GetComposite().elements[ElementType.SELECT];
            SelectStatement query_elements = (SelectStatement)analizer.ParseQuery(inputQuery);
          //  queryTree.GetComposite().GetElement(ElementType.WHERE).GetComposite().GetElement(ElementType.COMPERISION).GetComposite().GetElement(ElementType.LEFT_OPERAND).GetComposite().GetElement(ElementType.CLASS_PROPERTY);
            var elements=query_elements.GetElements();
            //var test = QueryOptimizer.findElementType(queryTree, ElementType.CLASS_PROPERTY);
            ClassProperty classProperty = new ClassProperty();
            Console.WriteLine(elements[ElementType.CLASS_NAME].ToString());
            SelectStatement s = new SelectStatement();
            
        }

        /*[TestMethod]
        public void test()
        {
            ServerSchemaStats serverSchemaStats = new ServerSchemaStats();
            Did d = Did.CreateNew();
            SchemaStats s = new SchemaStats();
            s.DatabaseId = 123465;
            PropertyId propertyId = new PropertyId();
            propertyId.Id = 12;
            propertyId.Name = "Property";
            propertyId.ParentClassId = 123456;
            s.PropertyStats.ValueHist.TryAdd(1, propertyId);
            ClassId classId = new ClassId();
            classId.Id = 14;
            classId.Name = "Class";
            s.ClassStats.objectNumbers.TryAdd(classId, 15);

            serverSchemaStats.UpdateStatisticSchemaData(d, s);

        }*/
        [TestMethod]
        public void testIndex()
        {

            var im = new MUTDOD.Server.Common.IndexMechanism.IndexMechanism<Type>(new MTDOD.CentralServerApp.Logger(new HardcodedSettings(), (l) => l.ToString()));

        }
    }
}
