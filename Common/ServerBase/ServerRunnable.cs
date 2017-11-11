using System;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Common.ServerBase
{
    public abstract class ServerRunnable : IServerRunnable
    {
        private readonly ILogger _logger;
        protected ICore Core { get; private set; }

        public abstract string Name { get; set; }

        public abstract string Adress { get; }

        public abstract short Port { get; set; }

        public string FullAdress { get { return String.Concat(Adress, ":", Port.ToString()); } }

        public ServerRunnable(ICore core, ILogger logger)
        {
            _logger = logger;
            Core = core;
            Init();
        }

        public void Restart()
        {
            Stop();
            Init();
            Run();
        }

        public void Run()
        {
            _logger.Log(Name, ServerLogMessages.ServerStarting, MessageLevel.ModuleOperation);

            RunMain();

            _logger.Log(Name, ServerLogMessages.ServerStared, MessageLevel.Info);
        }

        public void Stop()
        {
            _logger.Log(Name, ServerLogMessages.ServerStopping, MessageLevel.ModuleOperation);

            StopMain();

            _logger.Log(Name, ServerLogMessages.ServerStopped, MessageLevel.Info);
        }

        protected void Init()
        {
            _logger.Log(Name, ServerLogMessages.ServerInitializationStarted, MessageLevel.ModuleOperation);

            InitMain();

            _logger.Log(Name, ServerLogMessages.ServerInitializationSuccessful, MessageLevel.Info);
        }

        protected virtual void InitMain()
        {
        }

        protected virtual void RunMain()
        {
        }

        protected virtual void StopMain()
        {
        }

        private static class ServerLogMessages
        {
            public const string ServerStarting = "Server is starting";
            public const string ServerStared = "Server has been started";
            public const string ServerStopping = "Server is being stoped";
            public const string ServerStopped = "Server has been stoped";
            public const string ServerInitializationStarted = "Server initialization has been started";
            public const string ServerInitializationSuccessful = "Server initialization has been completed successfully";
            public const string ServerInitializationFailed = "Server initialization has failed";
        }


    }
}
