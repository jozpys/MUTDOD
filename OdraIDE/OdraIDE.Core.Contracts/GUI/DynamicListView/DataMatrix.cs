using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    /// <summary>
    /// From http://www.codeproject.com/KB/WPF/WPF_DynamicListView.aspx
    /// </summary>
    public class DataMatrix : IEnumerable
    {
        public List<MatrixColumn> Columns { get; set; }
        public List<object[]> Rows { get; set; }

        public DataMatrix()
        {
            Columns = new List<MatrixColumn>();
            Rows = new List<object[]>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GenericEnumerator(Rows.ToArray());
        }
    }
}