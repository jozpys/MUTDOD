using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IndexPlugin;
using BasicIndexes.BinaryTree;
using MUTDOD.Common.Types;
using MUTDOD.Common.ModuleBase.Communication;

namespace BasicIndexes
{
    public class IntIndexMUTDOD : IIndex<String>
    {
        private string _settings = string.Empty;

        #region Implementation of IDisposable

        public void Dispose()
        {
            ;
        }

        #endregion

        #region Implementation of IIndex

        public string Name
        {
            get { return "BST Index: integer"; }
        }

        public IndexData EmptyIndexData
        {
            get { return new IntBinaryTreeMUTDOD(); }
        }

        public Type[] AvailableIndexingTypes
        {
            get { return new Type[] { typeof(int) }; }
        }

        public settingsChangedHandler SettingsChanged { get; set; }

        public void SetSettings(string xml)
        {
            _settings = xml;
        }

        public bool isValid(IndexData indexData)
        {
            return true;
        }

        public IndexData rebuildIndex(IndexData indexData, Oid[] objs)
        {
            throw new NotImplementedException();
        }

        public IndexData rebuildIndex(IndexData indexData, Dictionary<Oid, DynamicRole[]> objs)
        {
            throw new NotImplementedException();
        }

        public IndexData rebuildIndex(IndexData indexData)
        {
            return EmptyIndexData;
        }

        private IntBinaryTreeMUTDOD getStorageData(IndexData indexData)
        {
            if (indexData == null)
                return new IntBinaryTreeMUTDOD();
            else if (!(indexData is IntBinaryTreeMUTDOD))
                throw new WrongIndexDataException(typeof(IntBinaryTreeMUTDOD), indexData.GetType(),
                                                  "Nieoczekiwany typ danych o indeksie");
            else
                return indexData as IntBinaryTreeMUTDOD;
        }

        public IndexData AddObject(IndexData indexData, Oid obj, string[] attributes, QueryParameters queryParameters)
        {
            IntBinaryTreeMUTDOD BT = getStorageData(indexData);

            /* FieldInfo[] objFields = obj.GetType().GetFields();

             foreach (String attribute in attributes)
             {
                 if (objFields.Count(p => p.Name == attribute) != 1)
                     throw new WrongAttributeException(obj.GetType(), attribute,
                                                       "Nie odnaleziono podanego atrybutu w wskazanym obiekcie");
                 else if (objFields.Single(p => p.Name == attribute).GetValue(obj).GetType() != typeof (int))
                     throw new WrongTypeToIndexException(objFields.Single(p => p.Name == attribute).GetType(),
                                                         string.Format("Unnsupported type of attribiute {0}", attribute));
             }*/

            foreach (String attribute in attributes)
            {
                // int attributeValue = (int) objFields.Where(p => p.Name == attribute).Single().GetValue(obj);
                string type=null;
                BT.AddToBTvalue(obj, attribute, 1, type); //TYPE -> string
            }

            return BT;
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
            IntBinaryTreeMUTDOD BT = getStorageData(indexData);

            FieldInfo[] objFields = obj.GetType().GetFields();

            foreach (String attribute in attributes)
            {
                if (objFields.Count(p => p.Name == attribute) != 1)
                    throw new WrongAttributeException(obj.GetType(), attribute,
                                                      "Nie odnaleziono podanego atrybutu w wskazanym obiekcie");
                //else if (objFields.Single(p => p.Name == attribute).GetType() != typeof(int))
                //    throw new WrongTypeToIndexException(objFields.Single(p => p.Name == attribute).GetType(), string.Format("Unnsupported type of attribiute {0}", attribute));
            }

            foreach (String attribute in attributes)
            {
                if (objFields.Single(p => p.Name == attribute).GetType() == typeof(int))
                {
                    int attributeValue = (int)objFields.Where(p => p.Name == attribute).Single().GetValue(obj);
                    //BT.RemoveFromBTvalue(obj, attribute, attributeValue); TYPE -> string
                }
            }

            return BT;
        }

