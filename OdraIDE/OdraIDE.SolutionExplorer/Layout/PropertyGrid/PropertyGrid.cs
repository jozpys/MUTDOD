using System;
using System.Collections.Generic;
using System.Linq;
using OdraIDE.Core;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Forms.Integration;
using OdraIDE.Utilities;

namespace OdraIDE.SolutionExplorer
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(CompositionPoints.Workbench.Pads.PropertyGrid, typeof(PropertyGrid))]
    [Pad(Name = PropertyGrid.PG_NAME)]
    public class PropertyGrid : AbstractPad
    {
        public const string PG_NAME = "PropertyGrid";

        public PropertyGrid()
        {
            Name = PG_NAME;
            Title = "Properties";
            Location = PadLocation.TopLeft;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.Properties);

            m_propertyGridHost.Child = m_propertyGrid;

        }

        private System.Windows.Forms.PropertyGrid m_propertyGrid = new System.Windows.Forms.PropertyGrid();
        private WindowsFormsHost m_propertyGridHost = new WindowsFormsHost();

        public WindowsFormsHost PropertyGridHost
        {
            get
            {
                return m_propertyGridHost;
            }
        }

        public void SetSelectedObject(object value)
        {
            m_propertyGrid.SelectedObject = value;
        }
    }
}
