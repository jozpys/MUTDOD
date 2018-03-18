using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(CompositionPoints.Workbench.Commands.RenameDatabase, typeof(ICustomCommand))]
    public class RenameDatabaseCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.RenameDatabaseDialog, typeof(RenameDatabaseDialog))]
        private RenameDatabaseDialog renameDatabaseDialog { get; set; }

        public string DatabaseName { get; set; }

        public RenameDatabaseCommand()
        {
            EnableCondition = new ConcreteCondition(true);
        }

        public void OnImportsSatisfied()
        {
            ExecuteCommand += new ExecuteHandler(RenameDatabase);
        }

        void RenameDatabase()
        {
            renameDatabaseDialog.DatabaseName = DatabaseName;
            renameDatabaseDialog.ShowDialog();
        }
    }
}