        public IndexData RemoveObject(IndexData indexData, Oid obj)
        {
            FieldInfo[] objFields = obj.GetType().GetFields();
            String[] attribiutes = objFields.Select(p => p.Name).ToArray();
            return RemoveObject(indexData, obj, attribiutes);
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
            IntBinaryTreeMUTDOD BT = getStorageData(indexData);

            return BT.GetObjectsFromBT();
        }

        public Guid[] GetIndexedDynamicRoles(IndexData indexData, int? packageSize, int skipItemsCount)
        {
            return new Guid[0];
        }

        public Guid[] FindObjects(IndexData indexData, string OIDClass, bool complexExtension, out int? readedObjects)
        {
            int ro;
            IntBinaryTreeMUTDOD BT = getStorageData(indexData);
            List<Guid> ret = BT.GetObjectsFromBT(OIDClass, out ro);
            readedObjects = ro;

            if (complexExtension)
            {
                foreach (string t in BT.GetObjectsTypesFromBT().Where(t => t != OIDClass))
                {
                    bool found = false;
                    string baseType = t;
                    while (baseType != null && !found)
                    {
                        if (baseType == OIDClass)
                        {
                            ret.AddRange(BT.GetObjectsFromBT(OIDClass, out ro));
                            readedObjects += ro;
                            found = true;
                        }
                        //baseType = baseType.BaseType; TODO parent class
                    }
                }
            }

            return ret.Distinct().ToArray();
        }

        public Guid[] FindObjects(IndexData indexData, string OIDClass, bool complexExtension, string[] attributes,
                                 object[] values, CompareType[] compareTypes, out int? readedObjects)
        {
            if (compareTypes.Count() != attributes.Count() || attributes.Count() != values.Count())
                throw new ArgumentException(
                    "Liczności tbalic z atrybutami, porównaniami oraz wartościami musza być identyczne!");

            readedObjects = 0;
            int ro;
            IntBinaryTreeMUTDOD BT = getStorageData(indexData);
            List<Guid> ret = null;

            for (int i = 0; i < attributes.Count(); i++)
            {
                List<Guid> foundOIDs = BT.GetObjectsFromBT(OIDClass, attributes[i], (int)values[i], compareTypes[i],
                                                          out ro);
                readedObjects += ro;

                if (complexExtension)
                {
                    foreach (string t in BT.GetObjectsTypesFromBT().Where(t => t != OIDClass))
                    {
                        bool found = false;
                        string baseType = t;
                        while (baseType != null && !found)
                        {
                            if (baseType == OIDClass)
                            {
                                foundOIDs.AddRange(BT.GetObjectsFromBT(OIDClass, attributes[i], (int)values[i],
                                                                       compareTypes[i], out ro));
                                readedObjects += ro;
                                found = true;
                            }
                            //baseType = baseType.; parent class TODO
                        }
                    }
                }

                if (ret == null)
                    ret = foundOIDs.Distinct().ToList();
                else
                    ret =
                        ret.Intersect(foundOIDs.Distinct()).ToList();
            }

            return ret.ToArray();
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
            return new IndexOperationCost
            {
                AverageCost = Convert.ToInt32(Math.Log(indexedObjects, 2)),
                OptimisticCost = 1,
                PessimisticCost = indexedObjects
            };
        }

        public IndexOperationCost ObjectIndexRefreshCost(int indexedObjects)
        {
            return new IndexOperationCost
            {
                AverageCost = Convert.ToInt32(Math.Log(indexedObjects, 2)),
                OptimisticCost = 1,
                PessimisticCost = indexedObjects
            };
        }

        public IndexOperationCost ObjectIndexRemoveCost(int indexedObjects)
        {
            return new IndexOperationCost
            {
                AverageCost = Convert.ToInt32(Math.Log(indexedObjects, 2)),
                OptimisticCost = 1,
                PessimisticCost = indexedObjects
            };
        }

