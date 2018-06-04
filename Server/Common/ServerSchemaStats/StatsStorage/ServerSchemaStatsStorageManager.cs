using MUTDOD.Common;
using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.ServerStats.StatsStorage
{
    public class ServerSchemaStatsStorageManager
    {
        private const string SearchPattern = "*.dat";
        private static ServerSchemaStatsStorageManager instance = null;

        protected static ServerSchemaStatsStorageManager Instance
        {
            get { return instance ?? (instance = new ServerSchemaStatsStorageManager()); }
        }

        private static string _storageDirectory
        {
            get
            {
                if (!Directory.Exists(Settings.GetInstance().ServerStatisticDataStorageDirectory))
                    Directory.CreateDirectory(Settings.GetInstance().ServerStatisticDataStorageDirectory);

                return Settings.GetInstance().ServerStatisticDataStorageDirectory;
            }
        }

        internal static StatisticData GetStatisticSchemaData(Did DatabaseId)
        {
            if (!File.Exists(String.Format("{0}{1}.dat", _storageDirectory, DatabaseId.Duid)))
                return null;

            StatisticData statisticData = null;
            using (
                Stream stream =
                    File.Open(
                        String.Format("{0}{1}.dat", Settings.GetInstance().ServerStatisticDataStorageDirectory, DatabaseId.Duid),
                        FileMode.Open))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                statisticData = (StatisticData)bFormatter.Deserialize(stream);
                stream.Close();
            }
            return statisticData;
        }

        internal static List<StatisticData> GetStatisticDataAllSchemas()
        {
            List<StatisticData> statisticDataAllSchemas = new List<StatisticData>();

            foreach (string FileName in Directory.GetFiles(_storageDirectory, SearchPattern))
            {
                try
                {
                    using (
                    Stream stream =
                        File.Open(FileName, FileMode.Open))
                    {
                        BinaryFormatter bFormatter = new BinaryFormatter();
                        statisticDataAllSchemas.Add((StatisticData)bFormatter.Deserialize(stream));
                        stream.Close();
                    }
                }
                catch(Exception ex)
                {
                    ServerSchemaStats.GetLoger().Log("ServerSchemaStatsStorageManager", string.Format("Unable to load SchemaStats from file: {0}", FileName), 
                                                     MessageLevel.Warning);
                }
            }
            return statisticDataAllSchemas;
        }

        internal static void UpdateStatisticSchemaData(Did DatabaseId, StatisticData statisticData)
        {
            using (Stream stream = File.Open(String.Format("{0}{1}.dat", _storageDirectory, DatabaseId.Duid), FileMode.Create))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, statisticData);
                stream.Close();
            }

        }
    }
}
