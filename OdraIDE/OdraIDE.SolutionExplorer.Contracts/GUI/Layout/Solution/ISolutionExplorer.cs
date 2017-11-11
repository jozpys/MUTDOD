﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;

namespace OdraIDE.SolutionExplorer
{
    public interface ISolutionExplorer
    {
        SharpTreeView TreeView { get; }
        void ShowPropertiesFor(object value);
    }
}
