using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using AvalonDock;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows;
using System.Xml.Serialization;
using System.Windows.Controls;
using System.ComponentModel;
using System.Xml;
using OdraIDE.Utilities;

namespace OdraIDE.Core.Layout
{
	[Export(Services.Layout.LayoutManager, typeof(ILayoutManager))]
	public class LayoutManager : ILayoutManager
	{
		private ResizingPanel m_mainPanel = new ResizingPanel();

		private ResizingPanel m_topPanel = new ResizingPanel();
		private DockablePane m_topLeftPane = new DockablePane(); // top left pads are docked to this
		private DocumentPane m_docPane = new DocumentPane(); // documents are put in here
		private DockablePane m_topRightPane = new DockablePane(); // top right pads are docked to this

		private DockablePane m_bottomPane = new DockablePane(); // bottom pads are docked to this

		//private ResizingPanel m_resizingPanel = new ResizingPanel(); // pads are docked to this

		[Import(OdraIDE.Core.CompositionPoints.Host.MainWindow, typeof(Window))]
		private Lazy<Window> mainWindow { get; set; }

		[Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
		private Lazy<IFileService> fileService { get; set; }

		public LayoutManager()
		{
			// Basic configuration:
			// \- ResizingPanel - main panel
			//  |- ResizingPanel - top panel
			//     |- DockablePane - left pane
			//     |- DocumentPane - documents pane
			//     \- DockablePane - right pane
			//  \- DockablePane - bottom pane

			m_mainPanel.Name = "MainPanel";

			m_topPanel.Name = "TopPanel";
			m_topLeftPane.Name = "TopLeftPane";
			m_docPane.Name = "DocumentPane";
			m_topRightPane.Name = "TopRightPane";

			m_bottomPane.Name = "BottomPane";

			m_mainPanel.Orientation = Orientation.Vertical;
			m_topPanel.Orientation = Orientation.Horizontal;

			ResizingPanel.SetResizeHeight(m_topPanel, new GridLength(70, GridUnitType.Star));
			ResizingPanel.SetResizeHeight(m_bottomPane, new GridLength(30, GridUnitType.Star));

			m_mainPanel.Children.Add(m_topPanel);
			m_mainPanel.Children.Add(m_bottomPane);

			ResizingPanel.SetResizeWidth(m_topLeftPane, new GridLength(20, GridUnitType.Star));
			ResizingPanel.SetResizeWidth(m_docPane, new GridLength(60, GridUnitType.Star));
			ResizingPanel.SetResizeWidth(m_topRightPane, new GridLength(20, GridUnitType.Star));

			m_topPanel.Children.Add(m_topLeftPane);
			m_topPanel.Children.Add(m_docPane);
			m_topPanel.Children.Add(m_topRightPane);

			DockManager.Content = m_mainPanel;

			DockManager.Loaded += new System.Windows.RoutedEventHandler(DockManager_Loaded);
			DockManager.LayoutUpdated += new EventHandler(DockManager_LayoutUpdated);

		}

		/// <summary>
		/// Shows a pad.  If it hasn't been shown before, it shows it
		/// docked to the right side.  Otherwise it restores it to the
		/// previous place that it was before hiding.  Doesn't work
		/// correctly for floating panes (yet).
		/// </summary>
		/// <param name="pad"></param>
		public void ShowPad(IPad pad, DockableContentState desideredState = DockableContentState.Docked)
		{
			if (!m_padLookup.ContainsKey(pad))
			{
				DockableContent content = new DockableContent();
				content.Content = pad;
				content.Title = pad.Title;
				content.Name = pad.Name;
				content.Icon = pad.Icon;
				if (pad.DesideredState == DockableContentState.AutoHide)
				{
					content.Loaded += new RoutedEventHandler(AutoHideAfterLoaded);
				}
				m_padLookup.Add(pad, content);
				DockablePane dp;
				switch (pad.Location)
				{
					case PadLocation.TopLeft:
						dp = m_topLeftPane;
						break;
					case PadLocation.TopRight:
						dp = m_topRightPane;
						break;
					case PadLocation.Bottom:
					default:
						dp = m_bottomPane;
						break;
				}
				dp.Items.Add(content);
				
				content.GotFocus += new RoutedEventHandler(pad.OnGotFocus);
				content.LostFocus += new RoutedEventHandler(pad.OnLostFocus);
			}
			if (desideredState == DockableContentState.AutoHide ||
				desideredState == DockableContentState.FloatingWindow)
			{
				DockManager.Show(m_padLookup[pad], desideredState);
			}
			else
			{
				DockManager.Show(m_padLookup[pad]);
			}
			
		}

		void AutoHideAfterLoaded(object sender, RoutedEventArgs e)
		{
			DockableContent content = sender as DockableContent;
			DockManager.Show(content, DockableContentState.AutoHide);
		}

		/// <summary>
		/// Hides the given pad, if it exists
		/// </summary>
		/// <param name="pad"></param>
		public void HidePad(IPad pad)
		{
			if (m_padLookup.ContainsKey(pad))
			{
				DockManager.Hide(m_padLookup[pad]);
			}
		}

		public void HideAllPads()
		{
			foreach (var content in m_padLookup.Values)
			{
				DockManager.Hide(content);
			}
		}

		/// <summary>
		/// Shows a document.  Puts it in the document pane.
		/// </summary>
		/// <param name="document"></param>
		public IDocument ShowDocument(IDocument document, bool switchToOpenedDocument)
		{
			if (document != null)
			{
				if (!m_documentLookup.ContainsKey(document))
				{
					DocumentContent content = new DocumentContent();
					content.Content = document;
					content.Title = document.Title;
					document.PropertyChanged += (sender, args) => 
					{
						if (args.PropertyName.Equals("Title"))
						{
							content.Title = (sender as IDocument).Title;
						}
					};
					content.Name = document.Name;
					m_documentLookup.Add(document, content);
					m_docPane.Items.Add(content);
					content.Closing += new EventHandler<CancelEventArgs>(document.OnClosing);
					//content.Closing += document.File.FileClosed;
					content.Closed += new EventHandler(content_Closed);
					content.GotFocus += new RoutedEventHandler(document.OnGotFocus);
					content.LostFocus += new RoutedEventHandler(document.OnLostFocus);
					document.OnOpened(content, new EventArgs());
				}
				if (switchToOpenedDocument)
				{
					DockManager.Show(m_documentLookup[document]);
				}
			}
			return document;
		}

		/// <summary>
		/// Closes the given instance of a document, if it exists
		/// </summary>
		/// <param name="document"></param>
		public void CloseDocument(IDocument document)
		{
			if (m_documentLookup.ContainsKey(document))
			{
				m_documentLookup[document].Close();
			}
		}

		/// <summary>
		/// Close all documents, if they are open
		/// </summary>
		public void CloseAllDocuments()
		{
			while (m_documentLookup.Count > 0)
			{
				IDocument doc = m_documentLookup.Keys.First();
				m_documentLookup[doc].Close();
			}
		}

		// Handles removing documents from the data structure when closed
		void content_Closed(object sender, EventArgs e)
		{
			DocumentContent content = sender as DocumentContent;
			IDocument document = content.Content as IDocument;
			m_documentLookup.Remove(document);
			document.OnClosed(sender, e);
		}

		public DocumentContent GetDocumentContent(IDocument document)
		{
			return m_documentLookup[document];
		}

		/// <summary>
		/// The View binds to this property
		/// </summary>
		public DockingManager DockManager
		{
			get
			{
				return m_Content;
			}
		}
		private readonly DockingManager m_Content = new DockingManager();

		#region " ILayoutManager Members "

		public event EventHandler Loaded;
		public event EventHandler LayoutUpdated;

		/// <summary>
		/// Pass through the LayoutUpdated Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void DockManager_LayoutUpdated(object sender, EventArgs e)
		{
			if (LayoutUpdated != null)
			{
				LayoutUpdated(sender, e);
			}
		}

		/// <summary>
		/// Pass through the Loaded Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void DockManager_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			if (Loaded != null)
			{
				Loaded(sender, new EventArgs());
			}
		}

