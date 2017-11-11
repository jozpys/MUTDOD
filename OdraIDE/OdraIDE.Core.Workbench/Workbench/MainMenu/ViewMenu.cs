using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;

namespace OdraIDE.Core.Workbench
{
    [Export(ExtensionPoints.Workbench.MainMenu.Self, typeof(IMenuItem))]
    class ViewMenu : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public ViewMenu()
        {
            ID = Extensions.Workbench.MainMenu.View;
            Header = Resources.Strings.Workbench_MainMenu_View;
            InsertRelativeToID = Extensions.Workbench.MainMenu.Edit;
            BeforeOrAfter = RelativeDirection.After;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }

    /// <summary>
    /// This is a wrapper for a toolbar so it can be controlled
    /// by a menu item in View->Toolbars
    /// </summary>
    class ToolBarMenuItem : AbstractMenuItem
    {
        public ToolBarMenuItem(IToolBar toolBar)
        {
            // store the toolbar and create a condition to 
            // control its visibility
            m_toolBar = toolBar;
            // preserve the existing visible state, in case it 
            // was set to true in the toolbar constructor
            m_toolBarVisibleCondition = new ConcreteCondition(m_toolBar.Visible);
            m_toolBar.VisibleCondition = m_toolBarVisibleCondition;

            ID = toolBar.ID;
            Header = toolBar.Name;
            InsertRelativeToID = toolBar.InsertRelativeToID;
            BeforeOrAfter = toolBar.BeforeOrAfter;
            IsCheckable = true;
            IsChecked = toolBar.Visible;
        }

        private readonly IToolBar m_toolBar = null;
        private readonly ConcreteCondition m_toolBarVisibleCondition;

        protected override void OnIsCheckedChanged()
        {
            m_toolBarVisibleCondition.SetCondition(IsChecked);
        }
    }

    [Export(ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    class ViewMenuToolBars : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public ViewMenuToolBars()
        {
            ID = Extensions.Workbench.MainMenu.ViewMenu.ToolBars;
            Header = Resources.Strings.Workbench_MainMenu_View_ToolBars;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.ToolBars.Self, typeof(IToolBar), AllowRecomposition = true)]
        private IEnumerable<IToolBar> toolBars { get; set; }

        public void OnImportsSatisfied()
        {
            // have to convert the ToolBars into MenuItems
            IList<IToolBar> sortedToolBars = extensionService.Sort(toolBars);
            List<IMenuItem> toolBarMenuItems = new List<IMenuItem>();
            foreach (IToolBar tb in sortedToolBars)
            {
                toolBarMenuItems.Add(new ToolBarMenuItem(tb));
            }
            Items = extensionService.Sort(toolBarMenuItems);
        }
    }
}
