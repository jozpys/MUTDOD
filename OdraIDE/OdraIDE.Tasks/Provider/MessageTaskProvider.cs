using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using OdraIDE.Editor;
using OdraIDE.Utilities;
using ICSharpCode.AvalonEdit.Document;
using System.ComponentModel.Composition;

namespace OdraIDE.Tasks
{
	[Export(typeof(ITaskProvider))]
	public class MessageTaskProvider : ITaskProvider
	{
		[Import(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
		protected ITaskService TaskService { get; set; }

		private IList<Task> tasks = new List<Task>();

		private string[] keywords = new string[] 
		{ 
			"///TODO", "///FIXME" , "///HACK", "///XXX"
		};

		public void Analyze(OpenedFile file)
		{
			List<Task> toRemove = new List<Task>();
			foreach (Task t in tasks)
			{
				if (t.FileName.Equals(file.FileName))
				{
					TaskService.Remove(t);
					toRemove.Add(t);
				}
			}
			foreach (Task t in toRemove)
			{
				tasks.Remove(t);
			}

			ISourceEditor editor = (ISourceEditor)file.Document;
			string text = editor.SourceCode;


			StringSearch search = new StringSearch(keywords);
			StringSearchResult[] results = search.FindAll(text);

			foreach (StringSearchResult ssr in results)
			{
				DocumentLine docLine = editor.TextEditor.Document.GetLineByOffset(ssr.Index);
				int line = docLine.LineNumber;
				int column = ssr.Index - docLine.Offset + 1;
				int length = docLine.Length - column + 1;
				string description = editor.TextEditor.Document.GetText(ssr.Index, length).Remove(0, 3);
				Task task = new Task(file.FileName, TaskType.Message, line, column, length, description);
				task.Underline = false;
				tasks.Add(task);
				TaskService.Add(task);
			}
		}
	}
}
