using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace OdraIDE.Editor
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.Views, typeof(ResourceDictionary))]
    public partial class SourceEditorView : ResourceDictionary
    {

        public SourceEditorView()
        {
            InitializeComponent();
        }
    }
}
