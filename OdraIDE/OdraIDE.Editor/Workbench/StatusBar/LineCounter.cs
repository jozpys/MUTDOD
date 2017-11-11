using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.Editor
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    [Export(CompositionPoints.Workbench.StatusBar.LineCounterHeading, typeof(LineCounterHeading))]
    public class LineCounterHeading : AbstractStatusBarLabel
    {
        public LineCounterHeading()
        {
            ID = "LineCounterHeading";
            InsertRelativeToID = "ApplicationStatusSeparator";
            BeforeOrAfter = RelativeDirection.After;
        }

        public void Show()
        {
            Text = "Line";
        }

        public void Clear()
        {
            Text = "";
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.StatusBar, typeof(IStatusBarItem))]
    [Export(CompositionPoints.Workbench.StatusBar.LineCounterText, typeof(LineCounterText))]
    public class LineCounterText : AbstractStatusBarLabel
    {
        public LineCounterText()
        {
            ID = "LineCounterText";
            Text = "";
            InsertRelativeToID = "LineCounterHeading";
            BeforeOrAfter = RelativeDirection.After;
        }

        public void SetLineNumber(int line)
        {
            Text = line.ToString();
        }

        public void Clear()
        {
            Text = "";
        }
    }
}
