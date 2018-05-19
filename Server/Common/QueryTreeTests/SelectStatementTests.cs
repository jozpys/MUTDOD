using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MUTDOD.Common;
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

namespace MUTDOD.Server.Common.QueryTree.Tests
{
    [TestClass()]
    public class SelectStatementTests
    {
        [TestMethod()]
        public void FindAllTest()
        {

            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 } };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 } };
            Property property3 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 3 } };
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.ClassProperties(It.Is<Class>(cl => cl.ClassId.Id == 1))).Returns(new List<Property>(){ property1, property2});

            Mock<IStorage> storage = new Mock<IStorage>();
            Mock<IStorable> storable1 = new Mock<IStorable>();
            Oid oid1 = new Oid(Guid.NewGuid(), 1);
            storable1.Setup(s => s.Oid).Returns(oid1);
            storable1.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property1, 1 }, { property2, "Name1" } });
            Mock<IStorable> storable2 = new Mock<IStorable>();
            Oid oid2 = new Oid(Guid.NewGuid(), 2);
            storable2.Setup(s => s.Oid).Returns(oid2);
            storable2.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property1, 2 }, { property2, "Name2" } });
            Mock<IStorable> storable3 = new Mock<IStorable>();
            Oid oid3 = new Oid(Guid.NewGuid(), 3);
            storable3.Setup(s => s.Oid).Returns(oid3);
            storable3.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property3, 12 } });
            storage.Setup(s => s.GetAll(It.IsAny<Did>())).Returns(new List<IStorable>() { storable1.Object, storable2.Object, storable3.Object });
            
            SelectStatement statement = new SelectStatement();
            
            Mock<ClassName> className = new Mock<ClassName>();
            QueryDTO dto = new QueryDTO
            {
                QueryClass = new Class {
                    ClassId = new ClassId { Id = 1, Name = "Test"},
                    Name = "Test" }
            };
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(dto);
            statement.Add(className.Object);

            QueryDTO result = statement.Execute(new QueryParameters { Database = database.Object, Storage = storage.Object });

            Assert.AreEqual(result.Result.QueryResultType, ResultType.ReferencesOnly);
            Assert.AreEqual(result.Result.QueryResults.Count, 2);
            Assert.IsTrue(result.Result.QueryResults.Contains(oid1));
            Assert.IsTrue(result.Result.QueryResults.Contains(oid2));
            Assert.IsFalse(result.Result.QueryResults.Contains(oid3));

        }

        [TestMethod]
        public void FilterTest()
        {

            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 } };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 } };
            Property property3 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 3 } };
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.ClassProperties(It.Is<Class>(cl => cl.ClassId.Id == 1))).Returns(new List<Property>() { property1, property2 });

            Mock<IStorage> storage = new Mock<IStorage>();
            Mock<IStorable> storable1 = new Mock<IStorable>();
            Oid oid1 = new Oid(Guid.NewGuid(), 1);
            storable1.Setup(s => s.Oid).Returns(oid1);
            storable1.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property1, 1 }, { property2, "Name1" } });
            Mock<IStorable> storable2 = new Mock<IStorable>();
            Oid oid2 = new Oid(Guid.NewGuid(), 2);
            storable2.Setup(s => s.Oid).Returns(oid2);
            storable2.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property1, 2 }, { property2, "Name2" } });
            Mock<IStorable> storable3 = new Mock<IStorable>();
            Oid oid3 = new Oid(Guid.NewGuid(), 3);
            storable3.Setup(s => s.Oid).Returns(oid3);
            storable3.Setup(s => s.Properties).Returns(new Dictionary<Property, object>() { { property3, 12 } });
            storage.Setup(s => s.GetAll(It.IsAny<Did>())).Returns(new List<IStorable>() { storable1.Object, storable2.Object, storable3.Object });

            SelectStatement statement = new SelectStatement();

            Mock<ClassName> className = new Mock<ClassName>();
            QueryDTO dto = new QueryDTO
            {
                QueryClass = new Class
                {
                    ClassId = new ClassId { Id = 1, Name = "Test" },
                    Name = "Test"
                }
            };
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(dto);
            statement.Add(className.Object);

            Mock<WhereStatement> where = new Mock<WhereStatement>();
            where.Setup(w => w.Execute(It.IsAny<QueryParameters>())).Returns<QueryParameters>(
                qp => new QueryDTO {
                    QueryClass = qp.Subquery.QueryClass,
                    QueryObjects = qp.Subquery.QueryObjects.Where(s => s.Oid == oid1)
                });
            statement.Add(where.Object);

            QueryDTO result = statement.Execute(new QueryParameters { Database = database.Object, Storage = storage.Object });

            Assert.AreEqual(result.Result.QueryResultType, ResultType.ReferencesOnly);
            Assert.AreEqual(result.Result.QueryResults.Count, 1);
            Assert.IsTrue(result.Result.QueryResults.Contains(oid1));
            Assert.IsFalse(result.Result.QueryResults.Contains(oid2));
            Assert.IsFalse(result.Result.QueryResults.Contains(oid3));

        }
    }
}