using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace OdraIDE.Core.Layout
{
    [Export(ExtensionPoints.Host.Views, typeof(ResourceDictionary))]
    public partial class LayoutManagerView : ResourceDictionary
    {
        public LayoutManagerView()
        {
            InitializeComponent();
        }
    }
}
