using System;
using MUTDOD.Common;

namespace MUTDOD.Common.Communication
{
    [Serializable]
    public class DTOQuery : IQuery
    {
        //for serialization
        public DTOQuery() { }
        public DTOQuery(IQuery query)
        {
            QueryText = query.QueryText;
        }
        public string QueryText { get; set; }
    }
}
