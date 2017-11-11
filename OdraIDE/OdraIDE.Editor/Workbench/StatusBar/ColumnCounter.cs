using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.Editor
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    [Export(CompositionPoints.Workbench.StatusBar.ColumnCounterHeading, typeof(ColumnCounterHeading))]
    public class ColumnCounterHeading : AbstractStatusBarLabel
    {
        public ColumnCounterHeading()
        {
            ID = "ColumnCounterHeading";
            InsertRelativeToID = "LineCounterText";
            BeforeOrAfter = RelativeDirection.After;
        }

        public void Show()
        {
            Text = "Column";
        }

        public void Clear()
        {
            Text = "";
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    [Export(CompositionPoints.Workbench.StatusBar.ColumnCounterText, typeof(ColumnCounterText))]
    public class ColumnCounterText : AbstractStatusBarLabel
    {
        public ColumnCounterText()
        {
            ID = "ColumnCounterText";
            Text = "";
            InsertRelativeToID = "ColumnCounterHeading";
            BeforeOrAfter = RelativeDirection.After;
        }

        public void SetColumnNumber(int column)
        {
            Text = column.ToString();
        }

        public void Clear()
        {
            Text = "";
        }
    }
}
