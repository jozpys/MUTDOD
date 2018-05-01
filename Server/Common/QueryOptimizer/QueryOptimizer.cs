using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.QueryTree;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MUTDOD.Server.Common.QueryOptimizer
{
    public class QueryOptimizer : Module, IQueryOptimizer
    {

        public string Name
        {
            get { return "QueryOptimizer"; }
        }

        public IQueryElement OptimizeQueryPlan(IQueryElement queryTree, QueryParameters queryParameters)
        {
            QueryPlanTree queryPlanTree = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                queryPlanTree = BuildTreeSummary(queryTree, queryParameters);
            }
            while (queryPlanTree == null && sw.ElapsedMilliseconds < 500000);

            return queryPlanTree != null ? queryPlanTree.QueryElement : queryTree;

        }

        public static QueryPlanTree BuildTreeSummary(IQueryElement queryTree, QueryParameters queryParameters)
        {
            QueryStack queryStack = new QueryStack();
            int childId = 1;

            QueryPlanTree queryPlanTree = new QueryPlanTree(childId, 0, queryTree, queryTree.ElementType, null, queryTree is AbstractLeaf ? ((AbstractLeaf)queryTree).GetValue() : "", queryTree.Cost, queryTree.Cardinality(queryParameters), queryTree.AccessType, queryTree.AccessObject);

            if (queryTree.GetComposite() != null)
            {
                queryPlanTree.Childrens = new List<QueryPlanTree>();
                foreach (var elements in queryTree.GetComposite().GetElements())
                {
                    foreach (var element in elements.Value)
                    {
                        queryStack.AddElement(element);

                        if (element.Optimize(queryParameters, queryStack))
                            return null;

                        if (element.GetComposite()?.GetElements()?.Count > 0)
                        {
                            QueryPlanTree queryPlanTreeChild = new QueryPlanTree(++childId, 1, element, elements.Key, queryPlanTree, "", element.Cost, element.Cardinality(queryParameters), element.AccessType, element.AccessObject);
                            queryPlanTree.Childrens.Add(queryPlanTreeChild);
                            if (BuildTreeChilds(element, childId, queryPlanTreeChild, queryParameters, queryStack) < 0)
                                return null;
                        }
                        else
                        {
                            QueryPlanTree queryPlanTreeChild = new QueryPlanTree(++childId, 1, element, elements.Key, queryPlanTree, element is AbstractLeaf ? ((AbstractLeaf)element).Value : "", element.Cost, element.Cardinality(queryParameters), element.AccessType, element.AccessObject);
                            queryPlanTree.Childrens.Add(queryPlanTreeChild);
                        }
                    }
                }

            }
            return queryPlanTree;

        }

        private static int BuildTreeChilds(IQueryElement queryTree, int parentId, QueryPlanTree parent, QueryParameters queryParameters, QueryStack queryStack)
        {
            int childID = parentId + 1;

            parent.Childrens = new List<QueryPlanTree>();
            if (queryTree.GetComposite()?.GetElements() != null)
            {
                foreach (var elements in queryTree.GetComposite().GetElements())
                {
                    foreach (var element in elements.Value)
                    {
                        queryStack.AddElement(element);

                        if (element.Optimize(queryParameters, queryStack))
                            return -1;

                        if (element.GetComposite()?.GetElements()?.Count > 0)
                        {
                            QueryPlanTree queryPlanTreeChild = new QueryPlanTree(childID, parentId, element, elements.Key, parent, "", element.Cost, element.Cardinality(queryParameters), element.AccessType, element.AccessObject);
                            parent.Childrens.Add(queryPlanTreeChild);
                            childID = BuildTreeChilds(element, childID, queryPlanTreeChild, queryParameters, queryStack);
                            if (childID < 0)
                                return childID;
                        }
                        else
                        {
                            parent.Childrens.Add(new QueryPlanTree(childID++, parentId, element, elements.Key, parent, element is AbstractLeaf ? ((AbstractLeaf)element).Value : "", element.Cost, element.Cardinality(queryParameters), element.AccessType, element.AccessObject));
                        }
                    }
                }
            }
            return childID;
        }

        public Dictionary<int, string> GetIndexes(string className, List<string> attributes, QueryParameters queryParameters)
        {
            IIndexMechanism<string> im = queryParameters.IndexMechanism;

            return im.GetIndexes().Where(p => im.GetTypesNameIndexedObjects(p.Key).Contains(className) &&
                                       im.GetIndexedAttribiutesForType(p.Key, className).All(s => attributes.Contains(s)))
                                  .ToDictionary(r => r.Key, r => r.Value);
        }

        public QueryPlan ConvertToQueryPlanSummary(QueryPlanTree queryPlanTree)
        {
            var _sumCost = 0;
            QueryPlan queryPlan = new QueryPlan(queryPlanTree.Id, queryPlanTree.ParentId, queryPlanTree.ElementType.ToString(), queryPlanTree.Value, queryPlanTree.Cost, queryPlanTree.Cardinality, queryPlanTree.AccessType.ToString(), queryPlanTree.AccessObject);
            _sumCost += queryPlanTree.Cost;

            if (queryPlanTree.Childrens != null && queryPlanTree.Childrens.Count > 0)
            {
                queryPlan.Childrens = new List<QueryPlan>();
                foreach (QueryPlanTree queryPlanTreeChild in queryPlanTree.Childrens)
                {
                    var queryPlanConverted = ConvertToQueryPlan(queryPlanTreeChild, out int sumCost);
                    _sumCost += sumCost;
                    queryPlan.Childrens.Add(queryPlanConverted);
                }
            }

            queryPlan.Cost = _sumCost;
            return queryPlan;
        }

        public QueryPlan ConvertToQueryPlan(QueryPlanTree queryPlanTree,out int sumCost)
        {
            QueryPlan queryPlan = new QueryPlan(queryPlanTree.Id, queryPlanTree.ParentId, queryPlanTree.ElementType.ToString(), queryPlanTree.Value, queryPlanTree.Cost, queryPlanTree.Cardinality, queryPlanTree.AccessType.ToString(), queryPlanTree.AccessObject);

            int _sumCost = queryPlanTree.Cost;

            if (queryPlanTree.Childrens != null && queryPlanTree.Childrens.Count > 0)
            {
                queryPlan.Childrens = new List<QueryPlan>();
                foreach (QueryPlanTree queryPlanTreeChild in queryPlanTree.Childrens)
                {
                    var queryPlanConverted = ConvertToQueryPlan(queryPlanTreeChild, out sumCost);
                    _sumCost += sumCost;
                    queryPlan.Childrens.Add(queryPlanConverted);
                }
                   
            }
            sumCost = _sumCost;
            return queryPlan;
        }

        public QueryPlan GetQueryPlan(IQueryElement queryTree, QueryParameters queryParameters)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            QueryPlanTree queryPlanTree = null;
            do
            {
                queryPlanTree = BuildTreeSummary(queryTree, queryParameters);
            }
            while (queryPlanTree == null && sw.ElapsedMilliseconds < 500000);

            QueryPlan queryPlan = ConvertToQueryPlanSummary(queryPlanTree);
            return queryPlan;
        }

    }
}
