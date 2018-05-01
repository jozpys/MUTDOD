using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdraIDE.Core.GUI.TreeItem
{
    public class TreeNode
    {
        private string name;
        private string value;
        private int ID;
        private int parentID;

        public TreeNode(string NodeName, int NodeId, int ParentNodeId, string NodeValue)
        {
            this.name = NodeName;
            this.ID = NodeId;
            this.parentID = ParentNodeId;
            this.value = NodeValue;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public int Id
        {
            get
            {
                return this.ID;
            }
        }

        public int ParentId
        {
            get
            {
                return this.parentID;
            }
        }
    }
}
