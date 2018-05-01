using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using IndexPlugin;

namespace IndexMechanism.IndexStorageManager
{
    internal class IndexStorageManager<T>
    {
        private static IndexStorageManager<T> _instance = null;

        protected static IndexStorageManager<T> Instance
        {
            get { return _instance ?? (_instance = new IndexStorageManager<T>()); }
        }

        private static string _storageDirectory
        {
            get
            {
                if (!Directory.Exists(CORE.Settings<T>.GetInstance().IndexesDataStorageDirectory))
                    Directory.CreateDirectory(CORE.Settings<T>.GetInstance().IndexesDataStorageDirectory);

                return CORE.Settings<T>.GetInstance().IndexesDataStorageDirectory;
            }
        }

        internal static IndexPlugin.IndexData GetIndexData(int index)
        {
            if (!File.Exists(String.Format("{0}{1}.dat", _storageDirectory, index)))
                return null;

            IndexPlugin.IndexData indexData = null;
            using (
                Stream stream =
                    File.Open(
                        String.Format("{0}{1}.dat", CORE.Settings<T>.GetInstance().IndexesDataStorageDirectory, index),
                        FileMode.Open))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                indexData = (IndexPlugin.IndexData)bFormatter.Deserialize(stream);
                stream.Close();
            }
            return indexData;

            //if (dane.ContainsKey(index))
            //    return dane[index];
            //else return null;
        }

        internal static void UpdateIndexData(int index, IndexPlugin.IndexData indexData)
        {
            using (Stream stream = File.Open(String.Format("{0}{1}.dat", _storageDirectory, index), FileMode.Create))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, indexData);
                stream.Close();
            }

            //if (dane.ContainsKey(index))
            //    dane[index] = indexData;
            //else dane.Add(index,indexData);
        }

       // private static Dictionary<int, IndexPlugin.IndexData> dane = new Dictionary<int, IndexData>();
    }
}