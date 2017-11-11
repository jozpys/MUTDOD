using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using OdraIDE.Core;
using System.Windows.Data;

namespace OdraIDE.SolutionExplorer
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.Views, typeof(ResourceDictionary))]
    public partial class SolutionExplorerView : ResourceDictionary
    {
        public SolutionExplorerView()
        {
            InitializeComponent();
        }
    }
}
