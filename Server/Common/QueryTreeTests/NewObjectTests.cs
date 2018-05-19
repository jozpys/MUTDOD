using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
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
    public class NewObjectTests
    {
        [TestMethod()]
        public void AddNewObjectTest()
        {
            var databaseId = new Did { Dli = 1, Duid = 2 };
            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 } };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 } };
            var properties = new List<Property>() { property1, property2 };
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.ClassProperties(It.Is<Class>(cl => cl.ClassId.Id == 1))).Returns(properties);
            database.Setup(d => d.DatabaseId).Returns(databaseId);

            Mock<IStorage> storage = new Mock<IStorage>();
            Oid oid = null;
            storage.Setup(s => s.Save(It.Is<Did>(d => d == databaseId), It.Is<IStorable>(st => st.Properties.Keys.All(p => properties.Contains(p))))).Callback<Did, IStorable>((did, store) => oid = store.Oid);

            Mock<Action<String, MessageLevel>> log = new Mock<Action<string, MessageLevel>>();

            NewObject newObjectStatement = new NewObject();

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
            newObjectStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            Mock<ObjectInitializationElement> element1 = new Mock<ObjectInitializationElement>();
            element1.Setup(e => e.Execute(It.Is<QueryParameters>(qp => qp == parameters))).Returns(new QueryDTO { Value = property1 });
            newObjectStatement.Add(element1.Object);
            Mock<ObjectInitializationElement> element2 = new Mock<ObjectInitializationElement>();
            element2.Setup(e => e.Execute(It.Is<QueryParameters>(qp => qp == parameters))).Returns(new QueryDTO { Value = property2 });
            newObjectStatement.Add(element2.Object);

            var result = newObjectStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("New object saved with id: " + oid, result.Result.StringOutput);
        }

        [TestMethod()]
        public void NoClassToCreateObjectTest()
        {
            String noClassLabel = "No class: Test";
            NewObject newObjectStatement = new NewObject();

            Mock<ClassName> className = new Mock<ClassName>();
            QueryDTO dto = new QueryDTO
            {
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = noClassLabel
                },
                QueryClass = new Class
                {
                    ClassId = new ClassId { Id = 1, Name = "Test" },
                    Name = "Test"
                }
            };
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(dto);
            newObjectStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters();
            var result = newObjectStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual(noClassLabel, result.Result.StringOutput);
        }

        [TestMethod()]
        public void CantCreateObjectOfInterfaceTest()
        {
            NewObject newObjectStatement = new NewObject();

            Mock<ClassName> className = new Mock<ClassName>();
            QueryDTO dto = new QueryDTO
            {
                QueryClass = new Class
                {
                    ClassId = new ClassId { Id = 1, Name = "Test" },
                    Name = "Test",
                    Interface = true
                },
            };
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(dto);
            newObjectStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters();
            var result = newObjectStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual(NewObject.cantCreateObjectLabel, result.Result.StringOutput);
        }
    }
}