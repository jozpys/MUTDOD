using System;

namespace MUTDOD.Common.ModuleBase
{
    public interface IQueryEngine : IModule
    {
        IQueryResult Execute(string dbName, IQueryTree queryTree);
    }
    public interface ICentralServerEngine : IModule, IQueryEngine
    {
        IQueryResult Execute(string dbName, IQuery query, SystemInfo systemInfo, Action<IQueryTree> doOnDataServers);
    }
}
