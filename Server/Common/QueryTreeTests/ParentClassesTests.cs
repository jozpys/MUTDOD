using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
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
    public class ParentClassesTests
    {
        [TestMethod()]
        public void ClassInheritsOneClassTest()
        {
            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            QueryParameters parameters = new QueryParameters();

            ParentClasses parentClassesStatement = new ParentClasses();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingClass });
            parentClassesStatement.Add(className.Object);
            
            QueryDTO result = parentClassesStatement.Execute(parameters);
            ISet<Class> parentClasses = result.Value;

            Assert.AreEqual(1, parentClasses.Count);
            Assert.IsTrue(parentClasses.Contains(existingClass));
        }

        [TestMethod()]
        public void ClassInheritsOneInterfaceTest()
        {
            var existingInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1",
                Interface = true
            };

            QueryParameters parameters = new QueryParameters();

            ParentClasses parentClassesStatement = new ParentClasses();

            Mock<ClassName> className = new Mock<ClassName>();
            className.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingInterface });
            parentClassesStatement.Add(className.Object);

            QueryDTO result = parentClassesStatement.Execute(parameters);
            ISet<Class> parentClasses = result.Value;

            Assert.AreEqual(1, parentClasses.Count);
            Assert.IsTrue(parentClasses.Contains(existingInterface));
        }

        [TestMethod()]
        public void ClassInheritsOneClassAndTwoInterfacesTest()
        {
            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var existingInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test2" },
                Name = "Test2",
                Interface = true
            };

            var existingOtherInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test3" },
                Name = "Test3",
                Interface = true
            };

            QueryParameters parameters = new QueryParameters();

            ParentClasses parentClassesStatement = new ParentClasses();

            Mock<ClassName> className1 = new Mock<ClassName>();
            className1.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingClass });
            parentClassesStatement.Add(className1.Object);

            Mock<ClassName> className2 = new Mock<ClassName>();
            className2.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingInterface });
            parentClassesStatement.Add(className2.Object);

            Mock<ClassName> className3 = new Mock<ClassName>();
            className3.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingOtherInterface });
            parentClassesStatement.Add(className3.Object);

            QueryDTO result = parentClassesStatement.Execute(parameters);
            ISet<Class> parentClasses = result.Value;

            Assert.AreEqual(3, parentClasses.Count);
            Assert.IsTrue(parentClasses.Contains(existingClass));
            Assert.IsTrue(parentClasses.Contains(existingInterface));
            Assert.IsTrue(parentClasses.Contains(existingOtherInterface));
        }

        [TestMethod()]
        public void ClassInheritsTwoClassTest()
        {
            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var otherExistingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test2" },
                Name = "Test2"
            };

            QueryParameters parameters = new QueryParameters();

            ParentClasses parentClassesStatement = new ParentClasses();

            Mock<ClassName> className1 = new Mock<ClassName>();
            className1.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingClass });
            parentClassesStatement.Add(className1.Object);

            Mock<ClassName> className2 = new Mock<ClassName>();
            className2.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = otherExistingClass });
            parentClassesStatement.Add(className2.Object);

            QueryDTO result = parentClassesStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("A class can't inherit from more than 1 class!", result.Result.StringOutput);
        }

        [TestMethod()]
        public void InterfaceInheritsTwoInterfaceTest()
        {
            var existingInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1",
                Interface = true
            };

            var otherExistingInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test2" },
                Name = "Test2",
                Interface = true
            };

            QueryParameters parameters = new QueryParameters();

            ParentClasses parentClassesStatement = new ParentClasses { InterfaceOnly = true };

            Mock<ClassName> className1 = new Mock<ClassName>();
            className1.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingInterface });
            parentClassesStatement.Add(className1.Object);

            Mock<ClassName> className2 = new Mock<ClassName>();
            className2.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = otherExistingInterface });
            parentClassesStatement.Add(className2.Object);

            QueryDTO result = parentClassesStatement.Execute(parameters);
            ISet<Class> parentClasses = result.Value;

            Assert.AreEqual(2, parentClasses.Count);
            Assert.IsTrue(parentClasses.Contains(existingInterface));
            Assert.IsTrue(parentClasses.Contains(otherExistingInterface));
        }

        [TestMethod()]
        public void InterfaceInheritsOneClassAndTwoInterfacesTest()
        {
            var existingClass = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test1" },
                Name = "Test1"
            };

            var existingInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test2" },
                Name = "Test2",
                Interface = true
            };

            var existingOtherInterface = new Class
            {
                ClassId = new ClassId { Id = 1, Name = "Test3" },
                Name = "Test3",
                Interface = true
            };

            QueryParameters parameters = new QueryParameters();

            ParentClasses parentClassesStatement = new ParentClasses { InterfaceOnly = true };

            Mock<ClassName> className1 = new Mock<ClassName>();
            className1.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingClass });
            parentClassesStatement.Add(className1.Object);

            Mock<ClassName> className2 = new Mock<ClassName>();
            className2.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingInterface });
            parentClassesStatement.Add(className2.Object);

            Mock<ClassName> className3 = new Mock<ClassName>();
            className3.Setup(cn => cn.Execute(It.Is<QueryParameters>(qp => parameters.Equals(qp)))).Returns(new QueryDTO { QueryClass = existingOtherInterface });
            parentClassesStatement.Add(className3.Object);

            QueryDTO result = parentClassesStatement.Execute(parameters);

            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("An interface can't inherit from a class!", result.Result.StringOutput);
        }
    }
}