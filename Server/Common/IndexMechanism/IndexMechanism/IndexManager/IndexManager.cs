using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using IndexMechanism.CORE;
using IndexPlugin;
using System.Threading;
using System.ComponentModel;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Indexing;

namespace IndexMechanism.IndexManager
{
    internal class IndexManager : IDisposable
    {
        private static IndexManager _instance = null;
        private List<IndexInfo> _indexes;
        private Dictionary<IndexInfo, IndexPlugin.IIndex> _indexesObjects;
        private AppDomain _pluginsDomain;
        private BackgroundWorker _statisticSaver;

        private string _indexesStoragePath
        {
            get
            {
                if (!Directory.Exists(CORE.Settings.GetInstance().IndexesStorageDirectory))
                    Directory.CreateDirectory(CORE.Settings.GetInstance().IndexesStorageDirectory);
                return CORE.Settings.GetInstance().IndexesStorageDirectory;
            }
        }

        public static IndexManager GetInstance()
        {
            return _instance ?? (_instance = new IndexManager());
        }

        private IndexManager()
        {
            LoadIndexes();
            _indexesObjects = new Dictionary<IndexInfo, IIndex>();
            _pluginsDomain = AppDomain.CreateDomain("pluginsDomain");
            _statisticSaver = new BackgroundWorker();
            _statisticSaver.DoWork += new DoWorkEventHandler(_statisticSaver_DoWork);
            _statisticSaver.WorkerSupportsCancellation = true;
            _statisticSaver.RunWorkerAsync();
        }

