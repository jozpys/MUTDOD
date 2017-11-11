using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using System.Resources;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using OdraIDE.Utilities;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("CentralServerNode")]
    public class CentralServerNode : SharpTreeNode
    {
        [Import(OdraIDE.SolutionExplorer.CompositionPoints.Workbench.Commands.ShowProperties, typeof(ICustomCommand))]
        private ShowPropertiesCommand showPropertiesCommand { get; set; }

        [Import(CompositionPoints.Workbench.Commands.Disconnect, typeof(ICustomCommand))]
        private DisconnectCommand disconnectCommand { get; set; }

        public CentralServerProperties Properties { get; set; }

        public CentralServerNode()
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
                return ImageHelper.GetImageFromResources(Resources.Images.CentralServer);
            }
        }

        private ContextMenu m_menu;

        public override ContextMenu GetContextMenu()
        {
            if (m_menu == null)
            {
                m_menu = new ContextMenu();

                MenuItem disconnectItem = new MenuItem();
                showPropertiesCommand.ObjectToShow = Properties;
                disconnectItem.Command = disconnectCommand;
                disconnectItem.Header = "Disconnect";
                m_menu.Items.Add(disconnectItem);

                m_menu.Items.Add(new Separator());

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
