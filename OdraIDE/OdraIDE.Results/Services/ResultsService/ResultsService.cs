using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.Results
{
    [Export(OdraIDE.Core.Services.Results.ResultsService, typeof(IResultsService))]
    public class ResultsService : IResultsService
    {
        [Import(CompositionPoints.Workbench.Pads.StringResultsPad, typeof(StringResultsPad))]
        private StringResultsPad stringResultsPad { get; set; }

        [Import(CompositionPoints.Workbench.Pads.GridResultsPad, typeof(GridResultsPad))]
        private GridResultsPad gridResultsPad { get; set; }

        public void ShowStringResult(string result)
        {
            stringResultsPad.ShowResult(result);
        }

        public void Clear()
        {
            stringResultsPad.Clear();
            gridResultsPad.Clear();
        }


        public void ShowDataResult(DataMatrix dataMatrix)
        {
            gridResultsPad.ShowResults(dataMatrix);
        }

        public void FocusOnDataResult()
        {
            gridResultsPad.Focus();
        }
    }
}
