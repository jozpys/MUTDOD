using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using ICSharpCode.AvalonEdit.Rendering;

namespace OdraIDE.Tasks
{
    public interface ITaskProvider
    {
        void Analyze(OpenedFile file);
    }
}
