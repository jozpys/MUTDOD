using System;
using System.Collections.Generic;
using IndexMechanism.CORE;
using IndexMechanism.IndexManager;
using IndexPlugin;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Indexing;
using MUTDOD.Common.Types;
using CompareType = MUTDOD.Common.ModuleBase.Indexing.CompareType;
using System.Linq;

namespace MUTDOD.Server.Common.IndexMechanism
{
    public class IndexMechanism<T> : Module, IIndexMechanism<T>
    {
        private static ILogger _loger = null;
        private static global::IndexMechanism.ObjectIndexer.ObjectIndexer<T> _objectIndexer = null;

        public IndexMechanism(ILogger logger)
        {
            _loger = logger;
            _objectIndexer = new global::IndexMechanism.ObjectIndexer.ObjectIndexer<T>();
        }

        internal static ILogger GetLoger()
        {
            return _loger;
        }

        public bool AddIndex(string path)
        {
            return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().AddIndex(path);
        }

        public Dictionary<int, string> GetIndexes()
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();

            foreach (IndexInfo<T> index in global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndexes())
            {
                ret.Add(index.IndexID, index.IndexName);
            }

            return ret;
        }

        public Type[] GetIndexingTypes(int indexId)
        {
            try
            {
                return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndexingTypes(indexId);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While readin index indexing types excption occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool Remomveindex(int id)
        {
            try
            {
                return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().RemoveIndex(id);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While removing index exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
        }

        public bool IndexObject(int indexID, Oid obj, QueryParameters queryParameters)
        {
            try
            {
                indexObjects(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), new Oid[] { obj }, null, queryParameters);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While indexing object exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, QueryParameters queryParameters)
        {
            try
            {
                indexObjects(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj, null, queryParameters);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While indexing objects exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, String[] attributes, QueryParameters queryParameters)
        {
            try
            {
                indexObjects(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj, attributes, queryParameters);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While indexing objects exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObject(int indexID, Oid obj, DynamicRole role)
        {
            try
            {
                indexObjectsRoles(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), new Oid[] { obj },
                                  role, null);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While indexing object role exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, DynamicRole role)
        {
            try
            {
                indexObjectsRoles(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj, role, null);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While indexing objects role exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, DynamicRole role, String[] attributes)
        {
            try
            {
                indexObjectsRoles(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj, role,
                                  attributes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While indexing objects role exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObject(int indexID, Oid obj, String[] attributes, QueryParameters queryParameters)
        {
            try
            {
                _objectIndexer.RemoveObject(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj,
                                            attributes, queryParameters);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While removing object from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObject(int indexID, Oid obj, QueryParameters queryParameters)
        {
            try
            {
                _objectIndexer.RemoveObject(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj, queryParameters);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While removing object from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role, String[] attributes)
        {
            try
            {
                _objectIndexer.RemoveDynamicRole(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj,
                                                 role, attributes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While removing object role from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role)
        {
            try
            {
                _objectIndexer.RemoveDynamicRole(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID), obj,
                                                 role);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While removing object role from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool ClearIndexedObjects(int indexID)
        {
            try
            {
                _objectIndexer.ClearIndexedObjects(indexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID));
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While clearing indexed object exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        protected void indexObjects(int indexID, IIndex<T> index, Oid[] obj, String[] attributes, QueryParameters queryParameters)
        {
            if (attributes == null)
                _objectIndexer.AddObjects(indexID, index, obj, queryParameters);
            else
                _objectIndexer.AddObjects(indexID, index, obj, attributes, queryParameters);
        }

        protected void indexObjectsRoles(int indexID, IIndex<T> index, Oid[] obj, DynamicRole role,
                                         String[] attributes)
        {
            if (attributes == null)
                _objectIndexer.AddDynamicRoles(indexID, index, obj, role);
            else
                _objectIndexer.AddDynamicRoles(indexID, index, obj, role, attributes);
        }

        public Guid[] GetIndexedObjects(int IndexID, int? packageSize, int skipItemsCount)
        {
            try
            {
                return _objectIndexer.GetIndexedObjects(IndexID,
                                                        global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(IndexID),
                                                        packageSize,
                                                        skipItemsCount);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While reading indexed objects exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] GetIndexedDynamicRoles(int IndexID, int? packageSize, int skipItemsCount)
        {
            try
            {
                return _objectIndexer.GetIndexedDynamicRoles(IndexID,
                                                             global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(IndexID),
                                                             packageSize,
                                                             skipItemsCount);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While reading indexed roles exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, T OIDClass, bool complexExtension, String[] attributes, object[] values,
                                 CompareType[] compareTypes)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(IndexID),
                                                  OIDClass,
                                                  complexExtension, attributes, values, compareTypes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, T OIDClass, bool complexExtension)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(IndexID),
                                                  OIDClass,
                                                  complexExtension);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, DynamicRole role, String[] attributes, object[] values,
                                 CompareType[] compareTypes)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(IndexID),
                                                  role, attributes, values, compareTypes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, DynamicRole role)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(IndexID),
                                                  role);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public void SetIndexSettings(int indexID, string settingsXML)
        {
            try
            {
                global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().SetIndexSettings(indexID, settingsXML);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While replacing indexed settings exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public string GetIndexSettings(int indexID)
        {
            try
            {
                return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndexSettings(indexID);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While reading indexed settings exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool CheckIfIndexValid(int indexID)
        {
            try
            {
                return _objectIndexer.CheckIndexValid(indexID,
                                                      global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID));
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While checkin indexed valid exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool RebuildIndex(int indexID)
        {
            try
            {
                return _objectIndexer.RebuildIndex(indexID,
                                                   global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID));
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While rebuilding indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool RebuildIndexWithObjects(int indexID, Oid[] objects)
        {
            try
            {
                return _objectIndexer.RebuildIndexWithObjects(indexID,
                                                              global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID),
                                                              objects);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name, string.Format("While rebuilding indexed objects exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool RebuildIndexWithRoles(int indexID, Dictionary<Oid, DynamicRole[]> objects)
        {
            try
            {
                return _objectIndexer.RebuildIndexWithRoles(indexID,
                                                            global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexID),
                                                            objects);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,
                        string.Format("While rebuilding indexed objects and roles exception occurred\n{0}", ex),
                        MessageLevel.Error);
                throw ex;
            }
        }

        public bool ResetStatistics(int indexID)
        {
            global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().ResetStatistics(indexID);
            return true;
        }

        public float GetStatistic(int indexID, IndexCostSource src, IndexCostType type, IndexCostInformation info,
                                  int? theoreticalIndexSize)
        {
            return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetStatistic(indexID, src, type, info, theoreticalIndexSize);
        }

        public string[] GetTypesNameIndexedObjects(int indexId)
        {
            return _objectIndexer.GetTypesNameIndexedObjects(indexId, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexId));
        }

        public string[] GetIndexedAttribiutesForType(int indexId, string type)
        {
            return _objectIndexer.GetIndexedAttribiutesForType(indexId, global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexId), type);

        }
        public string GetIndex(int indexId)
        {
            return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexId).Name;
        }

        public int GetAvarageObjectFindCost(int indexId, int numberIndexedObject)
        {
            return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexId).ObjectFindCost(numberIndexedObject).AverageCost;
        }

        public int GetPessimisticObjectFindCost(int indexId, int numberIndexedObject)
        {
            return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexId).ObjectFindCost(numberIndexedObject).PessimisticCost;
        }

        public int GetOptimisticObjectFindCost(int indexId, int numberIndexedObject)
        {
            return global::IndexMechanism.IndexManager.IndexManager<T>.GetInstance().GetIndex(indexId).ObjectFindCost(numberIndexedObject).OptimisticCost;

        }
        
        public Dictionary<int, string> GetIndexesForClass(T className)
        { 
            return GetIndexes().Where(p => GetTypesNameIndexedObjects(p.Key).Contains(className.ToString()))
                               .ToDictionary(r => r.Key, r => r.Value);

        }

        public Dictionary<int, string> GetIndexesForAttributes(T className, List<string> attributes)
        {
            return GetIndexes().Where(p => GetTypesNameIndexedObjects(p.Key).Contains(className.ToString()) &&
                                       GetIndexedAttribiutesForType(p.Key, className.ToString()).All(s => attributes.Contains(s)))
                               .ToDictionary(r => r.Key, r => r.Value);
        }

        public Dictionary<int, string> GetIndexesForAttribute(T className, string attribute)
        {
            return GetIndexes().Where(p => GetTypesNameIndexedObjects(p.Key).Contains(className.ToString()) &&
                                       GetIndexedAttribiutesForType(p.Key, className.ToString()).Any(s => s.Contains(attribute)))
                               .ToDictionary(r => r.Key, r => r.Value);
        }

        public string Name { get { return "IndexMechanism"; } }
    }
}