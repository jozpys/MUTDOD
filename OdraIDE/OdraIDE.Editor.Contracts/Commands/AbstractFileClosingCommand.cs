using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;

namespace OdraIDE.Editor
{
    public abstract class AbstractFileClosingCommand : AbstractExtension, IFileClosingCommand
    {
        public abstract bool OnClosing(OpenedFile file);
    }
}
