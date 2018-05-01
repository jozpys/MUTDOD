using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdraIDE.Core;
using OdraIDE.QueryPlan.Services;
//using OdraIDE.Editor.DODQL.Workbench.ToolBar;
using System.Windows.Input;
using OdraIDE.QueryPlan.Model;

namespace OdraIDE.QueryPlan.Commands
{
    [Export(QueryPlan.CompositionPoints.Workbench.Commands.GenerateQueryPlan, typeof(Core.ICustomCommand))]
    public class GenerateQueryPlanCommand : BaseCommand
    {
        [Import(OdraIDE.Core.Services.QueryPlan.QueryPlanService, typeof(IQueryPlanService))]
        private IQueryPlanService queryPlanService { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        //[Import(OdraIDE.Editor.Sbql.CompositionPoints.Workbench.ExecuteToolbar.DatabasesComboBox, typeof(DatabasesToolBarComboBox))]
        //private DatabasesToolBarComboBox databasesComboBox { get; set; }

        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
        private ApplicationStatus applicationStatus { get; set; }

        private string m_lastQuery;

        [Export(typeof(KeyBinding))]
        private KeyBinding KeyBinding
        {
            get
            {
                //Exports shortcut for this command (F5)
                return new KeyBinding(this, new KeyGesture(Key.F6, ModifierKeys.None));
            }
        }

        public GenerateQueryPlanCommand()
        {
            EnableCondition = new ConcreteCondition(true);
            ExecuteCommand += new ExecuteHandler(GenerateQueryPlan);
        }

        private static FileName m_SelectedFileName;

        private void GenerateQueryPlan()
        {
            TreeTest treeTest = new TreeTest();
            treeTest.id = 3;
            treeTest.name = "trzeci";

            TreeTest t2 = new TreeTest();
            t2.id = 4;
            t2.name = "czwarty";
            treeTest.Children = new List<TreeTest>();
            treeTest.Children.Add(t2);

            List<TreeTest> lista = new List<TreeTest>();
            lista.Add(treeTest);

            queryPlanService.ShowQueryPlan(lista);
        }
    }
}
