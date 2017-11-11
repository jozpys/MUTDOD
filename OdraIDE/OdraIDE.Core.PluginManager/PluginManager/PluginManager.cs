using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OdraIDE.Utilities;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using Xstream.Core;
using OdraIDE.Core.Exceptions;

namespace OdraIDE.Core.PluginManager
{
	[Export(OdraIDE.Core.PluginManager.CompositionPoints.PluginManager.PluginManagerDialog, typeof(PluginManager))]
	public class PluginManager : AbstractExtension ,IPartImportsSatisfiedNotification
	{
		[Import(Services.Logging.LoggingService, typeof(ILoggingService))]
		private ILoggingService logger { get; set; }

		[Import(OdraIDE.Core.CompositionPoints.Host.MainWindow)]
		private Lazy<Window> mainWindowExport { get; set; }

		[Import(OdraIDE.Core.Services.Messaging.MessagingService, typeof(IMessagingService))]
		private IMessagingService messagingService { get; set; }

		[Import(OdraIDE.Core.Services.FileDialog.FileDialogService, typeof(IFileDialogService))]
		private IFileDialogService fileDialogService { get; set; }

		public string PLUGIN_DIR = Directory.GetCurrentDirectory() + @"\Plugins";
		public string PLUGIN_BIN_DIR = Directory.GetCurrentDirectory() + @"\Plugins\bin";

		/// <summary>
		/// Displays the Plugin Manager Dialog as modal
		/// </summary>
		public void ShowDialog()
		{
			Window mainWindow = mainWindowExport.Value;
			Window pluginManagerDialog = new PluginManagerView();
			pluginManagerDialog.Owner = mainWindow;
			pluginManagerDialog.DataContext = this;
			logger.Info("Showing plugin manager dialog...");
			pluginManagerDialog.ShowDialog();
			logger.Info("Plugin manager closed.");
		}

		public void OnImportsSatisfied()
		{
			string dir = PLUGIN_BIN_DIR;
			foreach (var p in PluginChecker.Instance.GetExtPlugins())
			{
				p.EnabledChanged += OnPluginEnabledChanged;
				m_Plugins.Add(p);
			}
		}

		//private PluginInfo ReadDllInfo(string filePath, bool enabled)
		//{
		//    Assembly a = Assembly.ReflectionOnlyLoadFrom(filePath);

		//    PluginInfo plugin = new PluginInfo(enabled);
		//    plugin.EnabledChanged += new EventHandler(OnPluginEnabledChanged);
		//    plugin.FilePath = Path.Combine(PLUGIN_DIR, Path.GetFileName(filePath));
		//    plugin.Name = a.GetName().Name;
		//    plugin.Version = a.GetName().Version.ToString();
		//    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
		//    IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(a);

		//    foreach (CustomAttributeData data in attributes)
		//    {
		//        Type t = data.Constructor.ReflectedType;
		//        if (t == typeof(AssemblyCopyrightAttribute))
		//            plugin.Copyright = data.ConstructorArguments[0].Value.ToString();

		//        if (t == typeof(AssemblyCompanyAttribute))
		//            plugin.Company = data.ConstructorArguments[0].Value.ToString();

		//        if (t == typeof(AssemblyDescriptionAttribute))
		//            plugin.Description = data.ConstructorArguments[0].Value.ToString();
		//    }
		//    return plugin;
		//}

		private void OnPluginEnabledChanged(object sender, EventArgs e)
		{
			Plugin p = sender as Plugin;
			string msg = "Plugin " + p.Name + " " + (p.Enabled ? "enabled" : "disabled") + ", you must restart the application to apply the change.";
			messagingService.ShowDialog(msg, "Successful", MessageBoxButtons.OK);
		}

		//Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
		//{
		//    return Assembly.ReflectionOnlyLoad(args.Name);
		//}

		private ObservableCollection<Plugin> m_Plugins = new ObservableCollection<Plugin>();

		public ObservableCollection<Plugin> Plugins { get { return m_Plugins; } }

		private Plugin m_SelectedPlugin;

		public Plugin SelectedPlugin 
		{
			get
			{
				return m_SelectedPlugin;
			}

			set
			{
				m_dirtyCondition.SetCondition(value != null);
				if (m_SelectedPlugin != value)
				{
					m_SelectedPlugin = value;
					NotifyPropertyChanged(m_SelectedPluginArgs);
				}
			}
		}

		static readonly PropertyChangedEventArgs m_SelectedPluginArgs =
			NotifyPropertyChangedHelper.CreateArgs<PluginManager>(o => o.SelectedPlugin);

		private ICondition dirtyCondition
		{
			get
			{
				return m_dirtyCondition;
			}
		}
		private ConcreteCondition m_dirtyCondition = new ConcreteCondition(false);

