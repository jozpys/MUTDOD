using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using OdraIDE.Editor;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Editing;
using OdraIDE.Utilities;
using ICSharpCode.AvalonEdit.Document;

namespace OdraIDE.Tasks
{
	[Export(typeof(TaskProviderManager))]
	public class TaskProviderManager : IPartImportsSatisfiedNotification
	{
		[Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
		private IFileService FileService { get; set; }

		[ImportMany(typeof(ITaskProvider))]
		private IEnumerable<ITaskProvider> TaskProviders { get; set; }

		public void OnImportsSatisfied()
		{
			FileService.FileCreated += new EventHandler<FileEventArgs>(FileService_FileCreated);            
		}

		void FileService_FileCreated(object sender, FileEventArgs e)
		{
			OpenedFile file = sender as OpenedFile;
			file.DocumentInitialized += new EventHandler(file_DocumentInitialized);
		}

		void file_DocumentInitialized(object sender, EventArgs ea)
		{
			OpenedFile file = sender as OpenedFile;
			if (file.Document is ISourceEditor)
			{
				ISourceEditor editor = (ISourceEditor)file.Document;
				editor.TextEditor.TextArea.Document.TextChanged += (s, e) =>
				{
					Analyze(file);
					editor.TextEditor.Focus();
				};
				Analyze(file);
			}
			
		}

		private void Analyze(OpenedFile file)
		{
			foreach (ITaskProvider provider in TaskProviders)
			{
				provider.Analyze(file);
			}
		}

	}
}
