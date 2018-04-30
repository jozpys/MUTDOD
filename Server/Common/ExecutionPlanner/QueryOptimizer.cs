using IndexPlugin;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.QueryTree;
using System.Collections.Generic;
using System.Linq;

namespace MUTDOD.Server.Common.DummyQueryOptimizer
{
    public class QueryOptimizer : Module, IQueryOptimizer
    {
        public string Name
        {
            get { return "QueryOptimizer"; }
        }

        public IQueryElement OptimizeQueryPlan(IQueryElement queryTree, QueryParameters queryParameters)
        {
            return queryTree;
        }

        public static QueryPlanTree BuildTreeSummary(IQueryElement queryTree)
        {
            int childId = 1;
            
            QueryPlanTree queryPlanTree = new QueryPlanTree(childId, 0, queryTree, queryTree.ElementType, null, queryTree is AbstractLeaf ? ((AbstractLeaf)queryTree).GetValue() : "", queryTree.Cost, queryTree.Cardinality, queryTree.AccessType);
            
            if (queryTree.GetComposite() != null)
            {
                queryPlanTree.Childrens = new List<QueryPlanTree>();
                foreach (var elements in queryTree.GetComposite().GetElements())
                {
                    foreach (var element in elements.Value)
                        if (element != null && element.GetComposite() != null && element.GetComposite().GetElements() != null && element.GetComposite().GetElements().Count > 0)
                        {
                            QueryPlanTree queryPlanTreeChild = new QueryPlanTree(childId++, 1, element, elements.Key, queryPlanTree, "", element.Cost, element.Cardinality, element.AccessType);
                            queryPlanTree.Childrens.Add(queryPlanTreeChild);
                            BuildTreeChilds(element, 1, queryPlanTreeChild);
                        }
                        else
                        {
                            QueryPlanTree queryPlanTreeChild = new QueryPlanTree(childId++, 1, element, elements.Key, queryPlanTree, element is AbstractLeaf ? ((AbstractLeaf)element).GetValue() : "", element.Cost, element.Cardinality, element.AccessType);
                            queryPlanTree.Childrens.Add(queryPlanTreeChild);
                        }
                }

            }
            return queryPlanTree;


        }

        private static int BuildTreeChilds(IQueryElement queryTree, int parentId, QueryPlanTree parent)
        {
            int childID = parentId + 1;

            parent.Childrens = new List<QueryPlanTree>();
            parent.Childrens.Find(p => p.ElementType == ElementType.CLASS_NAME);
            if (queryTree.GetComposite() != null && queryTree.GetComposite().GetElements() != null)
            {
                foreach (var elements in queryTree.GetComposite().GetElements())
                {
                    foreach (var element in elements.Value)
                        if (element.GetComposite() != null && element.GetComposite().GetElements() != null && element.GetComposite().GetElements().Count > 0)
                        {
                            QueryPlanTree queryPlanTreeChild = new QueryPlanTree(childID, parentId, element, elements.Key, parent, "", element.Cost, element.Cardinality, element.AccessType);
                            parent.Childrens.Add(queryPlanTreeChild);
                            childID = BuildTreeChilds(element, childID, queryPlanTreeChild);
                        }
                        else
                        {
                            parent.Childrens.Add(new QueryPlanTree(childID++, parentId, element, elements.Key, parent, element is AbstractLeaf ? ((AbstractLeaf)element).GetValue() : "", element.Cost, element.Cardinality, element.AccessType));
                        }
                }
            }
            return childID;
        }
        public void replaceComparision(QueryPlanTree queryPlanTree)
        {

            foreach (var queryPlanTreeChild in queryPlanTree.Childrens)
            {
              //  if (queryPlanTreeChild.ElementType.Equals(ElementType.COMPERISION)
                    ;
            }

        }
        public void find(QueryPlanTree queryPlanTree)
        {
            List<QueryPlanTree> found = new List<QueryPlanTree>();

            queryPlanTree.Childrens.ForEach(p =>
            {
                found.Add(findNext(ElementType.WHERE_OPERATION, found, p.Childrens));
            });
        }
        public QueryPlanTree findNext(ElementType elementType, List<QueryPlanTree> founded, List<QueryPlanTree> childs)
        {
            return childs.Find(p => p.ElementType.Equals(elementType) && !founded.Contains(p));
        }

        public void replace(QueryPlanTree first, QueryPlanTree  second){
            var temp = new QueryPlanTree(first);

            first.Cardinality = second.Cardinality;
            first.Childrens = second.Childrens;
            first.Cost = second.Cost;
            first.ElementType = second.ElementType;
            first.Id = second.Id;
            first.Parent = second.Parent;
            first.QueryElement = second.QueryElement;
            first.Value = second.Value;

            second.Cardinality = temp.Cardinality;
            second.Childrens = temp.Childrens;
            second.Cost = temp.Cost;
            second.ElementType = temp.ElementType;
            second.Id = temp.Id;
            second.Parent = temp.Parent;
            second.QueryElement = temp.QueryElement;
            second.Value = temp.Value;
        }

        public Dictionary<int, string> getIndexes(string className, List<string> attributes, QueryParameters queryParameters)
        {
            IIndexMechanism im = queryParameters.IndexMechanism;
           
            return im.GetIndexes().Where(p => im.GetTypesNameIndexedObjects(p.Key).Contains(className) &&
                                       im.GetIndexedAttribiutesForType(p.Key, className).All(s => attributes.Contains(s)))
                                  .ToDictionary(r=>r.Key, r=>r.Value);
        }

        public QueryPlan convertToQueryPlan(QueryPlanTree queryPlanTree)
        {
            QueryPlan queryPlan = new QueryPlan(queryPlanTree.Id, queryPlanTree.ParentId, queryPlanTree.ElementType.ToString(), queryPlanTree.Value, queryPlanTree.Cost, queryPlanTree.Cardinality);

            if (queryPlanTree.Childrens != null && queryPlanTree.Childrens.Count > 0)
            {
                queryPlan.Childrens = new List<QueryPlan>();
                foreach (QueryPlanTree queryPlanTreeChild in queryPlanTree.Childrens)
                    queryPlan.Childrens.Add(convertToQueryPlan(queryPlanTreeChild));
            }
            return queryPlan;
        }

        public QueryPlan GetQueryPlan(IQueryElement queryTree, QueryParameters queryParameters)
        {
            return convertToQueryPlan(BuildTreeSummary(queryTree));
        }

    }
}
