using System;
using System.ComponentModel;
using System.Windows.Forms;
using MUTDOD.Common;
using MUTDOD.Common.ServerBase;
using MUTDOD.Common.Settings;
using MUTDOD.Server.DataServer.DataServerBase;

namespace MTDOD.DataServerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted+= BwOnRunWorkerCompleted;
            textBox1.Enabled = false;
            bw.RunWorkerAsync();
        }

        private void BwOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            progressBar1.Visible = false;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            DSRunnableProgram.Register<ILogger, Logger>("guiLogAction", new Action<Logger.LogItem>(s => AddLog(s)));
            DSRunnableProgram.Init();
            Invoke((MethodInvoker) delegate
            {
                var runnable = DSRunnableProgram.Resolve<IServerRunnable>();
                this.Text = String.Format("MUTDOD Data Server: {0}", runnable.Name);
                textBox1.Text = runnable.Adress;
            });
        }

        private object _locker = new object();

        private void AddLog(Logger.LogItem logItem)
        {
            Invoke((MethodInvoker) delegate
            {
                {
                    lock (_locker)
                    {
                        dataGridView1.Rows.Add(string.Format("{0:yyyy:MM:dd-HH:mm:ss}", DateTime.Now),
                            logItem.MessageLevel, logItem.SenderName, logItem.Message);
                        dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0];
                    }
                }
            });
        }
    }
}
