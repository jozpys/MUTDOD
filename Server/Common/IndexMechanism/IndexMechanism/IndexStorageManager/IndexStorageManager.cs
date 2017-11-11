using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using IndexPlugin;

namespace IndexMechanism.IndexStorageManager
{
    internal class IndexStorageManager
    {
        private static IndexStorageManager _instance = null;

        protected static IndexStorageManager Instance
        {
            get { return _instance ?? (_instance = new IndexStorageManager()); }
        }

        private static string _storageDirectory
        {
            get
            {
                if (!Directory.Exists(CORE.Settings.GetInstance().IndexesDataStorageDirectory))
                    Directory.CreateDirectory(CORE.Settings.GetInstance().IndexesDataStorageDirectory);

                return CORE.Settings.GetInstance().IndexesDataStorageDirectory;
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
                        String.Format("{0}{1}.dat", CORE.Settings.GetInstance().IndexesDataStorageDirectory, index),
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