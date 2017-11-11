using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace OdraIDE.Core
{
    /// <summary>
    /// From http://www.codeproject.com/KB/WPF/WPF_DynamicListView.aspx
    /// </summary>
    public class MatrixColumn
    {
        public string Name { get; set; }
        public string StringFormat { get; set; }
        public int Width { get; set; }
        public DataTemplate CellTemplate { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}