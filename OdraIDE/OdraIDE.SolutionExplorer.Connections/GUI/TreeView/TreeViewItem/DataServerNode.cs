using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using OdraIDE.Utilities;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("DataServerNode")]
    public class DataServerNode : SharpTreeNode
    {
        public DataServerProperties Properties { get; set; }

        public DataServerNode()
        {
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return Properties.Name;
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.DataServer);
            }
        }

        private ContextMenu m_menu;

        [Import(OdraIDE.SolutionExplorer.CompositionPoints.Workbench.Commands.ShowProperties, typeof(ICustomCommand))]
        private ShowPropertiesCommand showPropertiesCommand { get; set; }

        public override ContextMenu GetContextMenu()
        {
            if (m_menu == null)
            {
                m_menu = new ContextMenu();
                MenuItem propertiesItem = new MenuItem();
                showPropertiesCommand.ObjectToShow = Properties;
                propertiesItem.Command = showPropertiesCommand;
                propertiesItem.Header = "Properties";
                m_menu.Items.Add(propertiesItem);
            }
            return m_menu;
        }
    }
}
