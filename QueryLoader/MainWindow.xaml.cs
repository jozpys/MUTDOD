using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace QueryLoader
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly String mainPath = Path.GetFullPath(Directory.GetCurrentDirectory());
        private readonly String analizerPath = "/Server/Common/EBNFQueryAnalyzer";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FileNameButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".g",
                Filter = "Grammar (.g)|*.g"
            };

            dlg.InitialDirectory = mainPath;

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                FileNameText.Text = filename;
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadProgress.Dispatcher.Invoke(() => LoadProgress.Value = 0, DispatcherPriority.Background);

            if (!File.Exists(FileNameText.Text))
            {
                ResultLabel.Content = "Brak pliku!";
                return;
            }
            File.Copy(FileNameText.Text, mainPath + analizerPath + "/ QueryGrammar.g", true);
            LoadProgress.Dispatcher.Invoke(() => LoadProgress.Value = 30, DispatcherPriority.Background);

            bool compileResult = CompileProject();
            if (!compileResult)
            {
                ResultLabel.Content = "Bład podczas ładowania pliku! Więcej informacji w logu.";
                return;
            }
            LoadProgress.Dispatcher.Invoke(() => LoadProgress.Value = 70, DispatcherPriority.Background);

            File.Copy(mainPath + analizerPath + "/bin/Debug/MUTDOD.Server.Common.EBNFQueryAnalyzer.dll", mainPath + "/bin/Debug/MUTDOD.Server.Common.EBNFQueryAnalyzer.dll", true);
            LoadProgress.Dispatcher.Invoke(() => LoadProgress.Value = 100, DispatcherPriority.Background);

            ResultLabel.Content = "Gramatyka załadowana poprawnie";

        }

        private bool CompileProject()
        {
            var buildFile = mainPath + analizerPath + "/EBNFQueryAnalyzer.csproj";

            List<ILogger> loggers = new List<ILogger>();
            var logger = new FileLogger();
            String logfile = "logfile=" + mainPath + "/QueryLogger.log";
            logger.Parameters = logfile;
            loggers.Add(logger);
            var projectCollection = new ProjectCollection();
            projectCollection.RegisterLoggers(loggers);
            var project = projectCollection.LoadProject(buildFile, "4.0");
            try
            {
                return project.Build();
            }
            finally
            {
                projectCollection.UnregisterAllLoggers();
            }
        }
    }
}
