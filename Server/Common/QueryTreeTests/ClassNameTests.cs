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
    public class ClassNameTests
    {
        [TestMethod()]
        public void FindClassTest()
        {
            ClassId classId1 = new ClassId { Id = 1, Name = "Name1" };
            ClassId classId2 = new ClassId { Id = 2, Name = "Name2" };
            ClassId classId3 = new ClassId { Id = 3, Name = "Name3" };

            ConcurrentDictionary<ClassId, Class> classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(classId1, new Class { ClassId = classId1, Name = classId1.Name });
            classes.TryAdd(classId2, new Class { ClassId = classId2, Name = classId2.Name });
            classes.TryAdd(classId3, new Class { ClassId = classId3, Name = classId3.Name });
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            ClassName className = new ClassName();
            className.Name = "Name2";
            var result = className.Execute(new QueryParameters { Database = database.Object });

            Assert.AreEqual("Name2", result.QueryClass.Name);
            Assert.IsNull(result.Result);
        }

        [TestMethod()]
        public void ClassDosentExistTest()
        {
            ClassId classId1 = new ClassId { Id = 1, Name = "Name1" };
            ClassId classId2 = new ClassId { Id = 2, Name = "Name2" };
            ClassId classId3 = new ClassId { Id = 3, Name = "Name3" };

            ConcurrentDictionary<ClassId, Class> classes = new ConcurrentDictionary<ClassId, Class>();
            classes.TryAdd(classId1, new Class { ClassId = classId1, Name = classId1.Name });
            classes.TryAdd(classId2, new Class { ClassId = classId2, Name = classId2.Name });
            classes.TryAdd(classId3, new Class { ClassId = classId3, Name = classId3.Name });
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema.Classes).Returns(classes);

            ClassName className = new ClassName();
            className.Name = "Name4";
            var result = className.Execute(new QueryParameters { Database = database.Object });

            Assert.AreEqual("Name4", result.QueryClass.Name);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Unknown class: Name4", result.Result.StringOutput);
        }

        [TestMethod()]
        [ExpectedException(typeof(ApplicationException))]
        public void SchemaDosentExistsTest()
        {
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.Schema).Returns((IDatabaseSchema) null);

            ClassName className = new ClassName();
            className.Name = "Name4";
            var result = className.Execute(new QueryParameters { Database = database.Object });
        }


    }
}