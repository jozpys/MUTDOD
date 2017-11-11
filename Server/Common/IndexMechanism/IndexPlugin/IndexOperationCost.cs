using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexPlugin
{
    public class IndexOperationCost
    {
        public int PessimisticCost { get; set; }
        public int AverageCost { get; set; }
        public int OptimisticCost { get; set; }
    }
}