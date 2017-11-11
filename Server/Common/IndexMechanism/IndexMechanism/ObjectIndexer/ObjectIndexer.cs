using System;
using System.Collections.Generic;
using System.Linq;
using IndexPlugin;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Indexing;
using MUTDOD.Common.Types;
using CompareType = MUTDOD.Common.ModuleBase.Indexing.CompareType;

namespace IndexMechanism.ObjectIndexer
{
    internal class ObjectIndexer
    {
        internal void AddObject(int IndexID, IIndex index, Oid obj, String[] attributes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.AddObject(indexStorage ?? index.EmptyIndexData, obj, attributes);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectIndexAdd,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void AddObjects(int IndexID, IIndex index, Oid[] obj, String[] attributes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                foreach (Oid oid in obj)
                {
                    DateTime indexStart = DateTime.Now;
                    indexStorage = index.AddObject(indexStorage ?? index.EmptyIndexData, oid, attributes);
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                        IndexCostInformation.OneObjectIndexAdd,
                        (float)
                            (DateTime.Now.Ticks - indexStart.Ticks)/
                        10000000);
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
            IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, indexStorage);
        }

        internal void AddObject(int IndexID, IIndex index, Oid obj)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.AddObject(indexStorage ?? index.EmptyIndexData, obj);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectIndexAdd,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void AddObjects(int IndexID, IIndex index, Oid[] obj)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                foreach (Oid oid in obj)
                {
                    DateTime indexStart = DateTime.Now;
                    indexStorage = index.AddObject(indexStorage ?? index.EmptyIndexData, oid);
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                        IndexCostInformation.OneObjectIndexAdd,
                        (float)
                            (DateTime.Now.Ticks - indexStart.Ticks)/
                        10000000);
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
            IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, indexStorage);
        }

        internal void AddDynamicRole(int IndexID, IIndex index, Oid obj, DynamicRole role)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.AddDynamicRole(indexStorage ?? index.EmptyIndexData, obj, role);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneRoleIndexing,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void AddDynamicRoles(int IndexID, IIndex index, Oid[] obj, DynamicRole role)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                foreach (Oid oid in obj)
                {
                    DateTime indexStart = DateTime.Now;
                    indexStorage = index.AddDynamicRole(indexStorage ?? index.EmptyIndexData, oid, role);
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                        IndexCostInformation.OneRoleIndexing,
                        (float)
                            (DateTime.Now.Ticks - indexStart.Ticks)/
                        10000000);
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
            IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, indexStorage);
        }

        internal void AddDynamicRole(int IndexID, IIndex index, Oid obj, DynamicRole role, String[] attributes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.AddDynamicRole(indexStorage ?? index.EmptyIndexData, obj, role,
                    attributes);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneRoleIndexing,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void AddDynamicRoles(int IndexID, IIndex index, Oid[] obj, DynamicRole role, String[] attributes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                foreach (Oid oid in obj)
                {
                    DateTime indexStart = DateTime.Now;
                    indexStorage = index.AddDynamicRole(indexStorage ?? index.EmptyIndexData, oid, role, attributes);
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                        IndexCostInformation.OneRoleIndexing,
                        (float)
                            (DateTime.Now.Ticks - indexStart.Ticks)/
                        10000000);
                }
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
            IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, indexStorage);
        }

        internal void RemoveObject(int IndexID, IIndex index, Oid obj, String[] attributes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.RemoveObject(indexStorage ?? index.EmptyIndexData, obj, attributes);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectIndexRemove,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void RemoveObject(int IndexID, IIndex index, Oid obj)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.RemoveObject(indexStorage ?? index.EmptyIndexData, obj);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectIndexRemove,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void RemoveDynamicRole(int IndexID, IIndex index, Oid obj, DynamicRole role, String[] attributes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.RemoveDynamicRole(indexStorage ?? index.EmptyIndexData, obj, role,
                    attributes);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneRoleIndexRemove,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void RemoveDynamicRole(int IndexID, IIndex index, Oid obj, DynamicRole role)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.RemoveDynamicRole(indexStorage ?? index.EmptyIndexData, obj, role);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneRoleIndexRemove,
                    (float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                    10000000);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal void ClearIndexedObjects(int IndexID, IIndex index)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                IndexData newIndexStorage = index.RemoveObjects(indexStorage ?? index.EmptyIndexData);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal Guid[] GetIndexedObjects(int IndexID, IIndex index, int? packageSize, int skipItemsCount)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                return index.GetIndexedObjects(indexStorage, packageSize, skipItemsCount);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal Guid[] GetIndexedDynamicRoles(int IndexID, IIndex index, int? packageSize, int skipItemsCount)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                return index.GetIndexedDynamicRoles(indexStorage, packageSize, skipItemsCount);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal Guid[] FindObjects(int IndexID, IIndex index, Type type, bool complexExtension)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                int? operations;
                Guid[] ret = index.FindObjects(indexStorage ?? index.EmptyIndexData, type, complexExtension,
                    out operations);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectSearch,
                    ((float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                     10000000)/ret.Length == 0
                        ? 1
                        : ret.Length);
                if ((operations ?? 0) > 0)
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID, IndexCostInformation.HitRatio,
                        ret.Length/(float) operations);
                return ret;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal Guid[] FindObjects(int IndexID, IIndex index, Type type, bool complexExtension, String[] attributes,
            object[] values, CompareType[] compareTypes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                int? operations;
                Guid[] ret = index.FindObjects(indexStorage ?? index.EmptyIndexData, type, complexExtension, attributes,
                    values, compareTypes.ToList().Select(convert).ToArray(), out operations);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectSearch,
                    ((float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                     10000000)/ret.Length == 0
                        ? 1
                        : ret.Length);
                if ((operations ?? 0) > 0)
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID, IndexCostInformation.HitRatio,
                        ret.Length/(float) operations);
                return ret;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal Guid[] FindObjects(int IndexID, IIndex index, DynamicRole dynamicRole)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                int? operations;
                Guid[] ret = index.FindObjects(indexStorage ?? index.EmptyIndexData, dynamicRole, out operations);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID, IndexCostInformation.OneRoleSearch,
                    ((float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                     10000000)/ret.Length == 0
                        ? 1
                        : ret.Length);
                if ((operations ?? 0) > 0)
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID, IndexCostInformation.HitRatio,
                        ret.Length/(float) operations);
                return ret;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal Guid[] FindObjects(int IndexID, IIndex index, DynamicRole dynamicRole, String[] attributes,
            object[] values, CompareType[] compareTypes)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                int? operations;
                Guid[] ret = index.FindObjects(indexStorage ?? index.EmptyIndexData, dynamicRole, attributes, values,
                    compareTypes.ToList().Select(convert).ToArray(), out operations);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID, IndexCostInformation.OneRoleSearch,
                    ((float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                     10000000)/ret.Length == 0
                        ? 1
                        : ret.Length);
                if ((operations ?? 0) > 0)
                    IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID, IndexCostInformation.HitRatio,
                        ret.Length/(float) operations);
                return ret;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal bool CheckIndexValid(int IndexID, IIndex index)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                return index.isValid(indexStorage ?? index.EmptyIndexData);
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal bool RebuildIndex(int IndexID, IIndex index)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                IndexData newIndexStorage = index.rebuildIndex(indexStorage ?? index.EmptyIndexData);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);

                return true;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal bool RebuildIndexWithObjects(int IndexID, IIndex index, Oid[] objects)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.rebuildIndex(indexStorage ?? index.EmptyIndexData, objects);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectIndexRefresh,
                    ((float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                     10000000)/objects.Length);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);

                return true;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        internal bool RebuildIndexWithRoles(int IndexID, IIndex index, Dictionary<Oid, DynamicRole[]> objects)
        {
            IndexData indexStorage = IndexStorageManager.IndexStorageManager.GetIndexData(IndexID);
            try
            {
                DateTime indexStart = DateTime.Now;
                IndexData newIndexStorage = index.rebuildIndex(indexStorage ?? index.EmptyIndexData, objects);
                IndexManager.IndexManager.GetInstance().includeInStatistics(IndexID,
                    IndexCostInformation.OneObjectIndexRefresh,
                    ((float)
                        (DateTime.Now.Ticks - indexStart.Ticks)/
                     10000000)/objects.Keys.Count);
                IndexStorageManager.IndexStorageManager.UpdateIndexData(IndexID, newIndexStorage);

                return true;
            }
            catch (Exception ex)
            {
                if (MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger() != null)
                    MUTDOD.Server.Common.IndexMechanism.IndexMechanism.GetLoger()
                        .Log("IndexMechanism", string.Format("Index {0} throwed exception\n{1}", index.GetType(), ex),
                            MessageLevel.Error);
                throw ex;
            }
        }

        private IndexPlugin.CompareType convert(CompareType t)
        {
            switch (t)
            {
                case CompareType.equal:
                    return IndexPlugin.CompareType.equal;
                case CompareType.greater:
                    return IndexPlugin.CompareType.greater;
                case CompareType.greaterOrEqual:
                    return IndexPlugin.CompareType.greaterOrEqual;
                case CompareType.less:
                    return IndexPlugin.CompareType.less;
                case CompareType.lessOrEqual:
                    return IndexPlugin.CompareType.lessOrEqual;
                case CompareType.like:
                    return IndexPlugin.CompareType.like;
                case CompareType.notEqual:
                    return IndexPlugin.CompareType.notEqual;
                case CompareType.notLike:
                    return IndexPlugin.CompareType.notLike;
                default:
                    throw new ApplicationException(string.Format("Unknown compare type: {0}", t));
            }
        }
    }
}