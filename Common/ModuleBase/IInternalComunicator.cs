namespace MUTDOD.Server.Common.ModuleBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IInternalComunicator
    {
        object ExecuteQuery(string query);
    }
}