		#region Add plugin button

		public IControl AddButton
		{
			get
			{
				if (m_AddButton == null)
				{
					m_AddButton = new AddPluginButton(this);
				}
				return m_AddButton;
			}
		}
		private IControl m_AddButton = null;

		private class AddPluginButton : AbstractButton
		{
			public AddPluginButton(PluginManager pm)
			{
				m_PluginManager = pm;
			}

			private PluginManager m_PluginManager = null;

			protected override void Run()
			{
				m_PluginManager.AddPlugin();
			}
		}

		public void AddPlugin()
		{
			bool addExtension = true;
			bool checkFileExists = true;
			bool checkPathExists = true;

			Dictionary<string, string> filters = new Dictionary<string, string>();
			filters.Add("plugin", "Plugin Files");

			string pluginFileName = fileDialogService.OpenFileDialog(
					"plugin", @"d:\", filters, //TODO bieżący katalog
					"Add plug-in",
					addExtension, checkFileExists, checkPathExists);

			if (pluginFileName != null)
			{
				Plugin plugin = null;
				try
				{
					plugin = Plugin.Read(pluginFileName);
					PluginChecker.Instance.CheckPlugin(plugin);
					PluginChecker.Instance.CheckPluginDependecies(plugin);
				}
				catch (InvalidPluginException ex)
				{
					messagingService.ShowMessage(ex.Message, "Error");
					return;
				}

				if (!Directory.Exists(PLUGIN_DIR))
				{
					Directory.CreateDirectory(PLUGIN_DIR);
				}

				string dllFileName = plugin.FilePath(Path.GetDirectoryName(pluginFileName));
				if (!File.Exists(dllFileName))
				{
					messagingService.ShowMessage("Plugin's dll doesn't exist [Path=" + dllFileName + "]", "Error");
					return;
				}

				string dllDestination = Path.Combine(PLUGIN_DIR, Path.GetFileName(dllFileName));
				string pluginDestination = Path.Combine(PLUGIN_DIR, Path.GetFileName(pluginFileName));

				if (File.Exists(dllDestination))
				{
					messagingService.ShowMessage("Plugin with this name exists. Add another or try again.", "Warning");
					return;
				}

				try
				{
					File.Copy(dllFileName, dllDestination);
					File.Copy(pluginFileName, pluginDestination);
				}
				catch (UnauthorizedAccessException)
				{
					messagingService.ShowMessage("Unauthorized access. Run program as Administrator.", "Error");
					return;
				}
				catch (Exception)
				{
					messagingService.ShowMessage("Adding new plugin failed. Restart application and try again.", "Error");
					return;
				}

				messagingService.ShowDialog("Update complete, you must restart the application to apply the update.", "Update complete", MessageBoxButtons.OK);
				
				plugin.Loaded = false;
				PluginChecker.Instance.AddPlugin(plugin);
				Plugins.Add(plugin);
				SelectedPlugin = plugin;
			}
		}

		#endregion

		#region Remove plugin button

		public IControl RemoveButton
		{
			get
			{
				if (m_RemoveButton == null)
				{
					m_RemoveButton = new RemovePluginButton(this);
				}
				return m_RemoveButton;
			}
		}
		private IControl m_RemoveButton = null;

		private class RemovePluginButton : AbstractButton
		{
			public RemovePluginButton(PluginManager pm)
			{
				m_PluginManager = pm;
				EnableCondition = pm.dirtyCondition;
			}

			private PluginManager m_PluginManager = null;

			protected override void Run()
			{
				m_PluginManager.RemoveSelectedPlugin();
			}
		}

		public void RemoveSelectedPlugin()
		{
			DialogResult result = messagingService.ShowDialog("Do you really want to delete the selected plug-in?", "Delete plug-in?",
				MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				try
				{
					File.Delete(SelectedPlugin.FilePath(PLUGIN_DIR));
					File.Delete(Path.Combine(SelectedPlugin.Directory, SelectedPlugin.Config.File + ".plugin"));
				}
				catch (UnauthorizedAccessException)
				{
					messagingService.ShowMessage("Unauthorized access. Run program as Administrator.", "Error");
					return;
				}
				catch (Exception)
				{
					messagingService.ShowMessage("Deleting plugin failed. Restart application and try again.", "Error");
					return;
				}

				messagingService.ShowDialog("Update complete, do you want to restart the application to apply the update.",
						"Update complete", MessageBoxButtons.OK);

				Plugins.Remove(SelectedPlugin);
				PluginChecker.Instance.RemovePlugin(SelectedPlugin);
			}
		}

		#endregion
	}
}
