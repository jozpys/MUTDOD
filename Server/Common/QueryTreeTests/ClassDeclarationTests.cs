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
    public class ClassDeclarationTests
    {
        [TestMethod()]
        public void SimpleClassDeclarationTest()
        {
            String newClassName = "newClass";
            int newClassId = 3;

            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 } };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 } };
            Property property3 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 3 } };
            var properties = new ConcurrentDictionary<PropertyId, Property>();
            properties.TryAdd(property1.PropertyId, property1);
            properties.TryAdd(property2.PropertyId, property2);
            properties.TryAdd(property3.PropertyId, property3);


            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var otherExistingClass = new Class
            {
                ClassId = new ClassId { Id = 2, Name = "Test2" },
                Name = "Test2"
            };

            var classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(existingClass.ClassId, existingClass);
            classes.TryAdd(otherExistingClass.ClassId, otherExistingClass);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            Mock<IStorage> storage = new Mock<IStorage>();
            Class createdClass = null;
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Callback<IDatabaseSchema>(
                schema => createdClass = schema.Classes.Values.Where(storable => storable.ClassId.Id == newClassId).SingleOrDefault());

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            ClassDeclaration classDeclarationStatement = new ClassDeclaration();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO
            {
                QueryClass = new Class
                {
                    Name = newClassName
                },
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No class"
                }
            });
            classDeclarationStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            var result = classDeclarationStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("New class: " + newClassName + " created.", result.Result.StringOutput);

            Assert.AreEqual(newClassId, createdClass.ClassId.Id);
            Assert.AreEqual(newClassName, createdClass.Name);
        }

        [TestMethod()]
        public void ClassWithPropertyDeclarationTest()
        {
            String newClassName = "newClass";
            int newClassId = 3;
            Property testProperty = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 4 } };

            Property property1 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 1 } };
            Property property2 = new Property { Type = Property.STRING, PropertyId = new PropertyId { Id = 2 } };
            Property property3 = new Property { Type = Property.INT, PropertyId = new PropertyId { Id = 3 } };
            var properties = new ConcurrentDictionary<PropertyId, Property>();
            properties.TryAdd(property1.PropertyId, property1);
            properties.TryAdd(property2.PropertyId, property2);
            properties.TryAdd(property3.PropertyId, property3);


            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var otherExistingClass = new Class
            {
                ClassId = new ClassId { Id = 2, Name = "Test2" },
                Name = "Test2"
            };

            var classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(existingClass.ClassId, existingClass);
            classes.TryAdd(otherExistingClass.ClassId, otherExistingClass);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            Mock<IStorage> storage = new Mock<IStorage>();
            Class createdClass = null;
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Callback<IDatabaseSchema>(
                schema => createdClass = schema.Classes.Values.Where(storable => storable.ClassId.Id == newClassId).SingleOrDefault());

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            ClassDeclaration classDeclarationStatement = new ClassDeclaration();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO
            {
                QueryClass = new Class
                {
                    Name = newClassName
                },
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No class"
                }
            });
            classDeclarationStatement.Add(className.Object);

            Mock<AttributeDeclaration> attributeDeclaration = new Mock<AttributeDeclaration>();
            attributeDeclaration.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Callback<QueryParameters>(qp => testProperty.ParentClassId = qp.Subquery.QueryClass.ClassId.Id)
                .Returns(new QueryDTO { Value = testProperty });
            classDeclarationStatement.Add(attributeDeclaration.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            var result = classDeclarationStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("New class: " + newClassName + " created.", result.Result.StringOutput);

            Assert.AreEqual(newClassId, createdClass.ClassId.Id);
            Assert.AreEqual(newClassName, createdClass.Name);
            Assert.AreEqual(newClassId, testProperty.ParentClassId);
        }

        [TestMethod()]
        public void NoDatabaseSchemaWhileClassDeclarationTest()
        {
            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            ClassDeclaration classDeclarationStatement = new ClassDeclaration();

            QueryParameters parameters = new QueryParameters { Storage = storage.Object, Log = log.Object };
            var result = classDeclarationStatement.Execute(parameters);

            storage.Verify(s => s.SaveSchema(It.IsAny<IDatabaseSchema>()), Times.Never);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Error ocured while class creation", result.Result.StringOutput);
        }

        [TestMethod()]
        public void ClassAlreadyExistsTest()
        {
            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(existingClass.ClassId, existingClass);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.SaveSchema(It.IsAny<IDatabaseSchema>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<String, MessageLevel>>();

            ClassDeclaration classDeclarationStatement = new ClassDeclaration();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.IsAny<QueryParameters>())).Returns(new QueryDTO { QueryClass = existingClass });
            classDeclarationStatement.Add(className.Object);

            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };
            var result = classDeclarationStatement.Execute(parameters);

            storage.Verify(s => s.SaveSchema(It.IsAny<IDatabaseSchema>()), Times.Never);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Class or interface with name: " + existingClass.Name + " arleady exists!", result.Result.StringOutput);
        }
    }
}