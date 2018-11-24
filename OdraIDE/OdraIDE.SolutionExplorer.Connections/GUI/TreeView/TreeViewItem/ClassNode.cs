
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                if (m_interface)
                {
                    return ImageHelper.GetImageFromResources(Resources.Images.PurpleBall);
                }
                return ImageHelper.GetImageFromResources(Resources.Images.GreenBall);
            }
        }
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("FieldNode")]
    public class FieldNode : SharpTreeNode
    {
        private string m_className;
        private string m_type;
        private bool m_reference;
        private bool m_array;

        public FieldNode(string className, string type, bool reference, bool array)
        {
            m_className = className;
            m_type = type;
            m_reference = reference;
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
                if (m_reference)
                {
                    return ImageHelper.GetImageFromResources(Resources.Images.reference);
                }
                return ImageHelper.GetImageFromResources(Resources.Images.field);
            }
        }
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export("MethodNode")]
    public class MethodNode : SharpTreeNode
    {
        private string m_className;
        private string m_returnType;
        private OrderedDictionary m_parameters;

        public MethodNode(string className, string returnValue, OrderedDictionary parameters)
        {
            m_className = className;
            m_returnType = returnValue;
            m_parameters = parameters;
            ShowIcon = true;
        }

        public override object Text
        {
            get
            {
                List<String> parametersString = new List<String>();
                foreach(DictionaryEntry param in m_parameters){
                    parametersString.Add(param.Key + ":" + param.Value);
                }
                if(m_returnType == null)
                {
                    m_returnType = "null";
                }
                String text = m_className + " [:" + m_returnType + "(" + String.Join(", ", parametersString) + ")]";
                return text;
            }
        }

        public override object Icon
        {
            get
            {
                return ImageHelper.GetImageFromResources(Resources.Images.Gear);
            }
        }
    }
}
