using System;

namespace OdraIDE.Core
{
    public class Task
    {
        private string m_description;
        private TaskType m_type;
        private FileName m_filename;
        private int m_Column;
        private int m_Line;
        private int m_Length = 0;
        private bool m_Underline = true;

        public Task(FileName fileName, TaskType type, int line, int column, string description)
        {
            this.m_filename = fileName;
            this.m_type = type;
            this.m_Line = line;
            this.m_Column = column;
            this.m_description = description;
        }

        public Task(FileName fileName, TaskType type, int line, int column, int length, string description)
            : this(fileName, type, line, column, description)
        {
            this.m_Length = length;
        }

        public FileName FileName
        {
            get
            {
               return m_filename;
            }
        }

        public TaskType TaskType
        {
            get
            {
                return m_type;
            }
        }

        public int Line
        {
            get
            {
                return m_Line;
            }
        }

        public int Length
        {
            get
            {
                return m_Length;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }
        }

        public String Description
        { 
            get
            {
                return m_description;
            }
        }

        /// <summary>
        /// Default true
        /// </summary>
        public bool Underline
        {
            get
            {
                return m_Underline;
            }

            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException("Underline");
                }
                this.m_Underline = value;
            }
        }

    }

    public enum TaskType
    {
        Error,
        Warning,
        Message
    }

    public delegate void TaskEventHandler(object sender, TaskEventArgs e);

    public class TaskEventArgs : EventArgs
    {
        Task task;

        public Task Task
        {
            get
            {
                return task;
            }
        }

        public TaskEventArgs(Task task)
        {
            this.task = task;
        }
    }
}
