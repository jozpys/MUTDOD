using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.CoreModule
{
    public class ModuleManager : IModuleManager
    {
        private readonly ILogger _logger;
        public ICore Core { get; private set; }

        public ModuleManager(ILogger logger)
        {
            _logger = logger;
        }

        public void Init(ICore core)
        {
            Core = core;
        }

        public IModule RegisterModule(IModule module)
        {
            _logger.Log(Constant.Name, string.Format(LogMessage.ModuleInitializationStartedFormat, module.Name), MessageLevel.ModuleOperation);
            module.Register(Core);
            return module;
        }
        
        public static class LogMessage
        {
            public const string ModuleInitializationStartedFormat = "Module initiation has been started. Module: {0}";
        }

        private static class Constant
        {
            public const string Name = "ModuleManager";
        }
    }
}
