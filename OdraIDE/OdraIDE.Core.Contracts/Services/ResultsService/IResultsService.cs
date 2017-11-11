using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IResultsService
    {
        void ShowStringResult(string result);
        void ShowDataResult(DataMatrix dataMatrix);
        void Clear();
    }
}
