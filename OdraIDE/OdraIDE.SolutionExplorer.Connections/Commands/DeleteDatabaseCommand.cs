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
    [Export(CompositionPoints.Workbench.Commands.DeleteDatabase, typeof(ICustomCommand))]
    public class DeleteDatabaseCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.DeleteDatabaseDialog, typeof(DeleteDatabaseDialog))]
        private DeleteDatabaseDialog DeleteDatabaseDialog { get; set; }

        public string DatabaseName { get; set; }

        public DeleteDatabaseCommand()
        {
            EnableCondition = new ConcreteCondition(true);
        }

        public void OnImportsSatisfied()
        {
            ExecuteCommand += new ExecuteHandler(RenameDatabase);
        }

        void RenameDatabase()
        {
            DeleteDatabaseDialog.DatabaseName = DatabaseName;
            DeleteDatabaseDialog.ShowDialog();
        }
    }
}
