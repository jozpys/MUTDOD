using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.Storage.MetadataElements
{
    [Serializable]
    public class StorageMetadata
    {
        public ConcurrentDictionary<Did, IDatabaseParameters> Databases { get; set; }
        private uint _oli = 1000;
        public Oid NextOid
        {
            get
            {
                return new Oid(new Guid(), _oli);
            }
        }

        private StorageMetadata()
        {

        }

        public static void SaveMetadata(StorageMetadata metadata, ILogger logger)
        {
            try
            {
                using (Stream s = File.Open("metadata.bin", FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(s, metadata);
                }
                logger.Log("StorageMetadata", "StorageMetadata saved successfully", MessageLevel.Info);
            }
            catch (SerializationException ex)
            {
                logger.Log("StorageMetadata", string.Format("StorageMetadata not saved.{0}", ex),MessageLevel.Error);
            }
        }
        public static StorageMetadata ReadMetadata(ILogger logger)
        {
            StorageMetadata result = null;
            if (File.Exists("metadata.bin"))
            {
                try
                {
                    int i = 5;
                    do
                    {
                        try
                        {
                            using (Stream s = File.Open("metadata.bin", FileMode.Open, FileAccess.ReadWrite))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                result = bf.Deserialize(s) as StorageMetadata;
                            }
                            break;
                        }
                        catch (IOException)
                        {
                            if (--i <= 0)
                                throw;

                            logger.Log("StorageMetadata", "Reading metadata exception! Remaining atteprts: " + i,
                                MessageLevel.Warning);
                            Thread.Sleep(new TimeSpan(0, 0, 0, (5 - i)*2));
                        }
                    }
                    while (i > 0) ;
                    logger.Log("StorageMetadata", "StorageMetadata loaded successfully",MessageLevel.Info);
                    return result;
                }
                catch (SerializationException)
                {
                    logger.Log("StorageMetadata", "StorageMetadata not loaded. Creating new.",MessageLevel.Warning);
                    return new StorageMetadata() { Databases = new ConcurrentDictionary<Did, IDatabaseParameters>() };
                }
            }
            else
            {
                logger.Log("StorageMetadata", "StorageMetadata not loaded. Creating new.",MessageLevel.Warning);
                return new StorageMetadata() { Databases = new ConcurrentDictionary<Did, IDatabaseParameters>() };
            }

        }



    }
}