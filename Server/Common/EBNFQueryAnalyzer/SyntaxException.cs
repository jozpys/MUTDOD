using MUTDOD.Common.ModuleBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    class SyntaxException : QuerySyntaxException
    {
        private string msg;

        public SyntaxException(string _msg) : base()
        {
            this.msg = _msg;
        }

        public override string Message
        {
            get
            {
                if (msg.StartsWith("unexpected token"))
                {
                    if (msg.IndexOf(" expected one") != -1)
                        this.msg = String.Format("{0} {1}", msg.Substring(0, msg.IndexOf(" expected one")), msg.Substring(msg.IndexOf("on line")));
                    return this.msg;
                }
                else
                    return msg;
            }
        }
    }
}
