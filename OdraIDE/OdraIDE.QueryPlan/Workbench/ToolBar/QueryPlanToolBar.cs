using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.QueryPlan.Workbench.ToolBar
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.ToolBars.Self, typeof(IToolBar))]
    class QueryPlanToolBar : AbstractToolBar, IPartImportsSatisfiedNotification
    {

        public QueryPlanToolBar()
        {
            Name = Resources.Strings.Workbench_QueryPlan;
            VisibleCondition = new ConcreteCondition(true);
        }

        [Import(OdraIDE.Core.Services.Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        public IExtensionService extensionService { get; set; }

        [ImportMany(QueryPlan.ExtensionPoints.Workbench.ToolBars.QueryPlan, typeof(IToolBarItem), AllowRecomposition = true)]
        private IEnumerable<IToolBarItem> importedItems { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private Lazy<IFileService> fileService { get; set; }

        [Export(QueryPlan.ExtensionPoints.Workbench.ToolBars.QueryPlan, typeof(IToolBarItem))]
        public class QueryPlanViewToolbar: AbstractToolBarButton, IPartImportsSatisfiedNotification
        {
            //[Import(QueryPlan.CompositionPoints.Workbench.Commands.GenerateQueryPlan, typeof(ICustomCommand))]
            //private ICustomCommand command { get; set; }

            public QueryPlanViewToolbar()
            {
                ID = "QueryPlanViewToolbar";
                ToolTip = "Generate Query Plan";
                SetIconFromBitmap(Resources.Images.query_plan);
            }

            #region IPartImportsSatisfiedNotification Members

            public void OnImportsSatisfied()
            {
               
               // Command = command;
            }

            #endregion
        }

        public void OnImportsSatisfied()
        {
            extensionService.Sort(importedItems);
            Items = importedItems;
            //throw new NotImplementedException();
        }
    }
}
