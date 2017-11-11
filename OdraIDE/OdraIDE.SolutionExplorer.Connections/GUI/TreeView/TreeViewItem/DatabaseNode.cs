using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using OdraIDE.Utilities;

namespace OdraIDE.SolutionExplorer.Connections
{
    public class DatabaseNode : SharpTreeNode
    {
        private string m_databaseName;

        public DatabaseNode(string databaseName)
        {
            m_databaseName = databaseName;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return m_databaseName;
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.Database);
            }
        }
    }
}
