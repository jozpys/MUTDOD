using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IControl : IExtension
    {
        string ToolTip { get; }
        bool Visible { get; }
        ICondition VisibleCondition { get; set; }
    }
}