        private void _statisticSaver_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!e.Cancel)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Settings.GetInstance().StatisticSaveSecoundsInterval));
                foreach (IndexInfo index in _indexes)
                {
                    if (index.StatisticChanged)
                        index.saveStatistics();
                }
            }
        }

        private void LoadIndex(string path)
        {
            IndexInfo i;
            if ((i = IndexInfo.Load(path)) != null)
                if (i.IsValid())
                    _indexes.Add(i);
                else if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism", string.Format("Unable to load index plugin: {0}", i.IndexClassName), MessageLevel.Warning);
        }

        private void LoadIndexes()
        {
            _indexes = new List<IndexInfo>();
            foreach (string filePath in Directory.GetFiles(this._indexesStoragePath).Where(p => p.EndsWith("xml")))
                this.LoadIndex(filePath);

            if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",String.Format("Loaded {0} indexes", _indexes.Count),
                                                     MessageLevel.Info);
        }

        internal bool AddIndex(string dllPath)
        {
            if (!File.Exists(dllPath))
                return false;

            string newIndexFileName = dllPath.Split('\\').Last();
            int tries = 0;
            while (File.Exists(string.Format("{0}\\{1}", this._indexesStoragePath, newIndexFileName)))
                newIndexFileName = string.Format("{0}({1}).{2}", dllPath.Split('\\').Last().Split('.').First(), ++tries,
                                                 dllPath.Split('\\').Last().Split('.').Last());


            AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
            Assembly assembly = tempDomain.Load(AssemblyName.GetAssemblyName(dllPath));
            //Assembly assembly = Assembly.LoadFile(dllPath);
            bool ret = false;

            Type[] assemblyTypes;
            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism", string.Format("Iterating types in new index plugin assembly failed\n{0}", ex.LoaderExceptions.First().Message),
                                                         MessageLevel.Warning);
                return false;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism", string.Format("Iterating types in new index plugin assembly failed\n{0}", ex.Message),
                                                         MessageLevel.Warning);
                return false;
            }

            foreach (Type type in assemblyTypes)
            {
                if (type.IsClass && type.GetInterfaces().Contains(typeof (IndexPlugin.IIndex)))
                {
                    try
                    {
                        IndexPlugin.IIndex i = (IndexPlugin.IIndex) Activator.CreateInstance(type);

                        if (!i.EmptyIndexData.GetType().IsSerializable)
                            throw new Exception();

                        IndexInfo ixi = new IndexInfo
                                            {
                                                IndexFileName = newIndexFileName,
                                                IndexName = i.Name,
                                                IndexClassName = type.FullName,
                                                IndexID = CORE.Settings.GetInstance().NextIndexesIdentity
                                            };

                        if (IndexInfo.Save(ixi,
                                           string.Format("{0}\\{1}-{2}.xml", this._indexesStoragePath, type.GUID,
                                                         newIndexFileName)))
                        {
                            _indexes.Add(ixi);
                            ret = true;
                        }
                        i.Dispose();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (ret)
            {
                try
                {
                    File.Copy(dllPath, string.Format("{0}\\{1}", this._indexesStoragePath, newIndexFileName));
                    File.SetAttributes(string.Format("{0}\\{1}", this._indexesStoragePath, newIndexFileName),
                                       FileAttributes.Normal);
                    if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                        MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism", string.Format("Added new index plugin assembly: {0}", newIndexFileName),
                                                             MessageLevel.Info);
                }
                catch (Exception ex)
                {
                    if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                        MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism", string.Format("Unable to add new index plugin assebly {0}", dllPath),
                                                             MessageLevel.Error);
                    ret = false;
                }
            }

            AppDomain.Unload(tempDomain);
            return ret;
        }

        private void unloadDomain()
        {
            Monitor.Enter(_instance);

            foreach (KeyValuePair<IndexInfo, IIndex> indexesObject in _indexesObjects)
            {
                indexesObject.Value.Dispose();
            }

            AppDomain.Unload(_pluginsDomain);
            _pluginsDomain = AppDomain.CreateDomain("pluginsDomain");

            Monitor.Exit(_instance);
        }

        internal bool RemoveIndex(int indexIdToRemove)
        {
            IndexInfo toRemove;

            var i = _indexes.Where(p => p.IndexID == indexIdToRemove);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexIdToRemove);
            else
                toRemove = i.Single();

            if (!_indexes.Contains(toRemove))
                return false;

            if (_indexesObjects.ContainsKey(toRemove))
                this.unloadDomain();

            bool ret = _indexes.Remove(toRemove);

            try
            {
                if (_indexes.Where(p => p.IndexFileName == toRemove.IndexFileName).Count() == 0)
                    File.Delete(string.Format("{0}\\{1}", this._indexesStoragePath, toRemove.IndexFileName));
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",
                        string.Format("Removed index plugin assembly file {0}", toRemove.IndexFileName),
                        MessageLevel.Info);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",
                        string.Format("Unable to remove index plugin assembly file {0}\n{1}", toRemove.IndexFileName,ex),
                        MessageLevel.Error);
                ret = false;
            }

            ret = ret && toRemove.Remove();

            return ret;
        }

        public List<IndexInfo> GetIndexes()
        {
            IndexInfo[] ret = new IndexInfo[_indexes.Count];
            _indexes.CopyTo(ret);
            return ret.ToList();
        }

        internal IIndex GetIndex(int indexId)
        {
            IndexInfo index;

            var i = _indexes.Where(p => p.IndexID == indexId);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexId);
            else
                index = i.Single();

            if (_indexesObjects.ContainsKey(index))
                return _indexesObjects[index];

            Monitor.Enter(_instance);
            try
            {
                Assembly assembly =
                    _pluginsDomain.Load(
                        AssemblyName.GetAssemblyName(string.Format("{0}\\{1}", this._indexesStoragePath,
                                                                   index.IndexFileName)));

                Type[] assemblyTypes;
                try
                {
                    assemblyTypes = assembly.GetTypes();
                }
                catch (Exception ex)
                {
                    if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                        MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",
                            string.Format("Iterating types in index plugin {0} failed\n{1}", index.IndexFileName, ex.Message),
                            MessageLevel.Error);
                    Monitor.Exit(_instance);
                    return null;
                }

                var t = assemblyTypes
                    .Where(p => p.IsClass)
                    .Where(p => p.FullName == index.IndexClassName);

                if (t.Count() == 1)
                {
                    Type type = t.Single();
                    IndexPlugin.IIndex ixi = (IndexPlugin.IIndex) Activator.CreateInstance(type);
                    _indexesObjects.Add(index, ixi);
                    ixi.SettingsChanged += new settingsChangedHandler(IIndexSettingsChanged);
                    ixi.SetSettings(index.IndexSettings);
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",
                        string.Format("Unable to read index assembly {0} from file {1}\n{2}", index.IndexClassName,
                                      index.IndexFileName,ex), MessageLevel.Error);

                Monitor.Exit(_instance);
                return null;
            }

            Monitor.Exit(_instance);
            return _indexesObjects[index];
        }

        internal Type[] GetIndexingTypes(int indexId)
        {
            return GetIndex(indexId).AvailableIndexingTypes;
        }

        public void IIndexSettingsChanged(IIndex sender, string XML)
        {
            IndexInfo index = null;
            foreach (KeyValuePair<IndexInfo, IIndex> keyValuePair in _indexesObjects)
            {
                if (keyValuePair.Value.Equals(sender))
                    index = keyValuePair.Key;
            }
            if (index == null)
                return;

            index.IndexSettings = XML;
        }

        internal void SetIndexSettings(int indexId, string XML)
        {
            IndexInfo index;

            var i = _indexes.Where(p => p.IndexID == indexId);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexId);
            else
                index = i.Single();

            GetIndex(indexId).SetSettings(XML);
            index.IndexSettings = XML;
        }

        internal string GetIndexSettings(int indexId)
        {
            IndexInfo index;

            var i = _indexes.Where(p => p.IndexID == indexId);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexId);
            else
                index = i.Single();

            return index.IndexSettings;
        }

        internal void ResetStatistics(int indexId)
        {
            IndexInfo index;

            var i = _indexes.Where(p => p.IndexID == indexId);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexId);
            else
                index = i.Single();

            index.IndexStatistic.resetStstistics();
        }

        internal float GetStatistic(int indexId, IndexCostSource src, IndexCostType type, IndexCostInformation info,
                                    int? theoreticalIndexSize)
        {
            IndexInfo index;

            var i = _indexes.Where(p => p.IndexID == indexId);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexId);
            else
                index = i.Single();

            switch (src)
            {
                case IndexCostSource.Statistic:
                    return getStatistics(index, type, info);
                case IndexCostSource.Theoretic:
                    return getTheoretical(GetIndex(indexId), type, info, theoreticalIndexSize);
                default:
                    throw new UnexpectedStatisticSourceException(src);
            }
        }

        private float getStatistics(IndexInfo index, IndexCostType type, IndexCostInformation info)
        {
            StatisticInfo statisticInfo;

            switch (info)
            {
                case IndexCostInformation.OneObjectSearch:
                    statisticInfo = index.IndexStatistic.ObjectSearch;
                    break;
                case IndexCostInformation.OneObjectIndexAdd:
                    statisticInfo = index.IndexStatistic.ObjectIndexing;
                    break;
                case IndexCostInformation.OneObjectIndexRemove:
                    statisticInfo = index.IndexStatistic.ObjectIndexRemove;
                    break;
                case IndexCostInformation.OneObjectIndexRefresh:
                    statisticInfo = index.IndexStatistic.ObjectIndexRefresh;
                    break;
                case IndexCostInformation.HitRatio:
                    statisticInfo = index.IndexStatistic.HitRatio;
                    break;
                case IndexCostInformation.OneRoleIndexRefresh:
                    statisticInfo = index.IndexStatistic.RoleIndexRefresh;
                    break;
                case IndexCostInformation.OneRoleIndexRemove:
                    statisticInfo = index.IndexStatistic.RoleIndexRemove;
                    break;
                case IndexCostInformation.OneRoleIndexing:
                    statisticInfo = index.IndexStatistic.RoleIndexing;
                    break;
                case IndexCostInformation.OneRoleSearch:
                    statisticInfo = index.IndexStatistic.RoleSearch;
                    break;
                default:
                    throw new UnexpectedStatisticRequestException(info);
            }

            float ret;

            switch (type)
            {
                case IndexCostType.Optimistic:
                    ret = statisticInfo.Optimistic;
                    break;
                case IndexCostType.Pessimistic:
                    ret = statisticInfo.Pessimistic;
                    break;
                case IndexCostType.Average:
                    ret = statisticInfo.Average;
                    break;
                default:
                    throw new UnexpectedStatisticTypeException(type);
            }

            return ret;
        }

        private float getTheoretical(IIndex index, IndexCostType type, IndexCostInformation info,
                                     int? theoreticalIndexSize)
        {
            IndexOperationCost statisticInfo;

            switch (info)
            {
                case IndexCostInformation.OneObjectSearch:
                    statisticInfo = index.ObjectFindCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneObjectIndexAdd:
                    statisticInfo =
                        index.ObjectIndexingCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneObjectIndexRemove:
                    statisticInfo =
                        index.ObjectIndexRemoveCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneObjectIndexRefresh:
                    statisticInfo =
                        index.ObjectIndexRefreshCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneRoleIndexRefresh:
                    statisticInfo =
                        index.RoleIndexRefreshCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneRoleIndexRemove:
                    statisticInfo =
                        index.RoleIndexRemoveCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneRoleIndexing:
                    statisticInfo = index.RoleIndexingCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                case IndexCostInformation.OneRoleSearch:
                    statisticInfo = index.RoleFindCost(theoreticalIndexSize == null ? 0 : (int) theoreticalIndexSize);
                    break;
                default:
                    throw new UnexpectedStatisticRequestException(info);
            }
            int ret;

            switch (type)
            {
                case IndexCostType.Optimistic:
                    ret = statisticInfo.OptimisticCost;
                    break;
                case IndexCostType.Pessimistic:
                    ret = statisticInfo.PessimisticCost;
                    break;
                case IndexCostType.Average:
                    ret = statisticInfo.AverageCost;
                    break;
                default:
                    throw new UnexpectedStatisticTypeException(type);
            }

            return ret;
        }

        internal void includeInStatistics(int indexId, IndexCostInformation info, float value)
        {
            IndexInfo index;

            var i = _indexes.Where(p => p.IndexID == indexId);
            if (i.Count() != 1)
                throw new WronxIndexIdException(indexId);
            else
                index = i.Single();

            switch (info)
            {
                case IndexCostInformation.OneObjectIndexAdd:
                    index.IndexStatistic.includeObjectIndexed(value);
                    break;
                case IndexCostInformation.OneObjectIndexRefresh:
                    index.IndexStatistic.includeObjectIndexRefresh(value);
                    break;
                case IndexCostInformation.OneObjectIndexRemove:
                    index.IndexStatistic.includeObjectRemoved(value);
                    break;
                case IndexCostInformation.OneObjectSearch:
                    index.IndexStatistic.includeObjectSearch(value);
                    break;
                case IndexCostInformation.HitRatio:
                    index.IndexStatistic.includeHitRatio(value);
                    break;
                case IndexCostInformation.OneRoleIndexRefresh:
                    index.IndexStatistic.includeRoleIndexRefresh(value);
                    break;
                case IndexCostInformation.OneRoleIndexRemove:
                    index.IndexStatistic.includeRoleRemoved(value);
                    break;
                case IndexCostInformation.OneRoleIndexing:
                    index.IndexStatistic.includeRoleIndexed(value);
                    break;
                case IndexCostInformation.OneRoleSearch:
                    index.IndexStatistic.includeRoleSearch(value);
                    break;

                default:
                    throw new UnexpectedStatisticRequestException(info);
                    break;
            }

            if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger().Log("IndexMechanism",string.Format("new index statistic {0} with value {1}", info.ToString(), value), MessageLevel.Info);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _statisticSaver.CancelAsync();
        }

        #endregion
    }

    public class WronxIndexIdException : Exception
    {
        public WronxIndexIdException(int id) : base(String.Format("Unexpected index id: {0}", id))
        {
        }
    }

    public class UnexpectedStatisticRequestException : Exception
    {
        public UnexpectedStatisticRequestException(IndexCostInformation info)
            : base(String.Format("Unexpected index statistic requested: {0}", info.ToString()))
        {
        }
    }

    public class UnexpectedStatisticSourceException : Exception
    {
        public UnexpectedStatisticSourceException(IndexCostSource src)
            : base(String.Format("Unexpected index statistic source requested: {0}", src.ToString()))
        {
        }
    }

    public class UnexpectedStatisticTypeException : Exception
    {
        public UnexpectedStatisticTypeException(IndexCostType type)
            : base(String.Format("Unexpected index statistic type requested: {0}", type.ToString()))
        {
        }
    }
}