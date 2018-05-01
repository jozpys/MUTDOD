using System;

namespace MUTDOD.Common
{
    public interface ILogger
    {
        void Log(string senderName, string message, MessageLevel messageLevel);
    }
    [Flags]
    public enum MessageLevel
    {
        Error = 1,
        Warning = 2 << 1,
        Info = 2 << 2,
        ModuleOperation = 2 << 3,
        QueryExecution = 2 << 4,
        Operations = 2 << 5,
        QueryPlan = 2 << 6,
    }
}