		/// <summary>
		/// A collection of tool pads, etc., from the workbench
		/// </summary>
		public ReadOnlyCollection<IPad> Pads
		{
			get
			{
				return new ReadOnlyCollection<IPad>(m_padLookup.Keys.ToList());
			}
		}
		private readonly Dictionary<IPad, DockableContent> m_padLookup = new Dictionary<IPad, DockableContent>();

		/// <summary>
		/// A collection of documents from the document manager
		/// </summary>
		public ReadOnlyCollection<IDocument> Documents
		{
			get
			{
				return new ReadOnlyCollection<IDocument>(m_documentLookup.Keys.ToList());
			}
		}
		private readonly Dictionary<IDocument, DocumentContent> m_documentLookup = new Dictionary<IDocument, DocumentContent>();

		/// <summary>
		/// Returns true if the given pad is visible.
		/// </summary>
		/// <param name="pad"></param>
		/// <returns></returns>
		public bool IsVisible(IPad pad)
		{
			if (m_padLookup.ContainsKey(pad))
			{
				DockableContent content = m_padLookup[pad];
				return (content.State != DockableContentState.Hidden);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns true if the given document is visible.
		/// </summary>
		/// <param name="document"></param>
		/// <returns></returns>
		public bool IsVisible(IDocument document)
		{
			if (m_documentLookup.ContainsKey(document))
			{
				DocumentContent content = m_documentLookup[document];
				return content.IsActiveDocument;
			}
			else
			{
				return false;
			}
		}

		// Set this to the list of all available pads and docs so that RestoreLayout
		// can use them to build new ones as needed.
		public void SetAllPadsDocuments(
			IEnumerable<Lazy<IPad, IPadMeta>> AllPads,
			IEnumerable<Lazy<IDocument, IDocumentMeta>> AllDocuments)
		{
			allPads = AllPads;
			allDocuments = AllDocuments;
		}
		private IEnumerable<Lazy<IPad, IPadMeta>> allPads = new Collection<Lazy<IPad, IPadMeta>>();
		private IEnumerable<Lazy<IDocument, IDocumentMeta>> allDocuments = new Collection<Lazy<IDocument, IDocumentMeta>>();

		/// <summary>
		/// Call this method to persist the current layout to disk.
		/// </summary>
		public void SaveLayout(StreamWriter sw)
		{
			if (DockManager.IsLoaded)
			{
				// Save pads
				List<string> padNamesList = new List<string>();
				foreach (IPad pad in Pads)
				{
					// We have to save all the pad names that have ever been
					// shown even if they're hidden now or else the layout
					// manager won't remember where they are when shown again.
					padNamesList.Add(pad.Name);
				}

				string padNames = String.Join(",", padNamesList.ToArray());

				XmlDocument document = new XmlDocument();
				XmlNode infoNode = document.CreateXmlDeclaration("1.0", "UTF-8", null);
				document.AppendChild(infoNode);

				XmlNode rootNode = document.CreateElement("Settings");

				//Save layout
				StringWriter swLayout = new StringWriter();
				DockManager.SaveLayout(swLayout);
				string layout = swLayout.ToString();
				XmlReader xmlReader = XmlReader.Create(new StringReader(layout));

				XmlNode layoutNode = document.CreateElement("Layout");
				XmlNode dmNode = document.ReadNode(xmlReader);
				layoutNode.AppendChild(dmNode);
				rootNode.AppendChild(layoutNode);

				// Save documents
				DocumentList docNamesList = new DocumentList();
				foreach (IDocument doc in Documents)
				{
					//only saved docs
					if (!doc.File.IsUntitled)
					{
						docNamesList.AddItem(new DocumentItem(doc.Name, doc.File.FileName));
					}
				}

				if (docNamesList.Items.Length > 0)
				{
					XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
					ns.Add("", "");

					XmlSerializer s = new XmlSerializer(typeof(DocumentList));
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.OmitXmlDeclaration = true;
					StringWriter swDocs = new StringWriter();
					XmlWriter xmlDocs = XmlWriter.Create(swDocs, settings);
					s.Serialize(xmlDocs, docNamesList, ns);

					string documents = swDocs.ToString();
					xmlReader = XmlReader.Create(new StringReader(documents));
					XmlNode docsNode = document.ReadNode(xmlReader);
					XmlNode documentsNode = document.CreateElement("Documents");
					documentsNode.AppendChild(docsNode);
					rootNode.AppendChild(documentsNode);
				}

				document.AppendChild(rootNode);
				document.Save(sw);
			}
			else
			{
				throw new InvalidOperationException("The DockManager isn't loaded yet.");
			}
		}

		/// <summary>
		/// Call this method to restore the existing layout from disk.
		/// </summary>
		public void RestoreLayout(StreamReader sr)
		{
			//if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
			//{
			//    return;
			//}

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(sr);
			}
			catch (XmlException)
			{
				return;
			}

			//Read and restore layout
			XmlNode layoutNode = doc.GetElementsByTagName("Layout").Item(0);
			if (layoutNode != null)
			{
				TextReader layoutTR = new StringReader(layoutNode.InnerXml);
				DockManager.RestoreLayout(layoutTR);
			}

			//Read and restore Documents
			XmlNode docListNode = doc.GetElementsByTagName("Documents").Item(0);
			if (docListNode != null)
			{
				DocumentList newList;
				XmlSerializer s = new XmlSerializer(typeof(DocumentList));
				TextReader r = new StringReader(docListNode.InnerXml);
				try
				{
					newList = (DocumentList)s.Deserialize(r);
				}
				catch (InvalidOperationException)
				{
					newList = null;
				}
				finally
				{
					r.Close();
				}

				if (newList != null)
				{
					foreach (DocumentItem item in newList.Items)
					{
						try
						{
							fileService.Value.OpenFile(item.memento);
						}
						catch (FileNotFoundException e)
						{
							//todo
						}

					}
				}
			}
		}
		#endregion

		public IDocument GetActiveDocument()
		{
			if (DockManager.ActiveDocument == null)
			{
				return null;
			}
			return DockManager.ActiveDocument.Content as IDocument;
		}

		public void SetMainWindowTitle(string title)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{
				mainWindow.Value.Title = title + " - OdraIDE";
			}
			
		}
	}
}
