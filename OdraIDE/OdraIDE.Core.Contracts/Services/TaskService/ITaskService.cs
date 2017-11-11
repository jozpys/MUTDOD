using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core
{
    public interface ITaskService
    {
        event TaskEventHandler Added;
        event TaskEventHandler Removed;
        event EventHandler Cleared;

        void Add(Task task);
        void Remove(Task task);
        void Clear();
        void RemoveAll(Predicate<Task> predicate);

        int GetCount(TaskType type);

        IEnumerable<Task> Tasks { get; }
    }
}
