using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface IExtensionService
    {
        IList<T> Sort<T>(IEnumerable<T> extensionCollection) where T : IExtension;
    }
}
