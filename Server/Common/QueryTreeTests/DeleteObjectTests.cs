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
    public class DeleteObjectTests
    {
        [TestMethod()]
        public void DeleteObjectTest()
        {
            Mock<IStorable> toDelete = new Mock<IStorable>();
            Oid oid = new Oid(Guid.NewGuid(), 1);
            toDelete.Setup(s => s.Oid).Returns(oid);

            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.DatabaseId).Returns(new Did());

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.Delete(It.IsAny<Did>(), It.Is<IEnumerable<IStorable>>(st => st.Contains(toDelete.Object)))).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<string, MessageLevel>>();

            DeleteObject deleteObjectStatement = new DeleteObject();
            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };

            Mock<SelectStatement> selectStatement = new Mock<SelectStatement>();
            selectStatement.Setup(select => select.Execute(It.Is<QueryParameters>(qp => parameters == qp)))
                .Returns(new QueryDTO
                {
                    QueryObjects = new List<IStorable>{ toDelete.Object },
                    Result = new DTOQueryResult { QueryResultType = ResultType.ReferencesOnly}
                });
            deleteObjectStatement.Add(selectStatement.Object);

            var result = deleteObjectStatement.Execute(parameters);

            storage.Verify();
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Objects deleted with ids: " + oid, result.Result.StringOutput);
        }

        [TestMethod()]
        public void DeleteEmptyObjectListTest()
        {
            Mock<IDatabaseParameters> database = new Mock<IDatabaseParameters>();
            database.Setup(d => d.DatabaseId).Returns(new Did());

            Mock<IStorage> storage = new Mock<IStorage>();
            storage.Setup(s => s.Delete(It.IsAny<Did>(), It.IsAny<IEnumerable<IStorable>>())).Verifiable();

            Mock<Action<String, MessageLevel>> log = new Mock<Action<string, MessageLevel>>();

            DeleteObject deleteObjectStatement = new DeleteObject();
            QueryParameters parameters = new QueryParameters { Database = database.Object, Storage = storage.Object, Log = log.Object };

            Mock<SelectStatement> selectStatement = new Mock<SelectStatement>();
            selectStatement.Setup(select => select.Execute(It.Is<QueryParameters>(qp => parameters == qp)))
                .Returns(new QueryDTO
                {
                    QueryObjects = new List<IStorable>(),
                    Result = new DTOQueryResult { QueryResultType = ResultType.ReferencesOnly }
                });
            deleteObjectStatement.Add(selectStatement.Object);

            var result = deleteObjectStatement.Execute(parameters);

            storage.Verify();
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual("Objects deleted with ids: ", result.Result.StringOutput);
        }

        [TestMethod()]
        public void SelectErrorWhileDeletingObjectTest()
        {
            String statementErrorMesage = "errorMesage";

            Mock<IStorage> storage = new Mock<IStorage>();

            DeleteObject deleteObjectStatement = new DeleteObject();
            QueryParameters parameters = new QueryParameters();

            Mock<SelectStatement> selectStatement = new Mock<SelectStatement>();
            selectStatement.Setup(select => select.Execute(It.Is<QueryParameters>(qp => parameters == qp)))
                .Returns(new QueryDTO
                {
                    Result = new DTOQueryResult
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = statementErrorMesage
                    }
                });
            deleteObjectStatement.Add(selectStatement.Object);

            var result = deleteObjectStatement.Execute(parameters);

            storage.Verify(s => s.Delete(It.IsAny<Did>(), It.IsAny<IEnumerable<IStorable>>()), Times.Never);
            Assert.AreEqual(ResultType.StringResult, result.Result.QueryResultType);
            Assert.AreEqual(statementErrorMesage, result.Result.StringOutput);
        }
    }
}