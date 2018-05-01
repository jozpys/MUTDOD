using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using OdraIDE.Core;
using System.ComponentModel;
using OdraIDE.Utilities;
using System.Windows.Data;

namespace OdraIDE.Core
{
    public class DynamicListView : ListView, INotifyPropertyChanged
    {
        public DynamicListView()
        {
            ItemsSource = DataMatrix;
            GridView gridView = new GridView();
            View = gridView;
            
            
        }

        private DataMatrix m_DataMatrix;

        public DataMatrix DataMatrix 
        {
            get
            {
                return m_DataMatrix;
            }

            set
            {
                if (m_DataMatrix != value)
                {
                    m_DataMatrix = value;
                    RefreshView();
                    NotifyPropertyChanged(m_DataMatrixArgs);
                    ItemsSource = DataMatrix;
                }
            }
        }

        private void RefreshView()
        {
            GridView gridView = View as GridView;
            int count = 0;
            gridView.Columns.Clear();
            foreach (var col in DataMatrix.Columns)
            {
                GridViewColumn colView = new GridViewColumn();
                colView.Header = col.Name;
                if (col.Width > 0) colView.Width = col.Width;
                
                if (col.CellTemplate != null) 
                {
                    colView.CellTemplate = col.CellTemplate;
                }
                else
                {
                    colView.DisplayMemberBinding = new Binding(string.Format("[{0}]", count));
                }

                gridView.Columns.Add(colView);
                count++;
            }
        }

        static readonly PropertyChangedEventArgs m_DataMatrixArgs =
            NotifyPropertyChangedHelper.CreateArgs<DynamicListView>(o => o.DataMatrix);


        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            var evt = PropertyChanged;
            if (evt != null)
            {
                evt(this, e);
            }
        }
    }
}
