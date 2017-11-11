using System;
using System.IO;
using MUTDOD.Common.Settings;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage.Core.Metadata
{
    public interface IDatabaseParameters
    {
        string BaseDirectory { get; }
        string Name { get; }
        string DataFileFullPath { get; }
        Int64 StartupSize { get; }
        Int32 IncreaseFactor { get; }
        IDatabaseSchema Schema { get; set; }
        Did DatabaseId { get; set; }
        int PageSize { get; }
        bool OptimizeSize { get; }
    }

    [Serializable]
    public class DatabaseParameters : IDatabaseParameters
    {
        public DatabaseParameters(string name, ISettingsManager settingsManager)
        {
            Name = name;
            BaseDirectory = settingsManager.DataBaseDirectory;
        }

        public string BaseDirectory { get; private set; }
        public string Name { get; private set; }

        public string DataFileFullPath
        {
            get { return Path.Combine(BaseDirectory, string.Format("{0}.bin", Name)); }
        }

        public long StartupSize { get; set; }
        public int IncreaseFactor { get; set; }
        public IDatabaseSchema Schema { get; set; }

        public Did DatabaseId { get; set; }

        public int PageSize { get; set; }

        public bool OptimizeSize { get; set; }
    }
}