using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.QueryTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MUTDOD.Server.Common.QueryTree.Tests
{
    [TestClass()]
    public class DerefTests
    {
        [TestMethod()]
        public void DerefTest()
        {
            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 }, Name = "ID" };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 }, Name = "Name" };
            Mock<IStorable> storable1 = new Mock<IStorable>();
            Oid oid1 = new Oid(Guid.NewGuid(), 1);
            storable1.Setup(s => s.Oid).Returns(oid1);
            storable1.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property1, 1 }, { property2, "Name1" } });

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            Mock<IStorage> storage = new Mock<IStorage>();

            Deref deref = new Deref();

            Mock<SelectStatement> selectStatement = new Mock<SelectStatement>();

            selectStatement.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => qp.Database == database.Object && qp.Storage == storage.Object))).Returns(
                new QueryDTO()
                {
                    QueryClass = new Class
                    {
                        ClassId = new ClassId { Id = 1, Name = "Test" },
                        Name = "Test"
                    },
                    QueryObjects = new List<IStorable>() { storable1.Object }

                });
            deref.Add(selectStatement.Object);

            QueryDTO result = deref.Execute(new QueryParameters { Database = database.Object, Storage = storage.Object });
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result.Result.StringOutput);

            XmlNode resultNode = doc.GetElementsByTagName("result").Item(0);
            XmlNodeList nodes = resultNode.ChildNodes;
            XmlNode storableNode = nodes[0];
            Assert.AreEqual("Test", storableNode.Name);

            XmlNodeList storableNodes = storableNode.ChildNodes;

            XmlNode oidNode = storableNodes[0];
            Assert.AreEqual("Oid", oidNode.Name);
            Assert.AreEqual(oid1.Id.ToString(), oidNode.FirstChild.Value);

            XmlNode IdNode = storableNodes[1];
            Assert.AreEqual("ID", IdNode.Name);
            Assert.AreEqual(1, Int32.Parse(IdNode.FirstChild.Value));

            XmlNode NameNode = storableNodes[2];
            Assert.AreEqual("Name", NameNode.Name);
            Assert.AreEqual("Name1", NameNode.FirstChild.Value);
        }

        [TestMethod()]
        public void DerefEmptyTest()
        {
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            Mock<IStorage> storage = new Mock<IStorage>();

            Deref deref = new Deref();

            Mock<SelectStatement> selectStatement = new Mock<SelectStatement>();

            selectStatement.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => qp.Database == database.Object && qp.Storage == storage.Object))).Returns(
                new QueryDTO()
                {
                    QueryClass = new Class
                    {
                        ClassId = new ClassId { Id = 1, Name = "Test" },
                        Name = "Test"
                    },
                    QueryObjects = new List<IStorable>()

                });
            deref.Add(selectStatement.Object);

            QueryDTO result = deref.Execute(new QueryParameters { Database = database.Object, Storage = storage.Object });
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result.Result.StringOutput);

            XmlNode resultNode = doc.GetElementsByTagName("result").Item(0);
            XmlNodeList nodes = resultNode.ChildNodes;
            Assert.AreEqual(0, nodes.Count);
        }
    }
}