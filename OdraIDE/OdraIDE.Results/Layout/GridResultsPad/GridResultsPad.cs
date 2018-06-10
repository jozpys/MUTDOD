using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using OdraIDE.Utilities;
using System.Windows.Controls;
using System.Windows.Data;

namespace OdraIDE.Results
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(OdraIDE.Results.CompositionPoints.Workbench.Pads.GridResultsPad, typeof(GridResultsPad))]
    [Pad(Name = GridResultsPad.RP_NAME)]
    public class GridResultsPad : AbstractPad
    {
        public const string RP_NAME = "GridResultsPad";

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        public GridResultsPad()
        {
            Name = RP_NAME;
            Title = "Data results";
            Location = PadLocation.Bottom;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.Results);
        }

        private DynamicListView m_ListView = new DynamicListView();

        public DynamicListView ListView
        {
            get
            {
                return m_ListView;
            }
        }

        public void ShowResults(DataMatrix dm)
        {
            m_ListView.DataMatrix = dm;
            layoutManager.Value.ShowPad(this);
        }

        public void Clear()
        {
            m_ListView.DataMatrix = new DataMatrix(); ;
        }

        public void Focus()
        {
            layoutManager.Value.ShowPad(this);
        }
    }
}
