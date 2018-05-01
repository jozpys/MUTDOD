using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OdraIDE.QueryPlan
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.Views, typeof(ResourceDictionary))]
    public partial class TreeResultPadView : ResourceDictionary
    {
        public TreeResultPadView()
        {
            InitializeComponent();
        }
    }
}
