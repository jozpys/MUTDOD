﻿using System;
using System.Collections.Generic;
using IndexMechanism.CORE;
using IndexMechanism.IndexManager;
using IndexPlugin;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Indexing;
using MUTDOD.Common.Types;
using CompareType = MUTDOD.Common.ModuleBase.Indexing.CompareType;

namespace MUTDOD.Server.Common.IndexMechanism
{
    public class IndexMechanism : Module, IIndexMechanism
    {
        private static ILogger _loger = null;
        private static global::IndexMechanism.ObjectIndexer.ObjectIndexer _objectIndexer = null;

        public IndexMechanism(ILogger logger)
        {
            _loger = logger;
            _objectIndexer = new global::IndexMechanism.ObjectIndexer.ObjectIndexer();
        }

        internal static ILogger GetLoger()
        {
            return _loger;
        }

        public bool AddIndex(string path)
        {
            return global::IndexMechanism.IndexManager.IndexManager.GetInstance().AddIndex(path);
        }

        public Dictionary<int, string> GetIndexes()
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();

            foreach (IndexInfo index in global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndexes())
            {
                ret.Add(index.IndexID, index.IndexName);
            }

            return ret;
        }

        public Type[] GetIndexingTypes(int indexId)
        {
            try
            {
                return global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndexingTypes(indexId);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While readin index indexing types excption occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool Remomveindex(int id)
        {
            try
            {
                return global::IndexMechanism.IndexManager.IndexManager.GetInstance().RemoveIndex(id);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While removing index exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
        }

        public bool IndexObject(int indexID, Oid obj)
        {
            try
            {
                indexObjects(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), new Oid[] {obj}, null);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While indexing object exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj)
        {
            try
            {
                indexObjects(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj, null);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While indexing objects exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, String[] attributes)
        {
            try
            {
                indexObjects(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj, attributes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While indexing objects exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObject(int indexID, Oid obj, DynamicRole role)
        {
            try
            {
                indexObjectsRoles(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), new Oid[] {obj},
                                  role, null);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While indexing object role exception occurred\n{0}", ex), MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, DynamicRole role)
        {
            try
            {
                indexObjectsRoles(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj, role, null);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While indexing objects role exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool IndexObjects(int indexID, Oid[] obj, DynamicRole role, String[] attributes)
        {
            try
            {
                indexObjectsRoles(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj, role,
                                  attributes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While indexing objects role exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObject(int indexID, Oid obj, String[] attributes)
        {
            try
            {
                _objectIndexer.RemoveObject(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj,
                                            attributes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While removing object from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObject(int indexID, Oid obj)
        {
            try
            {
                _objectIndexer.RemoveObject(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While removing object from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role, String[] attributes)
        {
            try
            {
                _objectIndexer.RemoveDynamicRole(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj,
                                                 role, attributes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While removing object role from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role)
        {
            try
            {
                _objectIndexer.RemoveDynamicRole(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID), obj,
                                                 role);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While removing object role from index exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        public bool ClearIndexedObjects(int indexID)
        {
            try
            {
                _objectIndexer.ClearIndexedObjects(indexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID));
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While clearing indexed object exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
            return true;
        }

        protected void indexObjects(int indexID, IIndex index, Oid[] obj, String[] attributes)
        {
            if (attributes == null)
                _objectIndexer.AddObjects(indexID, index, obj);
            else
                _objectIndexer.AddObjects(indexID, index, obj, attributes);
        }

        protected void indexObjectsRoles(int indexID, IIndex index, Oid[] obj, DynamicRole role,
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
                                                        global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(IndexID),
                                                        packageSize,
                                                        skipItemsCount);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While reading indexed objects exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] GetIndexedDynamicRoles(int IndexID, int? packageSize, int skipItemsCount)
        {
            try
            {
                return _objectIndexer.GetIndexedDynamicRoles(IndexID,
                                                             global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(IndexID),
                                                             packageSize,
                                                             skipItemsCount);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While reading indexed roles exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, Type OIDClass, bool complexExtension, String[] attributes, object[] values,
                                 CompareType[] compareTypes)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(IndexID),
                                                  OIDClass,
                                                  complexExtension, attributes, values, compareTypes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, Type OIDClass, bool complexExtension)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(IndexID),
                                                  OIDClass,
                                                  complexExtension);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, DynamicRole role, String[] attributes, object[] values,
                                 CompareType[] compareTypes)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(IndexID),
                                                  role, attributes, values, compareTypes);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public Guid[] FindObjects(int IndexID, DynamicRole role)
        {
            try
            {
                return _objectIndexer.FindObjects(IndexID, global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(IndexID),
                                                  role);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While searching objects in indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public void SetIndexSettings(int indexID, string settingsXML)
        {
            try
            {
                global::IndexMechanism.IndexManager.IndexManager.GetInstance().SetIndexSettings(indexID, settingsXML);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While replacing indexed settings exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public string GetIndexSettings(int indexID)
        {
            try
            {
                return global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndexSettings(indexID);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While reading indexed settings exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool CheckIfIndexValid(int indexID)
        {
            try
            {
                return _objectIndexer.CheckIndexValid(indexID,
                                                      global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID));
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While checkin indexed valid exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool RebuildIndex(int indexID)
        {
            try
            {
                return _objectIndexer.RebuildIndex(indexID,
                                                   global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID));
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While rebuilding indexed exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool RebuildIndexWithObjects(int indexID, Oid[] objects)
        {
            try
            {
                return _objectIndexer.RebuildIndexWithObjects(indexID,
                                                              global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID),
                                                              objects);
            }
            catch (Exception ex)
            {
                if (GetLoger() != null)
                    GetLoger().Log(Name,string.Format("While rebuilding indexed objects exception occurred\n{0}", ex),
                                                         MessageLevel.Error);
                throw ex;
            }
        }

        public bool RebuildIndexWithRoles(int indexID, Dictionary<Oid, DynamicRole[]> objects)
        {
            try
            {
                return _objectIndexer.RebuildIndexWithRoles(indexID,
                                                            global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetIndex(indexID),
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
            global::IndexMechanism.IndexManager.IndexManager.GetInstance().ResetStatistics(indexID);
            return true;
        }

        public float GetStatistic(int indexID, IndexCostSource src, IndexCostType type, IndexCostInformation info,
                                  int? theoreticalIndexSize)
        {
            return global::IndexMechanism.IndexManager.IndexManager.GetInstance().GetStatistic(indexID, src, type, info, theoreticalIndexSize);
        }

        public string Name { get { return "IndexMechanism"; } }
    }    
}