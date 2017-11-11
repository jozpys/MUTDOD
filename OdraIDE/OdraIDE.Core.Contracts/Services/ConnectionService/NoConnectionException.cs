using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public class NoConnectionException : ApplicationException
    {
        public override string Message
        {
            get
            {
                return "There is no connection to server";
            }
        }
    }
}
