using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndexMechanism.CORE;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using CompareType = MUTDOD.Common.ModuleBase.Indexing.CompareType;

namespace IndexMechanismAdminConsole
{
    internal delegate void AddMessageToGrid(string[] row);

    public partial class Form1 : Form, ILogger
    {
        private IIndexMechanism im;
        private obiekty _obiekty;
        private AddMessageToGrid _logMessageInGrid;

        private static Form1 _instance;

        internal static Form1 GetInstance()
        {
            if (_instance == null)
                _instance = new Form1();

            return _instance;
        }

        private Form1()
        {
            InitializeComponent();
            this.Shown += new EventHandler(Form1_Shown);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            _logMessageInGrid += new AddMessageToGrid(LogMessageToGrid);
            _obiekty = new obiekty();
            _obiekty.Closed += new EventHandler(_obiekty_Closed);
            im = new MUTDOD.Server.Common.IndexMechanism.IndexMechanism(this);
            button3_Click(null, null);
            _obiekty.Show();
            this.Focus();
        }

        private void _obiekty_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogMessageToGrid(string[] row)
        {
            this.dataGridView2.Rows.Add(row);
            this.dataGridView2.FirstDisplayedScrollingRowIndex = this.dataGridView2.RowCount - 1;
           
            using (StreamWriter stream = new StreamWriter("Log.txt", true))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in row)
                {
                    sb.Append(s);
                    sb.Append(" | ");
                }
                stream.WriteLine(sb.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {InitialDirectory = Application.StartupPath};
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                MessageBox.Show(im.AddIndex(openFileDialog.FileName) ? "dodano pomyślnie" : "nie udało się dodać!");

            button3_Click(null, null);
        }

        #region Implementation of ILogger

        private string GetPreambula(MessageLevel type)
        {
            string preambula;
            switch (type)
            {
                case MessageLevel.Error:
                    preambula = "E";
                    break;
                case MessageLevel.Info:
                    preambula = "I";
                    break;
                case MessageLevel.Warning:
                    preambula = "W";
                    break;
                default:
                    preambula = "";
                    break;
            }
            return preambula;
        }

        public void Log(string sender, string msg, MessageLevel type)
        {
            this.Invoke(_logMessageInGrid, new object[]
                                               {
                                                   new string[]
                                                       {
                                                           string.Format("{0:HH:mm:ss:fff}", DateTime.Now),
                                                           GetPreambula(type),sender, msg
                                                       }
                                               });
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            foreach (KeyValuePair<int, string> ix in this.im.GetIndexes())
            {
                this.dataGridView1.Rows.Add(new string[] {Boolean.FalseString, ix.Value, ix.Key.ToString()});
            }
            _obiekty.SetIndexes(this.im.GetIndexes());
            ;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int del = 0;
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    if (Boolean.Parse(this.dataGridView1.Rows[i].Cells["RemoveIndex"].Value.ToString()))
                    {
                        if (im.Remomveindex(int.Parse(this.dataGridView1.Rows[i].Cells["ID"].Value.ToString())))
                            del++;
                        _obiekty.RemoveIndexIfExists(int.Parse(this.dataGridView1.Rows[i].Cells["ID"].Value.ToString()));
                    }
                }
                if (del > 0)
                    MessageBox.Show(string.Format("Usunieto pomyślnie {0} indeksów", del));

