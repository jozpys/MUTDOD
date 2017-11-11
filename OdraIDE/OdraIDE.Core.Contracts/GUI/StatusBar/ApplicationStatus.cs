using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using System.ComponentModel;

namespace OdraIDE.Core
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    [Export(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationStatus, typeof(ApplicationStatus))]
    public class ApplicationStatus : AbstractStatusBarLabel
    {
        [Import(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationProgressBar, typeof(ApplicationProgressBar))]
        private ApplicationProgressBar applicationProgressBar { get; set; }

        public ApplicationStatus()
        {
            ID = "ApplicationStatus";
        }

        public void SetStatus(string status, bool showProgressBar)
        {
            Text = status;
            applicationProgressBar.SetVisible(showProgressBar);
        }

        public void Clear()
        {
            Text = "";
        }

        public void Busy()
        {
            Text = "Busy...";
            applicationProgressBar.Show();
        }

        public void Ready()
        {
            Text = "Ready";
            applicationProgressBar.Hide(); ;
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    [Export(OdraIDE.Core.CompositionPoints.Workbench.StatusBar.ApplicationProgressBar, typeof(ApplicationProgressBar))]
    public class ApplicationProgressBar : AbstractStatusBarProgressBar
    {
        public ApplicationProgressBar()
        {
            ID = "ApplicationProgressBar";
            InsertRelativeToID = "ApplicationStatus";
            BeforeOrAfter = RelativeDirection.After;
            VisibleCondition = new ConcreteCondition(false);
        }

        public void Show()
        {
            (VisibleCondition as ConcreteCondition).SetCondition(true);
        }

        public void Hide()
        {
            (VisibleCondition as ConcreteCondition).SetCondition(false);
        }

        public void SetVisible(bool visible)
        {
            (VisibleCondition as ConcreteCondition).SetCondition(visible);
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    public class ApplicationStatusSeparator : AbstractStatusBarSeparator
    {
        public ApplicationStatusSeparator()
        {
            ID = "ApplicationStatusSeparator";
            InsertRelativeToID = "ApplicationProgressBar";
            BeforeOrAfter = RelativeDirection.After;
        }
    }
}
