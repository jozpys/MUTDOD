using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.SolutionExplorer
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(CompositionPoints.Workbench.Commands.ShowProperties, typeof(ICustomCommand))]
    public class ShowPropertiesCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.Pads.SolutionExplorer, typeof(ISolutionExplorer))]
        private ISolutionExplorer solutionExplorer { get; set; }
        
        public object ObjectToShow { get; set; }

        void ShowProperties()
        {
            solutionExplorer.ShowPropertiesFor(ObjectToShow);
        }

        public void OnImportsSatisfied()
        {
            ExecuteCommand += new ExecuteHandler(ShowProperties);
        }
    }
}
