
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ICSharpCode.TreeView;
using OdraIDE.Utilities;

namespace OdraIDE.SolutionExplorer.Connections
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("ClassNode")]
    public class ClassNode : SharpTreeNode
    {
        private string m_className;
        private bool m_interface;
        private List<string> m_parentClasses;

        public ClassNode(string className, bool isInterface, List<string> parentClasses)
        {
            m_className = className;
            m_interface = isInterface;
            m_parentClasses = parentClasses;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                string visableName = m_className;
                if (m_interface)
                {
                    visableName = "(I) " + visableName;
                }
                if(m_parentClasses.Any())
                {
                    visableName += " :" + String.Join(" ,", m_parentClasses);
                }
                return visableName;
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
        private string m_type;
        private bool m_array;

        public FieldNode(string className, string type, bool array)
        {
            m_className = className;
            m_type = type;
            m_array = array;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                if (m_array)
                {
                    return m_className + " (Array " + m_type + "[ ])";
                }
                return m_className + " (" + m_type + ")";
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
