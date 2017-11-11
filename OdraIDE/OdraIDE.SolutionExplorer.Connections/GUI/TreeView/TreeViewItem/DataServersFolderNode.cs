using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using OdraIDE.Utilities;

namespace OdraIDE.SolutionExplorer.Connections
{
    public class DataServersFolderNode : SharpTreeNode
    {
        public DataServersFolderNode()
        {
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return "Data Servers";
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.DataServers);
            }
        }
    }
}
