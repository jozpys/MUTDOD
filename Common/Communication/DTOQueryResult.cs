using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.Communication
{
    [Serializable]
    public class DTOQueryResult : IQueryResult
    {
        //for serialization
        public DTOQueryResult()
        {
        }

        public DTOQueryResult(IQueryResult query)
        {
            QueryResultType = query.QueryResultType;
            NextResult = query.NextResult;
            StringOutput = query.StringOutput;
            QueryResults = query.QueryResults;
        }

        public ResultType QueryResultType { get; set; }

        [XmlIgnore]
        public IQueryResult NextResult
        {
            get { return NextDTOResult; }
            set { NextDTOResult = value == null ? null : new DTOQueryResult(value); }
        }

        public DTOQueryResult NextDTOResult;
        public string StringOutput { get; set; }
        public List<Oid> QueryResults { get; set; }
    }
}
