using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using OdraIDE.Utilities;
using System.Windows.Controls;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("DatabasesFolderNode")]
    public class DatabasesFolderNode : SharpTreeNode
    {

        [Import(CompositionPoints.Workbench.Commands.NewDatabase, typeof(ICustomCommand))]
        private NewDatabaseCommand newDatabaseCommand { get; set; }
        
        public DatabasesFolderNode()
        {
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return "Databases";
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.DatabaseDown);
            }
        }

        public override object ExpandedIcon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.DatabaseUp);
            }
        }

        private ContextMenu m_menu;

        public override ContextMenu GetContextMenu()
        {
            if (m_menu == null)
            {
                m_menu = new ContextMenu();
                MenuItem addDatabaseItem = new MenuItem();
                addDatabaseItem.Command = newDatabaseCommand;
                addDatabaseItem.Header = "Add Database";
                addDatabaseItem.Icon = ImageHelper.GetImageFromResources(Resources.Images.AddDatabase);
                m_menu.Items.Add(addDatabaseItem);
            }
            return m_menu;
        }
    }
}
