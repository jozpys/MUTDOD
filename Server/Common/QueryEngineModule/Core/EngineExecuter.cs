using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryEngineModule.Core
{
    internal class EngineExecuter
    {
        private readonly IDatabaseParameters _database;
        private readonly IStorage _storage;
        private readonly Action<string, MessageLevel> _log;

        public EngineExecuter(IDatabaseParameters database, IStorage storage, Action<string, MessageLevel> log)
        {
            _database = database;
            _storage = storage;
            _log = log;
        }

        internal virtual DTOQueryResult Execute(IQueryTree queryTree)
        {
            switch (queryTree.TokenName)
            {
                case TokenName.NEW_OBJECT:
                    var className = GetClassName(queryTree.ProductionsList.Single(q => q.TokenName == TokenName.CLASS_NAME));
                    var objectClass = GetClass(className);
                    if (objectClass == null)
                        return new DTOQueryResult
                        {
                            NextResult = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Unknown class: " + className
                        };
                    var propeteries =
                        _database.Schema.Properties.Select(p => p.Value)
                            .Where(p => p.ParentClassId == objectClass.ClassId.Id)
                            .ToList();
                    var oid = new Oid(Guid.NewGuid(), _database.DatabaseId.Dli);
                    var toStore = new Storable {Oid = oid};

                    var attr =
                        queryTree.ProductionsList.SingleOrDefault(
                            q => q.TokenName == TokenName.OBJECT_INITIALIZATION_ATTRIBUTES_LIST);
                    if (attr != null)
                    {
                        foreach (
                            var attrToSet in
                                attr.ProductionsList.Where(q => q.TokenName == TokenName.OBJECT_INITIALIZATION_ELEMENT))
                        {
                            var field =
                                attrToSet.ProductionsList.Single(q => q.TokenName == TokenName.ATTRIBUTE_NAME)
                                    .ProductionsList.Single()
                                    .TokenValue;
                            var property = propeteries.SingleOrDefault(p => p.Name == field);
                            if (property == null)
                                return new DTOQueryResult
                                {
                                    NextResult = null,
                                    QueryResultType = ResultType.StringResult,
                                    StringOutput = "Unknown field: " + field
                                };
                            var literal = attrToSet.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.LITERAL);
                            if (literal != null)
                                toStore.Properties.Add(property, GetLiteral(literal));
                        }
                    }

                    _storage.Save(_database.DatabaseId, toStore);
                    _log("new object saved with id: " + oid, MessageLevel.QueryExecution);
                    return new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "new object saved with id: " + oid
                    };
                case TokenName.GET:
                    var get_stm = queryTree.ProductionsList.Where(q => q.TokenName == TokenName.GET_STM);
                    if (get_stm.Count() == 1)
                        return Execute(get_stm.Single());
                    goto default;
                case TokenName.GET_STM:
                    var classNameToGet =
                        GetClassName(
                            queryTree.ProductionsList.Single(q => q.TokenName == TokenName.GET_HEADER)
                                .ProductionsList.Single(q => q.TokenName == TokenName.CLASS_NAME));
                    var classToGet = GetClass(classNameToGet);
                    if (classToGet == null)
                        return new DTOQueryResult
                        {
                            NextResult = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Unknown class: " + classNameToGet
                        };
                    var objs = _storage.GetAll(_database.DatabaseId);
                    objs = objs.Where(s => s.Properties.All(p => p.Key.ParentClassId == classToGet.ClassId.Id));
                    var seachCriteria = queryTree.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.WHERE_CLAUSE);
                    if (seachCriteria != null)
                    {
                        var wc = BuildWhereCriteria(seachCriteria);
                        objs = objs.Where(s => wc.All(f => f(s)));
                    }

                    var deref = queryTree.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.K_DEREF);
                    if(deref!=null)
                        return new DTOQueryResult
                        {
                            NextResult = null,
                            QueryResultType = ResultType.Default,
                            StringOutput = ToXml(objs,classToGet).OuterXml
                        };

                    return new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.ReferencesOnly,
                        QueryResults = objs.Select(o => o.Oid).ToList()
                    };
                default:
                    return new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Your query is correct. The execution is not ready yet"
                    };
            }
        }

        private XmlDocument ToXml(IEnumerable<IStorable> toSave, Class @class)
        {
            var doc = new XmlDocument();
            var root = (XmlElement)doc.AppendChild(doc.CreateElement("result"));
            foreach (var obj in toSave)
            {
                var el = (XmlElement) root.AppendChild(doc.CreateElement(@class.Name));
                el.AppendChild(doc.CreateElement("Oid")).InnerText = obj.Oid.Id.ToString();
                foreach (var p in obj.Properties)
                    el.AppendChild(doc.CreateElement(p.Key.Name)).InnerText = p.Value.ToString();
            }
            return doc;
        }

        private List<Func<IStorable, bool>> BuildWhereCriteria(IQueryTree queryTree)
        {
            switch (queryTree.TokenName)
            {
                case TokenName.WHERE_CLAUSE:
                    var ret = new List<Func<IStorable, bool>>();
                    foreach (var subTree in queryTree.ProductionsList)
                        BuildWhereCriteria(subTree).ForEach(ret.Add);
                    return ret;
                case TokenName.AND:
                case TokenName.OR:
                case TokenName.K_WHERE:
                    return new List<Func<IStorable, bool>>();
                case TokenName.AND_OR_CLAUSE:
                case TokenName.CLAUSE:
                    var ret2 = new List<Func<IStorable, bool>>();
                    foreach (var subTree in queryTree.ProductionsList)
                        BuildWhereCriteria(subTree).ForEach(ret2.Add);
                    return ret2;
                case TokenName.WHERE_OPERATION:
                    var wv =
                        queryTree.ProductionsList.Single(q => q.TokenName == TokenName.WHERE_VALUE);
                    var leftLiteral = wv.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.LITERAL);
                    var leftField = wv.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.NAME);
                    if (leftLiteral == null & leftField == null)
                        return new List<Func<IStorable, bool>>(); //TODO!
                    var wt =
                        queryTree.ProductionsList.Single(q => q.TokenName == TokenName.WHERE_TAIL);
                    var wo = wt.ProductionsList.Single(q => q.TokenName == TokenName.WHERE_OPERATOR);
                    if (wo.ProductionsList.Count > 1)
                        throw new ApplicationException(
                            "No support for other tokens then IS_NULL, IS_NOT_NULL and COMPARISON_OPERATOR");
                    var param = Expression.Parameter(typeof (IStorable));
                    if (wo.ProductionsList.Single().TokenName == TokenName.IS_NULL)
                    {
                        if (leftLiteral != null)
                        {
                            var left = Expression.Constant(GetLiteral(leftLiteral));
                            var clasue = Expression.Equal(left, Expression.Constant(null));

                            return
                                new List<Func<IStorable, bool>>(new[]
                                {Expression.Lambda<Func<IStorable, bool>>(clasue, param).Compile()});
                        }
                        else
                            return new List<Func<IStorable, bool>>(new Func<IStorable, bool>[]
                            {
                                storable =>
                                {
                                    var f = storable.Properties.Where(p => p.Key.Name == leftField.TokenValue);
                                    return !f.Any()
                                           || f.Single().Value == null;
                                }
                            });
                    }
                    if (wo.ProductionsList.Single().TokenName == TokenName.IS_NOT_NULL)
                    {
                        if (leftLiteral != null)
                        {
                            var left = Expression.Constant(GetLiteral(leftLiteral));
                            var clasue = Expression.NotEqual(left, Expression.Constant(null));

                            return
                                new List<Func<IStorable, bool>>(new[]
                                {Expression.Lambda<Func<IStorable, bool>>(clasue, param).Compile()});
                        }
                        else
                            return new List<Func<IStorable, bool>>(new Func<IStorable, bool>[]
                            {
                                storable =>
                                {
                                    var f = storable.Properties.Where(p => p.Key.Name == leftField.TokenValue);
                                    return f.Count() == 1
                                           && f.Single().Value != null;
                                }
                            });
                    }
                    if (wo.ProductionsList.Single().TokenName == TokenName.COMPARISON_OPERATOR)
                    {
                        var rightwv = wt.ProductionsList.Single(q => q.TokenName == TokenName.WHERE_VALUE);
                        var rightLiteral = rightwv.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.LITERAL);
                        var rightField = rightwv.ProductionsList.SingleOrDefault(q => q.TokenName == TokenName.NAME);

                        if (leftLiteral != null && rightLiteral != null)
                        {
                            var left = Expression.Constant(GetLiteral(leftLiteral));
                            var right = Expression.Constant(GetLiteral(rightLiteral));
                            var clasue = GetComparationExpression(wo.ProductionsList.Single().ProductionsList.Single().TokenName, left,
                                right);

                            return
                                new List<Func<IStorable, bool>>(new[]
                                {Expression.Lambda<Func<IStorable, bool>>(clasue, param).Compile()});
                        }
                        else
                            return new List<Func<IStorable, bool>>(new Func<IStorable, bool>[]
                            {
                                storable =>
                                {
                                    Expression left;
                                    Expression right;
                                    if (leftField != null)
                                    {
                                        var f =
                                            storable.Properties.Where(p => p.Key.Name == leftField.TokenValue);
                                        if (!f.Any())
                                            return false;
                                        var prop = f.Single();
                                        left = Expression.Constant(prop.Value, prop.Key.DotNetType);
                                    }
                                    else left = Expression.Constant(GetLiteral(leftLiteral));
                                    if (rightField != null)
                                    {
                                        var f =
                                            storable.Properties.Where(p => p.Key.Name == rightField.TokenValue);
                                        if (!f.Any())
                                            return false;
                                        var prop = f.Single();
                                        right = Expression.Constant(prop.Value, prop.Key.DotNetType);
                                    }
                                    else right = Expression.Constant(GetLiteral(rightLiteral));
                                    var clasue = GetComparationExpression(
                                        wo.ProductionsList.Single().ProductionsList.Single().TokenName, left, right);

                                    return
                                        Expression.Lambda<Func<IStorable, bool>>(clasue, param)
                                            .Compile()(storable);
                                }
                            });
                    }
                    else
                        throw new ApplicationException(
                            "Unknown WHERE_OPERATION token: " + wo.ProductionsList.Single().TokenName);
                default:
                    throw new ApplicationException("Unknown WHERE_CLAUSE token: " + queryTree.TokenName);
            }
        }

        private Expression GetComparationExpression(TokenName tokenName, Expression left, Expression right)
        {
            switch (tokenName)
            {
                case TokenName.GREATER:
                    return Expression.GreaterThan(left, right);
                case TokenName.LESS:
                    return Expression.LessThan(left, right);
                case TokenName.GREATER_EQUAL:
                    return Expression.GreaterThanOrEqual(left, right);
                case TokenName.LESS_EQUAL:
                    return Expression.LessThanOrEqual(left, right);
                case TokenName.ISEQUAL:
                    return Expression.Equal(left, right);
                case TokenName.NOT_EQUAL:
                    return Expression.NotEqual(left, Expression.Constant(null));
                default:
                    throw new ApplicationException("Unknown COMPARISON_OPERATOR: " + tokenName);
            }
        }

        private string GetClassName(IQueryTree queryTree)
        {
            return queryTree.ProductionsList.Single().TokenValue;
        }

        protected Class GetClass(string className)
        {
            if (_database.Schema == null)
                throw new ApplicationException("Schema can not be null!");
            return _database.Schema.Classes.Select(c => c.Value).SingleOrDefault(c => c.Name == className);
        }

        protected object GetLiteral(IQueryTree literalQueryTree)
        {
            switch (literalQueryTree.TokenName)
            {
                case TokenName.LITERAL:
                    return GetLiteral(literalQueryTree.ProductionsList.Single());
                case TokenName.NUMBER:
                    var IsFloat = literalQueryTree.ProductionsList.Any(q => q.TokenName == TokenName.FLOAT_PRESICION);
                    var number = GetNumber(literalQueryTree);
                    return IsFloat
                        ? (object)Double.Parse(number, NumberStyles.Number, CultureInfo.InvariantCulture)
                        : Int32.Parse(number, NumberStyles.Number, CultureInfo.InvariantCulture);
                case TokenName.STRING_VALUE:
                    return literalQueryTree.TokenValue;
                case TokenName.BOOL_VALUE:
                    if (literalQueryTree.TokenValue.ToUpper() == "TRUE")
                        return true;
                    else if (literalQueryTree.TokenValue.ToUpper() == "FALSE")
                        return false;
                    else
                        throw new ApplicationException("Unknown BOOL_VALUE token value: " + literalQueryTree.TokenValue);
                case TokenName.NULL_VALUE:
                    return null;
                default:
                    throw new ApplicationException("Unknown LITERAL token: " + literalQueryTree.TokenName);
            }
        }

        protected string GetNumber(IQueryTree numerQueryTree)
        {
            if (numerQueryTree.ProductionsList == null)
                return numerQueryTree.TokenValue;

            var sb = new StringBuilder();
            numerQueryTree.ProductionsList.ToList().ForEach(q => sb.Append(GetNumber(q)));
            return sb.ToString();
        }

        private class SearchCritera : ISearchCriteria
        {

        }
    }
}
