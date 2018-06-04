using System;
using System.ServiceModel.Channels;

namespace MUTDOD.Common.Settings
{
    public interface ISettingsManager
    {
        string CentralServerRemoteAdress { get; }
        Binding CentralServerRemoteBinding { get; }
        StorageStrategy StorageStrategy { get; }
        string DataBaseDirectory { get; }
        string LogFileDirectory { get; }
        int MaxNumberObjectsFullScan { get; }
    }

    [Serializable]
    public enum StorageStrategy
    {
        Speed = 0,
        Esent = 1,
        MemoryMappedFiles = 2
    }
}
