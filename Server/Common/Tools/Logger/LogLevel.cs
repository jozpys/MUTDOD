using System;

namespace MUTDOD.Server.Common.Tools.Logger
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Errors = 2 << 0,
        Warnings = 2 << 1,
        WarningsOrHigher = Info - 1,
        Info = 2 << 2,
        InfoOrHigher = ModuleOperation - 1,
        ModuleOperation = 2 << 3,
        ModuleOperationOrHigher = QueryExecution - 1,
        QueryExecution = 2 << 4,
        QueryExecutionOrHigher = Operations - 1,
        Operations = 2 << 5,
        OperationsOrHigher = (2 << 7) - 1,
        All = (2 << 16) - 1 
    }
}
