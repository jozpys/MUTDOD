using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IndexPlugin;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace BasicIndexes
{
    internal class StringDumpIndex : IIndex<Type>
    {
        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of IIndex

        public string Name
        {
            get { return "List Index: string"; }
        }

        public IndexData EmptyIndexData
        {
            get { return new ListData<string>(); }
        }

        public Type[] AvailableIndexingTypes
        {
            get { return new Type[] {typeof (string)}; }
        }

        public settingsChangedHandler SettingsChanged { get; set; }

        public void SetSettings(string xml)
        {
            ;
        }

        public bool isValid(IndexData indexData)
        {
            return true;
        }

        public IndexData rebuildIndex(IndexData indexData, Oid[] objs)
        {
            return indexData;
        }

        public IndexData rebuildIndex(IndexData indexData, Dictionary<Oid, DynamicRole[]> objs)
        {
            return indexData;
        }

        public IndexData rebuildIndex(IndexData indexData)
        {
            return EmptyIndexData;
        }

        private ListData<string> getStorageData(IndexData indexData)
        {
            if (indexData == null)
                return new ListData<string>();
            else if (!(indexData is ListData<string>))
                throw new WrongIndexDataException(typeof (ListData<string>), indexData.GetType(),
                                                  "Nieoczekiwany typ danych o indeksie");
            else
                return indexData as ListData<string>;
        }

        public IndexData AddObject(IndexData indexData, Oid obj, string[] attributes, QueryParameters queryParameters)
        {
            ListData<string> ld = getStorageData(indexData);

            FieldInfo[] objFields = obj.GetType().GetFields();

            foreach (String attribute in attributes)
            {
                if (objFields.Count(p => p.Name == attribute) != 1)
                    throw new WrongAttributeException(obj.GetType(), attribute,
                                                      "Nie odnaleziono podanego atrybutu w wskazanym obiekcie");
                else if (objFields.Single(p => p.Name == attribute).GetValue(obj).GetType() != typeof (string))
                    throw new WrongTypeToIndexException(objFields.Single(p => p.Name == attribute).GetType(),
                                                        string.Format("Unnsupported type of attribiute {0}", attribute));
            }

            foreach (String attribute in attributes)
            {
                string attributeValue = objFields.Where(p => p.Name == attribute).Single().GetValue(obj).ToString();
                ld.Data.Add(new storeItem<string>
                                {val = attributeValue, attribiute = attribute, type = obj.GetType(), ID = obj.Id});
            }

            return ld;
        }

        public IndexData AddObject(IndexData indexData, Oid obj, QueryParameters queryParameters)
        {
            FieldInfo[] objFields = obj.GetType().GetFields();
            String[] attribiutes = objFields.Select(p => p.Name).ToArray();
            return AddObject(indexData, obj, attribiutes, queryParameters);
        }

        public IndexData AddDynamicRole(IndexData indexData, Oid obj, DynamicRole role, string[] attributes)
        {
            throw new NotImplementedException();
        }

        public IndexData AddDynamicRole(IndexData indexData, Oid obj, DynamicRole role)
        {
            throw new NotImplementedException();
        }

        public IndexData RemoveObject(IndexData indexData, Oid obj, string[] attributes)
        {
            throw new NotImplementedException();
        }

        public IndexData RemoveObject(IndexData indexData, Oid obj)
        {
            throw new NotImplementedException();
        }

        public IndexData RemoveDynamicRole(IndexData indexData, Oid obj, DynamicRole role, string[] attributes)
        {
            throw new NotImplementedException();
        }

        public IndexData RemoveDynamicRole(IndexData indexData, Oid obj, DynamicRole role)
        {
            throw new NotImplementedException();
        }

        public IndexData RemoveObjects(IndexData indexData)
        {
            return EmptyIndexData;
        }

        public Guid[] GetIndexedObjects(IndexData indexData, int? packageSize, int skipItemsCount)
        {
            return getStorageData(indexData).Data.Select(p => p.ID).Distinct().ToArray();
        }

        public Guid[] GetIndexedDynamicRoles(IndexData indexData, int? packageSize, int skipItemsCount)
        {
            throw new NotImplementedException();
        }

        public Guid[] FindObjects(IndexData indexData, Type OIDClass, bool complexExtension, out int? readedObjects)
        {
            List<Guid> ret;
            ListData<string> ld = getStorageData(indexData);
            ret = ld.Data.Where(p => p.type == OIDClass).Select(p => p.ID).Distinct().ToList();
            readedObjects = ret.Count();

            if (complexExtension)
            {
                foreach (Type t in ld.Data.Where(p => p.type != OIDClass).Select(p => p.type).Distinct())
                {
                    bool found = false;
                    Type baseType = t.BaseType;
                    while (baseType != null && !found)
                    {
                        if (baseType == OIDClass)
                        {
                            List<Guid> r = ld.Data.Where(p => p.type == baseType).Select(p => p.ID).Distinct().ToList();
                            readedObjects += r.Count;
                            ret.AddRange(r);
                            found = true;
                        }
                        baseType = baseType.BaseType;
                    }
                }
            }


            return ret.Distinct().ToArray();
        }

        public Guid[] FindObjects(IndexData indexData, Type OIDClass, bool complexExtension, string[] attributes,
                                 object[] values, CompareType[] compareTypes, out int? readedObjects)
        {
            List<Guid> ret = new List<Guid>();
            ListData<string> ld = getStorageData(indexData);
            readedObjects = 0;
            for (int i = 0; i < values.Count(); i++)
            {
                List<Guid> r =
                    (ld.Data.Where(p => p.type == OIDClass && isTrue(p.val, values[i].ToString(), compareTypes[i])).
                        Select(p => p.ID).Distinct().ToList());
                readedObjects += r.Count();
                ret.AddRange(r);
            }

            if (complexExtension)
            {
                foreach (Type t in ld.Data.Where(p => p.type != OIDClass).Select(p => p.type).Distinct())
                {
                    bool found = false;
                    Type baseType = t.BaseType;
                    while (baseType != null && !found)
                    {
                        if (baseType == OIDClass)
                        {
                            for (int i = 0; i < values.Count(); i++)
                            {
                                List<Guid> r =
                                    ld.Data.Where(
                                        p => p.type == baseType && isTrue(p.val, values[i].ToString(), compareTypes[i]))
                                        .Select(p => p.ID).Distinct().ToList();
                                readedObjects += r.Count;
                                ret.AddRange(r);
                                found = true;
                            }
                        }
                        baseType = baseType.BaseType;
                    }
                }
            }


            return ret.Distinct().ToArray();
        }

        private static bool isTrue(string a, string b, CompareType ct)
        {
            bool ret;

            switch (ct)
            {
                case CompareType.greater:
                    ret = a.CompareTo(b) > 0;
                    break;
                case CompareType.greaterOrEqual:
                    ret = a.CompareTo(b) >= 0;
                    break;
                case CompareType.like:
                case CompareType.equal:
                    ret = a.CompareTo(b) == 0;
                    break;
                case CompareType.notLike:
                case CompareType.notEqual:
                    ret = a.CompareTo(b) != 0;
                    break;
                case CompareType.less:
                    ret = a.CompareTo(b) < 0;
                    break;
                case CompareType.lessOrEqual:
                    ret = a.CompareTo(b) <= 0;
                    break;
                default:
                    throw new WrongCompareTypeException(ct, "unexpected compare type");
            }
            return ret;
        }

        public Guid[] FindObjects(IndexData indexData, DynamicRole dynamicRole, out int? readedObjects)
        {
            throw new NotImplementedException();
        }

        public Guid[] FindObjects(IndexData indexData, DynamicRole dynamicRole, string[] attributes, object[] values,
                                 CompareType[] compareTypes, out int? readedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost ObjectIndexingCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost ObjectIndexRefreshCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost ObjectIndexRemoveCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost ObjectFindCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost RoleIndexingCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost RoleIndexRefreshCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost RoleIndexRemoveCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public IndexOperationCost RoleFindCost(int indexedObjects)
        {
            throw new NotImplementedException();
        }

        public string[] GetTypesNameIndexedObjects(IndexData indexData)
        {
            throw new NotImplementedException();
        }

        public List<string> GetIndexedAttribiutesForType(Type t)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}