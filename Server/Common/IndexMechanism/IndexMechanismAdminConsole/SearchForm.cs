using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MUTDOD.Common.ModuleBase.Indexing;

namespace IndexMechanismAdminConsole
{
    public partial class SearchForm : Form
    {
        public delegate void SearchResult(Guid[] objects);

        public SearchResult SetResult;

        private int indexID;

        public SearchForm(Type[] objects, int indexID)
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(objects);
            this.indexID = indexID;
            this.Closing += new CancelEventHandler(SearchForm_Closing);
        }

        private void SearchForm_Closing(object sender, CancelEventArgs e)
        {
            SetResult(new Guid[0]);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                DataGridViewComboBoxCell attributes = (DataGridViewComboBoxCell) row.Cells["Atrybut"];
                attributes.Items.Clear();
                attributes.Items.AddRange(getAttribiutesOfType(comboBox1.SelectedItem as Type));


                DataGridViewComboBoxCell porownania = (DataGridViewComboBoxCell) row.Cells["Porownanie"];
                porownania.Items.Clear();
                porownania.Items.AddRange(getEnumsValues(typeof (IndexPlugin.CompareType)));
            }
        }

        private object[] getAttribiutesOfType(Type p)
        {
            if (p == null)
                return new object[0];

            return p.GetFields().Select(field => (object) field.Name).ToArray();
        }

        private object[] getEnumsValues(Type e)
        {
            if (!e.IsEnum)
                return new object[0];

            Array a = Enum.GetValues(e);
            List<object> values = new List<object>();
            for (int i = 0; i < a.Length; i++)
                values.Add(a.GetValue(i).ToString());
            return values.ToArray();
        }

        private void search_Click(object sender, EventArgs e)
        {
            setEnableSearchButtons(false);
            List<SearchingCondition> searchConditions = new List<SearchingCondition>();
            bool valid = true;
            for (int r = 0; r < dataGridView1.Rows.Count - 1; r++)
            {
                DataGridViewComboBoxCell attributes = (DataGridViewComboBoxCell) dataGridView1.Rows[r].Cells["Atrybut"];
                DataGridViewComboBoxCell porownania =
                    (DataGridViewComboBoxCell) dataGridView1.Rows[r].Cells["Porownanie"];
                DataGridViewTextBoxCell wartosc = (DataGridViewTextBoxCell) dataGridView1.Rows[r].Cells["Wartosc"];

                bool rowValid = true;

                if (attributes.Value == null || attributes.Value.ToString().Length == 0)
                    rowValid = false;
                else if (porownania.Value == null || porownania.Value.ToString().Length == 0)
                    rowValid = false;
                else if (wartosc.Value == null || wartosc.Value.ToString().Length == 0)
                    rowValid = false;

                valid = valid && rowValid;

                if (!rowValid)
                    dataGridView1.Rows[r].DefaultCellStyle.BackColor = Color.Pink;
                else
                {
                    searchConditions.Add(new SearchingCondition(attributes.Value.ToString(), porownania.Value.ToString(),
                                                                wartosc.Value.ToString()));
                    dataGridView1.Rows[r].DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                }
            }

            valid = valid && searchConditions.Count > 0;

            if (!valid)
            {
                setEnableSearchButtons(true);
                return;
            }

            DateTime search_start = DateTime.Now;

            Guid[] found =
                Form1.GetInstance().CheckIndexForObjects(indexID,
                                                         comboBox1.SelectedItem as Type,
                                                         complexExtension.Checked,
                                                         searchConditions.Select(s => s.attribiute).ToArray(),
                                                         searchConditions.Select(s => s.Comparator).ToArray(),
                                                         searchConditions.Select(s => s.Value).ToArray());

            if (SetResult != null)
            {
                TimeSpan search_time = DateTime.Now.Subtract(search_start);
                SetResult(found);
                MessageBox.Show(string.Format("Found {0} objects. total search time: {1:c}", found.Count(), search_time));
            }
            this.Closing -= new CancelEventHandler(SearchForm_Closing);
            this.Close();
        }

        private struct SearchingCondition
        {
            public string attribiute;
            private string compareOperator;
            private string value;

            public SearchingCondition(string a, string c, string v)
            {
                attribiute = a;
                compareOperator = c;
                value = v;
            }

            public CompareType Comparator
            {
                get
                {
                    Array e = Enum.GetValues(typeof (CompareType));
                    CompareType ret = CompareType.equal;
                    for (int i = 0; i < e.Length; i++)
                        if (e.GetValue(i).ToString() == compareOperator)
                            ret = (CompareType) e.GetValue(i);

                    return ret;
                }
            }

            public object Value
            {
                get
                {
                    DateTime retDT;
                    float retF;
                    int retI;

                    if (DateTime.TryParse(value, out retDT))
                        return retDT;
                    else if (int.TryParse(value, out retI))
                        return retI;
                    else if (float.TryParse(value, out retF))
                        return retF;
                    else
                        return value;
                }
            }
        }

        private void SearchObjectsNOAttribiutesNeeded_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;

            setEnableSearchButtons(false);
            Guid[] found =
                Form1.GetInstance().CheckIndexForObjects(indexID,
                                                         comboBox1.SelectedItem as Type,
                                                         complexExtension.Checked);

            if (SetResult != null)
                SetResult(found);
            this.Closing -= new CancelEventHandler(SearchForm_Closing);
            this.Close();
        }

        private void setEnableSearchButtons(bool enable)
        {
            search.Enabled = enable;
            SearchObjectsNOAttribiutesNeeded.Enabled = enable;
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {

        }
    }
}