using System;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Common.ModuleBase
{
    public interface IQueryAnalyzer : IModule
    {
        IQueryTree ParseQuery(IQuery inputQuery, IDatabaseSchema databaseSchema);
    }

    public abstract class QuerySyntaxException : Exception
    {
    }

    public abstract class QuerySemanticException : Exception
    {
        public abstract IEnumerable<IQuerySemanticExceptionItem> ExceptionList { get; }
        public QuerySemanticException(string message) : base(message) { }
    }

    public interface IQuerySemanticExceptionItem
    {
        string name { get; }
        string message { get; }
        int line { get; }
        int col { get; }
    }
}
