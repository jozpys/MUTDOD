using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core.Exceptions
{
    public class InvalidPluginException : Exception
    {
        public PluginConfig Plugin { get; set; }

        public InvalidPluginException() : base() { }
        public InvalidPluginException(string message) : base(message) { }
        public InvalidPluginException(string message, System.Exception inner) : base(message, inner) { }
    }
}
