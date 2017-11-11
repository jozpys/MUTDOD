using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.ComponentModel.Composition.Hosting;
using OdraIDE.Core.Exceptions;
using OdraIDE.Core.Services;
using System.Linq;

namespace OdraIDE.Core.Host
{

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, IPartImportsSatisfiedNotification
	{
		private CompositionContainer _container;

		/// <summary>
		/// Main WPF startup window for the application
		/// </summary>
		[Import(CompositionPoints.Host.MainWindow, typeof(Window))]
		public new Window MainWindow
		{
			get { return base.MainWindow; }
			set { base.MainWindow = value; }
		}

		[ImportMany(ExtensionPoints.Host.Styles, typeof(ResourceDictionary), AllowRecomposition = true)]
		private IEnumerable<ResourceDictionary> Styles { get; set; }

		[ImportMany(ExtensionPoints.Host.Views, typeof(ResourceDictionary), AllowRecomposition = true)]
		private IEnumerable<ResourceDictionary> Views { get; set; }

		[Import(Logging.LoggingService, typeof(ILoggingService))]
		public ILoggingService logger { get; set; }

		/// <summary>
		/// Import komend uruchamianych przy starcie aplikacji
		/// </summary>
		[ImportMany(ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand), AllowRecomposition = true)]
		private IEnumerable<IExecutableCommand> StartupCommands { get; set; }

		/// <summary>
		/// Import komend uruchamianych przy zamykaniu aplikacji
		/// </summary>
		[ImportMany(ExtensionPoints.Host.ShutdownCommands, typeof(IExecutableCommand), AllowRecomposition = true)]
		private IEnumerable<IExecutableCommand> ShutdownCommands { get; set; }

		/// <summary>
		/// Potrzebny do posortowania rozszerzen np. komend
		/// </summary>
		[Import(Services.Host.ExtensionService)]
		private IExtensionService ExtensionService { get; set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);

			try
			{
				// DON'T USE LOGGER HERE.  It's not composed yet.
				base.OnStartup(e);

				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Start();

				if (Compose())
				{
					stopWatch.Stop();

					// Now we can use logger
					logger.InfoWithFormat("Composition complete...({0} milliseconds)", stopWatch.ElapsedMilliseconds);

					logger.Info("Showing Main Window...");
					MainWindow.Show();
				}
			}
			catch (Exception ex)
			{
				LogException(ex, "Application startup error. See logs.");
			}
		}

		void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			LogException(ex, "Unexpected exception. See logs.");
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			if (ShutdownCommands != null)
			{
				// Uruchomienie komend przy zamykaniu
				IList<IExecutableCommand> commands = ExtensionService.Sort(ShutdownCommands);
				foreach (IExecutableCommand cmd in commands)
				{
					logger.Info("Running shutdown command " + cmd.ID + "...");
					try
					{
						cmd.Run();
					}
					catch (Exception ex)
					{
						logger.Error("Exception while running command " + cmd.ID, ex);
					}
					logger.Info("Shutdown command " + cmd.ID + " completed.");
				}

				Thread.Sleep(500); // Give threads and other parts of the app a bit of time to
			}

			if (_container != null)
			{
				_container.Dispose();
			}
		}

		/// <summary>
		/// Wczytuje wszystkie moduły aplikacji.
		/// </summary>
		/// <returns>True - jeśli powiodło się wczytywanie</returns>
		private bool Compose()
		{
			IDictionary<string, IList<string>> dllFiles = null;
			try
			{
				dllFiles = PluginChecker.Instance.ReadAndCheckPlugins();
			}
			catch (InvalidPluginException ex)
			{
				LogException(ex, ex.Message);
				return false;                
			}

			var exportFactoryProvider = new ExportFactoryProvider();
		
			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new DllDirectoryCatalog(PluginChecker.LIB_DIR, dllFiles[PluginChecker.LIB_DIR]));
			catalog.Catalogs.Add(new DllDirectoryCatalog(PluginChecker.EXT_DIR, dllFiles[PluginChecker.EXT_DIR]));

			_container = new CompositionContainer(catalog, exportFactoryProvider);
			exportFactoryProvider.SourceProvider = _container;

			try
			{
				_container.ComposeParts(this);
			}
			catch(Exception ex)
			{
				LogException(ex, "Application startup error. See logs.");
				return false;
			}
			return true;
		}

		private void LogException(Exception ex, string msg)
		{
			FileStream file = new FileStream("StartupErrors.log", FileMode.Append, FileAccess.Write);
			StreamWriter sw = new StreamWriter(file);
			sw.WriteLine("-------------------------------------------------<<<<< " + DateTime.Now + " >>>>>-------------------------------------------------");
			sw.WriteLine(DateTime.Now + ": Exception:");
			sw.WriteLine(ex.ToString());
			if (ex.GetType() == typeof(ReflectionTypeLoadException)) 
			{
				foreach (var item in ((ReflectionTypeLoadException)ex).LoaderExceptions)
				{
					sw.WriteLine(item.ToString());
				}
			}
			sw.WriteLine(">>>>-------------------------------------------------------------------------------------------------------------------------<<<<");
			sw.Close();
			MessageBox.Show(msg);
			Thread.Sleep(3000);
			Shutdown();
		}

		private bool m_startupCommandsRun = false;

		public void OnImportsSatisfied()
		{
			this.Resources.MergedDictionaries.Clear(); // in case of recompose
			logger.Info("Importing Styles...");
			foreach (ResourceDictionary r in Styles)
			{
				this.Resources.MergedDictionaries.Add(r);
			}
			logger.Info("Importing Views...");
			foreach (ResourceDictionary r in Views)
			{
				this.Resources.MergedDictionaries.Add(r);
			}

			if (!m_startupCommandsRun) // Don't run on recomposition
			{
				m_startupCommandsRun = true;
				// Run all the startup commands
				IList<IExecutableCommand> commands = ExtensionService.Sort(StartupCommands);
				foreach (IExecutableCommand cmd in commands)
				{
					logger.Info("Running startup command " + cmd.ID + "...");
					try
					{
						cmd.Run();
					}
					catch (Exception ex)
					{
						logger.Error("Exception while running command " + cmd.ID, ex);
					}
					logger.Info("Startup command " + cmd.ID + " completed.");
				}
			}            
		}


	}
}
