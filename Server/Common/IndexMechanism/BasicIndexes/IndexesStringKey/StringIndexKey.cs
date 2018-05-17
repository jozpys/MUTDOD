using BasicIndexes.BinaryTree;
using IndexPlugin;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.Indexes.BasicIndexes.IndexesStringKey
{
    public class StringIndexKey : IIndex<String>
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
            get { return "BST Index: string"; }
        }

        public IndexData EmptyIndexData
        {
            get { return new StringBinaryTree(); }
        }

        public Type[] AvailableIndexingTypes
        {
            get { return new Type[] { typeof(string) }; }
        }

        public settingsChangedHandler<string> SettingsChanged { get; set; }

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

        private StringBinaryTree getStorageData(IndexData indexData)
        {
            if (indexData == null)
                return new StringBinaryTree();
            else if (!(indexData is StringBinaryTree))
                throw new WrongIndexDataException(typeof(StringBinaryTree), indexData.GetType(),
                                                  "Nieoczekiwany typ danych o indeksie");
            else
                return indexData as StringBinaryTree;
        }

        public IndexData AddObject(IndexData indexData, Oid obj,  string[] attributes, QueryParameters queryParameters)
        {
            StringBinaryTree BT = getStorageData(indexData);

            var classOid = GetClassByOID(obj, queryParameters);

            VerifyAttributes(attributes, queryParameters, classOid);

            foreach (String attribute in attributes)
            {
                string attributeValue = GetAttributeValue(attribute, obj, queryParameters);
                BT.AddToBTvalue(obj, classOid.Name, attribute, attributeValue);
            }

            return BT;
        }

        public IndexData AddObject(IndexData indexData, Oid obj, QueryParameters queryParameters)
        {
            String[] attribiutes = GetAttributes(obj, queryParameters);
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

        public IndexData RemoveObject(IndexData indexData, Oid obj, string[] attributes, QueryParameters queryParameters)
        {
            StringBinaryTree BT = getStorageData(indexData);

            var classOid = GetClassByOID(obj, queryParameters);

            VerifyAttributes(attributes, queryParameters, classOid);

            foreach (String attribute in attributes)
            {

                string attributeValue = GetAttributeValue(attribute, obj, queryParameters);
                BT.RemoveFromBTvalue(obj, classOid.Name, attribute, attributeValue);
            }

            return BT;
        }

        public IndexData RemoveObject(IndexData indexData, Oid obj, QueryParameters queryParameters)
        {
            String[] attribiutes = GetAttributes(obj, queryParameters);
            return RemoveObject(indexData, obj, attribiutes, queryParameters);
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
            StringBinaryTree BT = getStorageData(indexData);

            return BT.GetObjectsFromBT();
        }

        public Guid[] GetIndexedDynamicRoles(IndexData indexData, int? packageSize, int skipItemsCount)
        {
            return new Guid[0];
        }

        public Guid[] FindObjects(IndexData indexData, string OIDClass, bool complexExtension, out int? readedObjects)
        {
            int ro;
            StringBinaryTree BT = getStorageData(indexData);
            List<Guid> ret = BT.GetObjectsFromBT(OIDClass, out ro);
            readedObjects = ro;

            /*if (complexExtension)
            {
                foreach (string t in BT.GetObjectsTypesFromBT().Where(t => t != OIDClass))
                {
                    bool found = false;
                    Type baseType = t.BaseType;
                    while (baseType != null && !found)
                    {
                        if (baseType == OIDClass)
                        {
                            ret.AddRange(BT.GetObjectsFromBT(OIDClass, out ro));
                            readedObjects += ro;
                            found = true;
                        }
                        baseType = baseType.BaseType;
                    }
                }
            }*/

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
            StringBinaryTree BT = getStorageData(indexData);
            List<Guid> ret = null;

            for (int i = 0; i < attributes.Count(); i++)
            {
                List<Guid> foundOIDs = BT.GetObjectsFromBT(OIDClass, attributes[i], values[i].ToString(), compareTypes[i],
                                                          out ro);
                readedObjects += ro;

                /*if (complexExtension)
                {
                    foreach (string t in BT.GetObjectsTypesFromBT().Where(t => t != OIDClass))
                    {
                        bool found = false;
                        Type baseType = t.BaseType;
                        while (baseType != null && !found)
                        {
                            if (baseType == OIDClass)
                            {
                                foundOIDs.AddRange(BT.GetObjectsFromBT(OIDClass, attributes[i], values[i].ToString(),
                                                                       compareTypes[i], out ro));
                                readedObjects += ro;
                                found = true;
                            }
                            baseType = baseType.BaseType;
                        }
                    }
                }*/

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
            return getStorageData(indexData).GetObjectsTypesFromBT();
        }

        public string[] GetIndexedAttribiutesForType(IndexData indexData, string t)
        {
            StringBinaryTree BT = getStorageData(indexData);
            return BT.GetAttributesForTypeFromBT(t);
        }

        private Class GetClassByOID(Oid obj, QueryParameters queryParameters)
        {
            return queryParameters.Database.Schema.Classes.Where(p => queryParameters.Database.Schema.ClassProperties(p.Value)
                                                                     .All(pc => queryParameters.Storage.Get(queryParameters.Database.DatabaseId, obj)
                                                                                .Properties.Any(po => po.Key.Equals(pc) && po.Key.ParentClassId == p.Key.Id)))
                                                          .Single().Value;
        }

        private bool CheckIfClassHasAttributes(string[] attributes, QueryParameters queryParameters, Class classOid)
        {
            return attributes.All(a => queryParameters.Database.Schema.ClassProperties(classOid)
                                       .Any(cp => cp.Name.Equals(a)));
        }

        private void VerifyAttributes(string[] attributes, QueryParameters queryParameters, Class classOid)
        {
            attributes
                .ToList()
                .ForEach(a =>
                        {
                            CheckIfClassHasAttribute(a, queryParameters, classOid);
                            VerifyAttributeType(a, queryParameters, classOid);
                        });
         }

        private void CheckIfClassHasAttribute(string attribute, QueryParameters queryParameters, Class classOid)
        {
            if(!queryParameters.Database.Schema.ClassProperties(classOid)
                                       .Any(cp => cp.Name.Equals(attribute)))

                throw new WrongAttributeException<string>(classOid.FullName, attribute,
                                                           "Nie odnaleziono podanego atrybutu w wskazanym obiekcie");
        }

        private void VerifyAttributeType(string attribute, QueryParameters queryParameters, Class classOid)
        {
            var attributeType = queryParameters.Database.Schema.ClassProperties(classOid)
                                .Where(p => p.Name.Equals(attribute))
                                .Single().DotNetType;
            if (attributeType != typeof(string))
                throw new WrongTypeToIndexException(attributeType, string.Format("Unnsupported type of attribiute {0}", attribute));
        }

        private string GetAttributeValue(string attribute, Oid obj, QueryParameters queryParameters)
        {
            return (string)queryParameters.Storage.Get(queryParameters.Database.DatabaseId, obj).Properties
                           .Where(p => p.Key.Name.Equals(attribute)).Single().Value;
        }

        private string[] GetAttributes(Oid obj, QueryParameters queryParameters)
        {
            return queryParameters.Storage.Get(queryParameters.Database.DatabaseId, obj).Properties
                   .Select(p => p.Key.Name)
                   .ToArray();
        }
        #endregion
    }

    [Serializable]
    public class StringBinaryTree : IndexData
    {
        public TBinarySTree<string, TBinarySTree<string, TBinarySTree<string, List<Guid>>>> BT;

        public StringBinaryTree()
        {
            BT = new TBinarySTree<string, TBinarySTree<string, TBinarySTree<string, List<Guid>>>>(compareString);
        }

        private TBinarySTree<string, TBinarySTree<string, List<Guid>>> GetAttribiutesBT(string t)
        {
            var tree = BT.find(t);
            if (tree == null)
                tree = BT.insert(t, new TBinarySTree<string, TBinarySTree<string, List<Guid>>>(compareString));

            return tree.value;
        }

        private TBinarySTree<string, List<Guid>> GetObjectsBT(string t, string attribiute)
        {
            var TypeTree = GetAttribiutesBT(t);
            var AttribiuteTree = TypeTree.find(attribiute);
            if (AttribiuteTree == null)
                AttribiuteTree = TypeTree.insert(attribiute, new TBinarySTree<string, List<Guid>>(compareString));

            return AttribiuteTree.value;
        }

        private List<Guid> GetObjectsBT(string t, string attribiute, string value)
        {
            var AttribiuteTree = GetObjectsBT(t, attribiute);
            var ValuesTree = AttribiuteTree.find(value);
            if (ValuesTree == null)
                ValuesTree = AttribiuteTree.insert(value, new List<Guid>());

            return ValuesTree.value;
        }

        public List<Guid> GetObjectsFromBT(string OID, string attribiute, string value, CompareType compare,
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

        public string[] GetAttributesForTypeFromBT(string className)
        {
            return BT.find(className)?.value.getAllKeys().Distinct().ToArray();
        }

        public void AddToBTvalue(Oid obj, string className, string attribiute, string value)
        {
            var values = GetObjectsBT(className, attribiute, value);
            if (!values.Contains(obj.Id))
                values.Add(obj.Id);
        }

        public void RemoveFromBTvalue(Oid obj, string className, string attribiute, string value)
        {
            var values = GetObjectsBT(className, attribiute, value);
            if (!values.Contains(obj.Id))
                values.Add(obj.Id);
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