        public IndexOperationCost ObjectFindCost(int indexedObjects)
        {
            return new IndexOperationCost
            {
                AverageCost = Convert.ToInt32(Math.Log(indexedObjects, 2)),
                OptimisticCost = 1,
                PessimisticCost = indexedObjects
            };
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
            IntBinaryTreeMUTDOD BT = getStorageData(indexData);
            return BT.GetObjectsTypesFromBT();
        }

        public List<string> GetIndexedAttribiutesForType(string t)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [Serializable]
    public class IntBinaryTreeMUTDOD: IndexData
    {
        public TBinarySTree<string, TBinarySTree<string, TBinarySTree<int, List<Guid>>>> BT;

        public IntBinaryTreeMUTDOD()
        {
            BT = new TBinarySTree<string, TBinarySTree<string, TBinarySTree<int, List<Guid>>>>(compareString);
        }

        private TBinarySTree<string, TBinarySTree<int, List<Guid>>> GetAttribiutesBT(string t)
        {
            var tree = BT.find(t);
            if (tree == null)
                tree = BT.insert(t, new TBinarySTree<string, TBinarySTree<int, List<Guid>>>(compareString));

            return tree.value;
        }

        private TBinarySTree<int, List<Guid>> GetObjectsBT(string t, string attribiute)
        {
            var TypeTree = GetAttribiutesBT(t);
            var AttribiuteTree = TypeTree.find(attribiute);
            if (AttribiuteTree == null)
                AttribiuteTree = TypeTree.insert(attribiute, new TBinarySTree<int, List<Guid>>(compareInts));

            return AttribiuteTree.value;
        }

        private List<Guid> GetObjectsBT(string t, string attribiute, int value)
        {
            var AttribiuteTree = GetObjectsBT(t, attribiute);
            var ValuesTree = AttribiuteTree.find(value);
            if (ValuesTree == null)
                ValuesTree = AttribiuteTree.insert(value, new List<Guid>());

            return ValuesTree.value;
        }

        public List<Guid> GetObjectsFromBT(string OID, string attribiute, int value, CompareType compare,
                                          out int readBTObjects)
        {
            List<Guid> ret = new List<Guid>();
            foreach (var i in GetObjectsBT(OID, attribiute).find(compare, value, out readBTObjects))
                ret.AddRange(i);
            return ret.Distinct().ToList();
        }

        public List<Guid> GetObjectsFromBT(string OID, out int readBTObjects)
        {
            List<Guid> ret = new List<Guid>();
            foreach (var i in GetAttribiutesBT(OID).getAllValues(out readBTObjects))
            {
                int r;
                foreach (List<Guid> j in i.getAllValues(out r))
                {
                    readBTObjects += r;
                    ret.AddRange(j);
                }
            }
            return ret.Distinct().ToList();
        }

        public Guid[] GetObjectsFromBT()
        {
            List<Guid> ret = new List<Guid>();
            foreach (var i in BT.getAllValues())
            {
                foreach (var j in i.getAllValues())
                {
                    foreach (var k in j.getAllValues())
                    {
                        ret.AddRange(k);
                    }
                }
            }
            return ret.Distinct().ToArray();
        }

        public string[] GetObjectsTypesFromBT()
        {
            return BT.getAllKeys();
        }

        public void AddToBTvalue(Oid obj, string attribiute, int value, string type)
        {
            var values = GetObjectsBT(type, attribiute, value);
            if (!values.Contains(obj.Id))
                values.Add(obj.Id);
        }

        public void RemoveFromBTvalue(Oid obj, string attribiute, int value, string type)
        {
            var values = GetObjectsBT(type, attribiute, value);
            if (!values.Contains(obj.Id))
                values.Add(obj.Id);
        }

        private CompareResult compareInts(int elem1, int elem2)
        {
            if (elem1 > elem2)
                return CompareResult.Greater;
            else if (elem1 < elem2)
                return CompareResult.Less;
            else
                return CompareResult.Equal;
        }

        private CompareResult compareString(string elem1, string elem2)
        {
            if (elem1.CompareTo(elem2) > 0)
                return CompareResult.Greater;
            else if (elem1.CompareTo(elem2) < 0)
                return CompareResult.Less;
            else
                return CompareResult.Equal;
        }
    }
}