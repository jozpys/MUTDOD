using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace OdraIDE.Core
{
    [Export(ExtensionPoints.Host.Views, typeof(ResourceDictionary))]
    public partial class AbstractStatusBarSeparatorView : ResourceDictionary
    {
        public AbstractStatusBarSeparatorView()
        {
            InitializeComponent();
        }
    }
}