                button3_Click(null, null);
            }
            catch (Exception ex)
            {
                Log("IndexMechanismAdminConsole", string.Format("Podczas dodawania wystapił błąd\t{0}", ex.Message), MessageLevel.Error);
                MessageBox.Show("Usuwanie zakończyło się niepowodzeniem. Więcej informacji w logu");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Cells["RemoveIndex"].Value =
                !Boolean.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["RemoveIndex"].Value.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> index = new List<int>();

            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (Boolean.Parse(this.dataGridView1.Rows[i].Cells["RemoveIndex"].Value.ToString()))
                    index.Add(int.Parse(this.dataGridView1.Rows[i].Cells["ID"].Value.ToString()));
            }
            if (index.Count == 1)
            {
                //ObjectManager.TestData.TestDataSet testDataSet = ObjectManager.TestData.Generator.GetTestData();
                //bool done = im.IndexObjects(index.First(), testDataSet.Osoby.ToArray())
                //            && im.IndexObjects(index.First(), testDataSet.Studenci.ToArray())
                //            && im.IndexObjects(index.First(), testDataSet.Wykladowcy.ToArray());
                //MessageBox.Show(done
                //                    ? "zindeksowano pomyślnie"
                //                    : "indeksowanie się nie pwoiodło");

                //var obiekty =
                //    testDataSet.Osoby.Select(o => (ObjectManager.Id) o).Union(
                //        testDataSet.Studenci.Select(s => (ObjectManager.Id) s)).Union(
                //            testDataSet.Wykladowcy.Select(w => (ObjectManager.Id) w));

                //obiekty = from testOID in obiekty
                //          join indexedOID in im.GetIndexedObjects(index.First(),-1,0)
                //              on testOID.Id equals indexedOID
                //          select testOID;

                //_obiekty.AddObjectsInToIndex(index.First(), obiekty.ToArray());

                var typesAvailableToIndex = im.GetIndexingTypes(index.First());
                List<Type> getDataTypes = new List<Type>();
                /*
                if (typesAvailableToIndex.Contains(typeof(int)))
                    getDataTypes.AddRange(new Type[]
                                              {
                                                  typeof (ObjectManager.TestData.BaseIntClass),
                                                  typeof (ObjectManager.TestData.IntClass)
                                              });
                if (typesAvailableToIndex.Contains(typeof(string)))
                    getDataTypes.AddRange(new Type[] { typeof(ObjectManager.TestData.StringClass) });

                ObjectManager.TestData.TestDataSet set =
                    ObjectManager.TestData.Generator.GetTestData(getDataTypes.ToArray(),
                                                                 Convert.ToInt32(Math.Floor(numericUpDown1.Value)));
                im.ClearIndexedObjects(index.First());
                DateTime startTime = DateTime.Now;

                if (set.baseInty.Count > 0)
                {
                    MessageBox.Show(
                        im.IndexObjects(index.First(), set.baseInty.ToArray())
                            ? String.Format(
                                "zindeksowano pomyślnie {1} obiektów klasy {2} w czasie {0:c}",
                                DateTime.Now.Subtract(startTime), set.baseInty.Count,
                                set.baseInty.First().GetType().FullName)
                            : "indeksowanie się nie pwoiodło", "Wynik indeksowania obiektów");
                }
                if (set.inty.Count > 0)
                {
                    startTime = DateTime.Now;
                    MessageBox.Show(
                        im.IndexObjects(index.First(), set.inty.ToArray())
                            ? String.Format(
                                "zindeksowano pomyślnie {1} obiektów klasy {2} w czasie {0:c}",
                                DateTime.Now.Subtract(startTime), set.inty.Count,
                                set.inty.First().GetType().FullName)
                            : "indeksowanie się nie pwoiodło", "Wynik indeksowania obiektów");
                }
                if (set.stringi.Count > 0)
                {
                    startTime = DateTime.Now;
                    MessageBox.Show(
                        im.IndexObjects(index.First(), set.stringi.ToArray())
                            ? String.Format(
                                "zindeksowano pomyślnie {1} obiektów klasy {2} w czasie {0:c}",
                                DateTime.Now.Subtract(startTime), set.stringi.Count,
                                set.stringi.First().GetType().FullName)
                            : "indeksowanie się nie pwoiodło", "Wynik indeksowania obiektów");
                }
                var obiekty = set.inty.Select(oid => (ObjectManager.Id)oid)
                    .Union(set.baseInty.Select(oid => (ObjectManager.Id)oid))
                    .Union(set.stringi.Select(oid => (ObjectManager.Id)oid));

                obiekty = from testOID in obiekty
                          join indexedOID in im.GetIndexedObjects(index.First(), -1, 0)
                              on testOID.Id equals indexedOID
                          select testOID;
                
                _obiekty.AddObjectsInToIndexAndClearExisting(index.First(), obiekty.ToArray());
                */

            }
            else
            //{
                //for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                //{
                    //runTestSimulation();
                //}
            //}
                MessageBox.Show("Wybierz dokładnie jednen indeks którym zindeksować testowe dane");
        }


        private void runTestSimulation()
        {
            //if (typesAvailableToIndex.Contains(typeof (string)))
            //    getDataTypes.AddRange(new Type[] {typeof (ObjectManager.TestData.StringClass)});
/*
            ObjectManager.TestData.TestDataSet set;

            for (int toGen = 1; toGen <= 1000000000; toGen *= 10)
            {
                System.GC.Collect();
                set = ObjectManager.TestData.Generator.GetTestData(
                    new Type[] {typeof (ObjectManager.TestData.IntClass)}, toGen);

                foreach (int ix in new int[] {3, 2})
                {
                    //AddMessage(string.Format("actual ticks: {0}",DateTime.Now.Ticks),LogTypes.Error);
                    im.RebuildIndex(ix);
                    DateTime start;

                    //AddMessage("usage of memory",LogTypes.Error,System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString());
                    start = DateTime.Now;
                    im.IndexObjects(ix, set.inty.ToArray());
                    AddMessage(
                        String.Format("zaindeksowano {0} obiektów indeksem numer {1}", set.inty.Count,
                                      ix), LogTypes.Warning,
                        string.Format("{0} milisekund", DateTime.Now.Subtract(start).TotalMilliseconds));
                }
                //3 testowe uruchomienia wyszukiwania elementu
                for (int ic = 0; ic<3;ic ++)
                {
                    int objPosition = Math.Abs((int) DateTime.Now.Ticks%set.inty.Count);
                    if (objPosition < set.inty.Count / 2)
                        objPosition += set.inty.Count/2;

                    ObjectManager.TestData.IntClass toFind = set.inty[objPosition];
                    foreach (int ix in new int[] {3, 2})
                    {
                        DateTime start;
                        start = DateTime.Now;
                        int[] found = im.FindObjects(ix, toFind.GetType(), false, new string[] {"baseInt"},
                                                     new object[] {(object) toFind.baseInt},
                                                     new CompareType[] {CompareType.equal});
                        AddMessage(
                            String.Format("odnalezionio {0} elementów indexem {1}", found.Count(), ix),
                            LogTypes.Information,
                            string.Format("{0} milisekund",
                                          DateTime.Now.Subtract(start).TotalMilliseconds));

                        //startTicks = DateTime.Now.Ticks;
                        //List<int> forSearch = new List<int>();
                        //for (int i = 0; i < set.inty.Count; i++)
                        //{
                        //    if (set.inty[i].baseInt == toFind.baseInt)
                        //        forSearch.Add(set.inty[i].Id);
                        //}
                        //AddMessage("przeszukanie forem", LogTypes.Information,
                        //           string.Format("w czasie {0} ticków osiągneło wynik {1} obiektów",
                        //                         DateTime.Now.Ticks - startTicks, forSearch.Count()));
                    }
                }
            }*/
        }

        internal Guid[] CheckIndexForObjects(int indexID, Type type, bool complexExtension, string[] attribiutes,
                                            CompareType[] compareTypes, object[] values)
        {
            return im.FindObjects(indexID, type, complexExtension, attribiutes, values, compareTypes);
        }

        internal Guid[] CheckIndexForObjects(int indexID, Type type, bool complexExtension)
        {
            return im.FindObjects(indexID, type, complexExtension);
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns["IndexName"], ListSortDirection.Ascending);
        }
    }
}