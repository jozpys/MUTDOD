using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core.Solution
{
    
    [Export(OdraIDE.Core.Services.Solution.SolutionService, typeof(ISolutionService))]
    public class SolutionService : ISolutionService
    {



    }
}
