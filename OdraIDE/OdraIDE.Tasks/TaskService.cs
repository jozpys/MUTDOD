using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using OdraIDE.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OdraIDE.Tasks
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(OdraIDE.Core.Services.Tasks.TaskService, typeof(ITaskService))]
    public class TaskService : ITaskService, IPartImportsSatisfiedNotification
    {
        public event TaskEventHandler Added;
        public event TaskEventHandler Removed;
        public event EventHandler Cleared;

        private List<Task> tasks = new List<Task>();
        private Dictionary<TaskType, int> taskCount = new Dictionary<TaskType, int>();

        [Import(CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
        private GridTasksPad gridTasksPad { get; set; }

        [Import(OdraIDE.Core.Services.File.FileService, typeof(IFileService))]
        private IFileService FileService { get; set; }

        [Import(typeof(TaskProviderManager))]
        private TaskProviderManager TaskProvider { get; set; }

        public void Add(Task task)
        {
            tasks.Add(task);
            if (!taskCount.ContainsKey(task.TaskType))
            {
                taskCount[task.TaskType] = 1;
            }
            else
            {
                taskCount[task.TaskType]++;
            }
            OnAdded(new TaskEventArgs(task));
        }

        public void Remove(Task task)
        {
            if (tasks.Contains(task))
            {
                tasks.Remove(task);
                taskCount[task.TaskType]--;
                OnRemoved(new TaskEventArgs(task));
            }
        }

        public void Clear()
        {
            taskCount.Clear();
            tasks.Clear();
            OnCleared(EventArgs.Empty);
        }

        public int GetCount(TaskType type)
        {
            if (!taskCount.ContainsKey(type))
            {
                return 0;
            }
            return taskCount[type];
        }

        public IEnumerable<Task> Tasks
        {
            get 
            {
                foreach (Task task in tasks)
                {
                    yield return task;
                }
            }
        }

        private void OnCleared(EventArgs e)
        {
            if (Cleared != null)
            {
                Cleared(null, e);
            }
        }

        private void OnAdded(TaskEventArgs e)
        {
            if (Added != null)
            {
                Added(null, e);
            }
        }

        private void OnRemoved(TaskEventArgs e)
        {
            if (Removed != null)
            {
                Removed(null, e);
            }
        }

        public void OnImportsSatisfied()
        {
            FileService.FileClosed += new EventHandler<FileEventArgs>(FileService_FileClosed);
        }

        private void FileService_FileClosed(object sender, FileEventArgs e)
        {
            OpenedFile file = sender as OpenedFile;

            IList<Task> toRemove = new List<Task>();

            foreach (Task task in Tasks)
            {
                if (file.FileName.Equals(task.FileName))
                {
                    toRemove.Add(task);
                }
            }

            foreach (Task task in toRemove)
            {
                Remove(task);
            }
        }

        public void RemoveAll(Predicate<Task> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            IList<Task> toRemove = new List<Task>();

            foreach (Task t in Tasks)
            {
                if (predicate(t))
                {
                    toRemove.Add(t);
                }
            }

            foreach (Task task in toRemove)
            {
                Remove(task);
            }
        }
    }
}
