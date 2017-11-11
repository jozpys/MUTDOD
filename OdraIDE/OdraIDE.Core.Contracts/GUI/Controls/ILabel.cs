using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface ILabel : IControl
    {
        string Text { get; }
    }
}
