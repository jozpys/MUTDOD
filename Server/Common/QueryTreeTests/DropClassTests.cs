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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.QueryTree.Tests
{
    [TestClass()]
    public class DropClassTests
    {
        [TestMethod()]
        public void RemoveClassTest()
        {
            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 } };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 } };
            Property property3 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 3 } };
            var properties = new ConcurrentDictionary<PropertyId, Property>();
            properties.TryAdd(property1.PropertyId, property1);
            properties.TryAdd(property2.PropertyId, property2);
            properties.TryAdd(property3.PropertyId, property3);


            var classToDrop = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var otherClass = new Class
            {
                ClassId = new ClassId { Id = 2, Name = "Test2" },
                Name = "Test2"
            };

            var classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(classToDrop.ClassId, classToDrop);
            classes.TryAdd(otherClass.ClassId, otherClass);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);
            var classProperties = new List<Property>() { property1, property2 };
            database.Setup(d => d.Schema.ClassProperties(It.Is<Class>(cl => cl.ClassId.Id == 1))).Returns(classProperties);
            database.Setup(d => d.Schema.Properties).Returns(properties);

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            DropClass dropClassStatement = new DropClass();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO { QueryClass = classToDrop});
            dropClassStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            var result = dropClassStatement.Execute(parameters);

            storage.Verify();
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Class:" + classToDrop.Name + " droped.", result.Result.StringOutput);
        }

        [TestMethod()]
        public void NoDatabaseSchemaTest()
        {
            var classToDrop = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            DropClass dropClassStatement = new DropClass();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO { QueryClass = classToDrop });
            dropClassStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Storage = storage.Object, Log = log.Object };
            var result = dropClassStatement.Execute(parameters);

            storage.Verify(s => s.SaveSchema(It.IsAny<IDatabaseSchema>()), Times.Never);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Error ocured while class droping", result.Result.StringOutput);
        }

        [TestMethod()]
        public void ClassDosentExistsTest()
        {
            var classToDrop = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var otherClass = new Class
            {
                ClassId = new ClassId { Id = 2, Name = "Test2" },
                Name = "Test2"
            };

            var classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(otherClass.ClassId, otherClass);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            DropClass dropClassStatement = new DropClass();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO
            {
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No class"
                },
                QueryClass = classToDrop
            });
            dropClassStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            var result = dropClassStatement.Execute(parameters);

            storage.Verify(s => s.SaveSchema(It.IsAny<IDatabaseSchema>()), Times.Never);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("No class with name: "+ classToDrop.Name, result.Result.StringOutput);
        }

        [TestMethod()]
        public void TryRemoveInterfaceTest()
        {
            var classToDrop = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1",
                Interface = true
            };

            var otherClass = new Class
            {
                ClassId = new ClassId { Id = 2, Name = "Test2" },
                Name = "Test2"
            };

            var classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(otherClass.ClassId, otherClass);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            DropClass dropClassStatement = new DropClass();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO
            {
                QueryClass = classToDrop
            });
            dropClassStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            var result = dropClassStatement.Execute(parameters);

            storage.Verify(s => s.SaveSchema(It.IsAny<IDatabaseSchema>()), Times.Never);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("No class with name: " + classToDrop.Name, result.Result.StringOutput);
        }
    }
}