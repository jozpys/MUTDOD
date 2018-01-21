using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.QueryTree
{
    public class NoClassPropertyException : Exception
    {
        public String PropertyName { get; set; }
    }
}
