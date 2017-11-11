using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace OdraIDE.SolutionExplorer.Connections
{
    [Export(CompositionPoints.Workbench.Commands.NewDatabase, typeof(ICustomCommand))]
    public class NewDatabaseCommand : BaseCommand, IPartImportsSatisfiedNotification
    {
        [Import(CompositionPoints.Workbench.NewDatabaseDialog, typeof(NewDatabaseDialog))]
        private NewDatabaseDialog newDatabaseDialog { get; set; }

        public NewDatabaseCommand()
        {
            EnableCondition = new ConcreteCondition(true);
        }

        //[Export(typeof(KeyBinding))]
        //private KeyBinding KeyBinding
        //{
        //    get
        //    {
        //        //Exports shortcut for this command (F5)
        //        return new KeyBinding(this, new KeyGesture(Key.F2, ModifierKeys.None));
        //    }
        //}

        public void OnImportsSatisfied()
        {
            ExecuteCommand += new ExecuteHandler(CreateNewDatabase);
        }

        void CreateNewDatabase()
        {
            newDatabaseDialog.ShowDialog();
        }
    }
}
