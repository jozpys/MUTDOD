﻿using System;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Indexing;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase
{
    public interface IIndexMechanism : IModule
    {
        bool AddIndex(string dllPath);
        bool Remomveindex(int id);
        Dictionary<int, string> GetIndexes();
        Type[] GetIndexingTypes(int indexId);
        bool IndexObject(int indexID, Oid obj);
        bool IndexObjects(int indexID, Oid[] obj);
        bool IndexObjects(int indexID, Oid[] obj, String[] attributes);
        bool IndexObject(int indexID, Oid obj, DynamicRole role);
        bool IndexObjects(int indexID, Oid[] obj, DynamicRole role);
        bool IndexObjects(int indexID, Oid[] obj, DynamicRole role, String[] attributes);
        bool RemoveObject(int indexID, Oid obj, String[] attributes);
        bool RemoveObject(int indexID, Oid obj);
        bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role, String[] attributes);
        bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role);
        bool ClearIndexedObjects(int indexID);
        Guid[] GetIndexedObjects(int IndexID, int? packageSize, int skipItemsCount);
        Guid[] GetIndexedDynamicRoles(int IndexID, int? packageSize, int skipItemsCount);

        Guid[] FindObjects(int IndexID, Type OIDClass, bool complexExtension, String[] attributes, object[] values,
            CompareType[] compareTypes);

        Guid[] FindObjects(int IndexID, Type OIDClass, bool complexExtension);

        Guid[] FindObjects(int IndexID, DynamicRole role, String[] attributes, object[] values,
            CompareType[] compareTypes);

        Guid[] FindObjects(int IndexID, DynamicRole role);
        void SetIndexSettings(int indexID, string settingsXML);
        string GetIndexSettings(int indexID);
        bool CheckIfIndexValid(int indexID);
        bool RebuildIndex(int indexID);
        bool RebuildIndexWithObjects(int indexID, Oid[] objects);
        bool RebuildIndexWithRoles(int indexID, Dictionary<Oid, DynamicRole[]> objects);
        bool ResetStatistics(int indexID);

        float GetStatistic(int indexID, IndexCostSource src, IndexCostType type, IndexCostInformation info,
            int? theoreticalIndexSize);
    }
}
