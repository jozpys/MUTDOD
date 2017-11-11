
using System.ComponentModel.Composition;
using ICSharpCode.TreeView;
using OdraIDE.Utilities;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("ClassNode")]
    public class ClassNode : SharpTreeNode
    {
        private string m_className;

        public ClassNode(string className)
        {
            m_className = className;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return m_className;
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.NewSolution);
            }
        }
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("FieldNode")]
    public class FieldNode : SharpTreeNode
    {
        private string m_className;

        public FieldNode(string className)
        {
            m_className = className;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return m_className;
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.field);
            }
        }
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("MethodNode")]
    public class MethodNode : SharpTreeNode
    {
        private string m_className;

        public MethodNode(string className)
        {
            m_className = className;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                return m_className;
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.method);
            }
        }
    }
}
