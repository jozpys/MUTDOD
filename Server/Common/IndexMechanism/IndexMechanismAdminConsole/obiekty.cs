using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MUTDOD.Common.Types;

namespace IndexMechanismAdminConsole
{
    public partial class obiekty : Form
    {
        private Dictionary<int, TabPage> _tabPages;
        private Dictionary<int, TreeView> _objectViewer;
        private Dictionary<int, List<Type>> _indexObjectTypes;

        public obiekty()
        {
            InitializeComponent();
            _tabPages = new Dictionary<int, TabPage>();
            _objectViewer = new Dictionary<int, TreeView>();
            _indexObjectTypes = new Dictionary<int, List<Type>>();
        }

        public void SetIndexes(Dictionary<int, string> indexes)
        {
            tabControl.TabPages.Clear();
            _tabPages.Clear();
            _objectViewer.Clear();
            _indexObjectTypes.Clear();
            foreach (KeyValuePair<int, string> index in indexes)
            {
                AddIndex(index.Key, index.Value);
            }
        }

        public bool AddIndex(int indexID, string indexName)
        {
            if (_tabPages.ContainsKey(indexID))
                return false;

            TabPage page = new TabPage();
            page.Text = indexName;
            page.Name = string.Format("page-{0}", indexID);
            page.Width = tabControl.Width;
            page.Height = tabControl.Height;
            TreeView obiekty = new TreeView();
            obiekty.Location = new Point(0, 0);
            obiekty.Width = page.Width;
            obiekty.Height = page.Height;
            page.Controls.Add(obiekty);
            obiekty.Dock = DockStyle.Fill;
            tabControl.TabPages.Add(page);
            page.Dock = DockStyle.Fill;
            _tabPages.Add(indexID, page);
            _objectViewer.Add(indexID, obiekty);
            _indexObjectTypes.Add(indexID, new List<Type>());
            return true;
        }

        public void AddIndexAndRemoveOldIfExists(int indexID, string indexName)
        {
            RemoveIndexIfExists(indexID);
            AddIndex(indexID, indexName);
        }

        public void RemoveIndexIfExists(int indexID)
        {
            if (_tabPages.ContainsKey(indexID))
            {
                tabControl.TabPages.Remove(_tabPages[indexID]);
                _tabPages.Remove(indexID);
            }
            if (_objectViewer.ContainsKey(indexID))
                _objectViewer.Remove(indexID);
        }

        public void AddObjectsInToIndex(int indexID, Oid[] objs)
        {
            foreach (Oid obj in objs.OrderBy(p => p.Id))
            {
                AddObjectInToIndex(indexID, obj);
            }
        }

        public void AddObjectsInToIndexAndClearExisting(int indexID, Oid[] objs)
        {
            if (_objectViewer.ContainsKey(indexID))
                _objectViewer[indexID].Nodes.Clear();

            AddObjectsInToIndex(indexID, objs);
        }

        public void AddObjectInToIndex(int indexID, Oid obj)
        {
            if (!_objectViewer.ContainsKey(indexID))
                throw new Exception("Firs add Index to object viewer");

            TreeView viewer = _objectViewer[indexID];
            TreeNode obiekt = new TreeNode(string.Format("{0} - {1}", obj.Id, obj.GetType().FullName),
                                           bulidAttribiutes(obj));
            obiekt.Name = obj.Id.ToString();

            TreeNode oldObiekt = findObjectNode(viewer.Nodes, obj);
            if (oldObiekt != null)
                viewer.Nodes.Remove(oldObiekt);

            viewer.Nodes.Add(obiekt);
            //viewer.Sort();
            if (!_indexObjectTypes[indexID].Contains(obj.GetType()))
                _indexObjectTypes[indexID].Add(obj.GetType());

            tabControl.SelectedTab = _tabPages[indexID];
        }

        private TreeNode[] bulidAttribiutes(object obj)
        {
            List<TreeNode> ret = new List<TreeNode>();

            FieldInfo[] objFields = obj.GetType().GetFields();
            String[] attribiutes = objFields.Select(p => p.Name).ToArray();
            foreach (string attribiute in attribiutes.OrderBy(p => p))
            {
                object attributeValue = objFields.Single(p => p.Name == attribiute).GetValue(obj);
                if (Convert.GetTypeCode(attributeValue) != TypeCode.Object)
                    ret.Add(new TreeNode(attribiute, new TreeNode[] {new TreeNode(attributeValue.ToString())}));
                else if (attributeValue.GetType().GetInterfaces().Contains(typeof (ICollection)))
                    ret.Add(new TreeNode(attribiute, buildCollectionNode((ICollection) attributeValue)));
                else if (attributeValue.GetType().GetInterfaces().Contains(typeof (Oid)))
                    ret.Add(
                        new TreeNode(string.Format("{0} - {1}", ((Oid) attributeValue).Id,
                                                   attributeValue.GetType().FullName)));
                else if (attributeValue.GetType().IsClass)
                    ret.Add(new TreeNode(attribiute, bulidAttribiutes(attributeValue)));
                else
                    ret.Add(new TreeNode(attribiute, new TreeNode[] {new TreeNode(attributeValue.ToString())}));
            }

            return ret.ToArray();
        }

        private TreeNode[] buildCollectionNode(ICollection obj)
        {
            List<TreeNode> ret = new List<TreeNode>();

            foreach (var o in obj)
            {
                if (Convert.GetTypeCode(o) != TypeCode.Object)
                    ret.Add(new TreeNode(o.ToString()));
                else if (o.GetType().GetInterfaces().Contains(typeof (Oid)))
                    ret.Add(new TreeNode(string.Format("{0} - {1}", o.GetType().FullName, ((Oid) o).Id)));
                else if (o.GetType().IsClass)
                    ret.Add(new TreeNode(obj.GetType().FullName, bulidAttribiutes(o)));
                else
                    ret.Add(new TreeNode(o.GetType().FullName, new TreeNode[] {new TreeNode(o.ToString())}));
            }

            return ret.ToArray();
        }

        private TreeNode findObjectNode(TreeNodeCollection nodeCollection, Oid obj)
        {
            TreeNode ret = null;

            foreach (TreeNode node in nodeCollection)
            {
                if (node.Name == obj.Id.ToString())
                    ret = node;
            }
            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_tabPages.Count == 0)
                return;
            button1.Enabled = false;
            TabPage page = tabControl.SelectedTab;
            int indexID = -1;
            foreach (KeyValuePair<int, TabPage> tabPage in _tabPages)
            {
                if (tabPage.Value.Equals(page))
                    indexID = tabPage.Key;
            }
            SearchForm sf = new SearchForm(_indexObjectTypes[indexID].ToArray(), indexID);
            sf.SetResult += new SearchForm.SearchResult(setSearchReulat);
            sf.Show();
        }

        internal void setSearchReulat(Guid[] objects)
        {
            button1.Enabled = true;

            listBox1.Items.Clear();
            if (objects != null)
                listBox1.Items.AddRange(objects.Select(p => (object) p).ToArray());
        }
    }
}