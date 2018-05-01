using System;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Indexing;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase
{
    public interface IIndexMechanism<T> : IModule
    {
        bool AddIndex(string dllPath);
        bool Remomveindex(int id);
        Dictionary<int, string> GetIndexes();
        Type[] GetIndexingTypes(int indexId);
        bool IndexObject(int indexID, Oid obj, QueryParameters queryParameters);
        bool IndexObjects(int indexID, Oid[] obj, QueryParameters queryParameters);
        bool IndexObjects(int indexID, Oid[] obj, String[] attributes, QueryParameters queryParameters);
        bool IndexObject(int indexID, Oid obj, DynamicRole role);
        bool IndexObjects(int indexID, Oid[] obj, DynamicRole role);
        bool IndexObjects(int indexID, Oid[] obj, DynamicRole role, String[] attributes);
        bool RemoveObject(int indexID, Oid obj, String[] attributes, QueryParameters queryParameters);
        bool RemoveObject(int indexID, Oid obj, QueryParameters queryParameters);
        bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role, String[] attributes);
        bool RemoveObjectRole(int indexID, Oid obj, DynamicRole role);
        bool ClearIndexedObjects(int indexID);
        Guid[] GetIndexedObjects(int IndexID, int? packageSize, int skipItemsCount);
        Guid[] GetIndexedDynamicRoles(int IndexID, int? packageSize, int skipItemsCount);

        Guid[] FindObjects(int IndexID, T OIDClass, bool complexExtension, String[] attributes, object[] values,
            CompareType[] compareTypes);

        Guid[] FindObjects(int IndexID, T OIDClass, bool complexExtension);

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
        List<string> GetTypesNameIndexedObjects(int indexId);
        List<string> GetIndexedAttribiutesForType(int indexId, string type);
        int GetAvarageObjectFindCost(int indexId, int numberIndexedObject);
        int GetPessimisticObjectFindCost(int indexId, int numberIndexedObject);
        int GetOptimisticObjectFindCost(int indexId, int numberIndexedObject);
        Dictionary<int, string> GetIndexesForClass(T className);
        Dictionary<int, string> getIndexesForAttributes(T className, List<string> attributes);
        Dictionary<int, string> getIndexesForAttribute(T className, string attribute);
    }
}
