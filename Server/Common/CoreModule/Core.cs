using System;
using System.Collections.Generic;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.CoreModule
{
    public class Core : ICore
    {
        private readonly ILogger _logger;

        public Core(IModuleManager moduleManager, IStorage storage, IQueryEngine queryEngine, IOdbc odbc, IQueryOptimizer queryOptimizer, IIndexMechanism<string> indexMechanism, ILogger logger)
        {
            _logger = logger;
            IndexMechanism = indexMechanism;
            QueryOptimizer = queryOptimizer;
            ModuleManager = moduleManager;
            Storage = storage;
            QueryEngine = queryEngine;
            ODBC = odbc;
        }
        public string Name
        {
            get { return Constant.Name; }
        }

        public IIndexMechanism<string> IndexMechanism { get; private set; }
        public IStorage Storage { get; private set; }

        public IQueryEngine QueryEngine { get; private set; }

        public IOdbc ODBC { get; private set; }
        public IQueryOptimizer QueryOptimizer { get; private set; }

        public IModuleManager ModuleManager { get; protected set; }

        public void Init(IDictionary<string, object> parameters)
        {
            _logger.Log(Name, LogMessage.CoreInitializationStarted, MessageLevel.ModuleOperation);

            ModuleManager.Init(this);

            ModuleManager.RegisterModule(Storage);

            ModuleManager.RegisterModule(QueryOptimizer);
            
            ModuleManager.RegisterModule(QueryEngine);

            ModuleManager.RegisterModule(ODBC);

            ModuleManager.RegisterModule(IndexMechanism);

            _logger.Log(Name, LogMessage.CoreInitializationSuccessful, MessageLevel.ModuleOperation);
        }

        public void Run()
        {
            _logger.Log(Name, LogMessage.CoreRunning, MessageLevel.ModuleOperation);
        }

        public void Stop()
        {
            _logger.Log(Name, LogMessage.CoreStopping, MessageLevel.ModuleOperation);

            _logger.Log(Name, LogMessage.CoreStopped, MessageLevel.ModuleOperation);
        }

        public IModule GetModule(IModule module)
        {
            throw new NotImplementedException();
        }

        private static class Constant
        {
            public const string Name = "Core";
        }

        private static class LogMessage
        {
            public const string CoreInitializationStarted = "Core initiation has been started";
            public const string CoreInitializationSuccessful = "Core initiation successful";
            public const string CoreRunning = "Core is running";
            public const string CoreStopped = "Core has been stopped";
            public const string CoreStopping = "Core is being stopped";
        }
    }
}