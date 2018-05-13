using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Settings;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryEngineModule.Core
{
    internal class CentralServerExecuter : EngineExecuter
    {
        private readonly IDatabaseParameters _database;
        private Action<IQueryElement> _doOnDataServers;
        private Action<string, MessageLevel> _log;
        private SystemInfo _systemInfo;
        private readonly IStorage _storage;
        private readonly ISettingsManager _settingsManager;

        public CentralServerExecuter(IDatabaseParameters database, Action<IQueryElement> doOnDataServers,
            SystemInfo systemInfo, IStorage storage,
            ISettingsManager settingsManager, Action<string, MessageLevel> log)
            : base(database, storage, log)
        {
            _database = database;
            _doOnDataServers = doOnDataServers;
            _systemInfo = systemInfo;
            _storage = storage;
            _settingsManager = settingsManager;
            _log = log;
        }

        internal override DTOQueryResult Execute(IQueryElement queryTree)
        {
            QueryParameters parameters = new QueryParameters {
                Database = _database,
                SystemInfo = _systemInfo,
                Storage = _storage,
                SettingsManager = _settingsManager,
                Log = _log };
            if (_doOnDataServers != null)
                _doOnDataServers(queryTree);
            QueryDTO result = queryTree.Execute(parameters);
            return result.Result;
        }

        internal override DTOQueryResult Execute(IQueryTree queryTree)
        {
            switch (queryTree.TokenName)
            {
                case TokenName.STATEMENT:
                    DTOQueryResult qr = null;
                    DTOQueryResult lastQr = null;
                    foreach (var subTree in queryTree.ProductionsList.Where(p => p.TokenName != TokenName.SEMICOLON))
                    {
                        var res = Execute(subTree);
                        if (qr == null)
                            lastQr = qr = res;
                        else
                        {
                            lastQr.NextResult = res;
                            lastQr = lastQr.NextDTOResult;
                        }
                    }
                    return qr;
                case TokenName.SYSTEM_OPERATION:
                    return Execute(queryTree.ProductionsList[0]);
                case TokenName.GET_SYSTEM_INFO:
                    var sb = new StringBuilder();
                    var sw = new StringWriter(sb);
                    var xmlSerializer = new XmlSerializer(typeof (SystemInfo));
                    xmlSerializer.Serialize(sw, _systemInfo);
                    return new DTOQueryResult()
                    {
                        NextResult = null,
                        QueryResults = null,
                        QueryResultType = ResultType.SystemInfo,
                        StringOutput = sb.ToString()
                    };
                case TokenName.CREATE_DATABASE:
                    try
                    {
                        var dbName = queryTree.ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;
                        var did = _storage.CreateDatabase(new DatabaseParameters(dbName, _settingsManager));
                        _log(string.Format("new database created as {0}", did), MessageLevel.Info);
                        var sb2 = new StringBuilder();
                        var sw2 = new StringWriter(sb2);
                        var xmlSerializer2 = new XmlSerializer(typeof (DatabaseInfo));
                        var db = _storage.GetDatabase(did);
                        xmlSerializer2.Serialize(sw2,
                            new DatabaseInfo()
                            {
                                Name = db.Name,
                                Classes = db.Schema.Classes.Select(c => new DatabaseClass
                                {
                                    Name = c.Value.Name,
                                    Interface = c.Value.Interface,
                                    Fields =
                                        db.Schema.Properties.Values.Where(p => p.ParentClassId == c.Value.ClassId.Id)
                                        .Select(f => new Field { Name = f.Name, Type = f.Type })
                                        .ToList(),
                                    Methods =
                                        db.Schema.Methods.ContainsKey(c.Key)
                                            ? db.Schema.Methods[c.Key]
                                            : new List<string>()
                                }).ToList()
                            });
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.DatabaseInfo,
                            StringOutput = sb2.ToString()
                        };

                    }
                    catch (Exception ex)
                    {
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Error during database creation: " + ex.ToString()
                        };
                    }
                case TokenName.CLASS_DECLARATION:
                case TokenName.INTERFACE_DECLARATION:
                    if (_database == null)
                    {
                        _log("Database is required!", MessageLevel.Error);
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Error ocured while class creation"
                        };
                    }
                    bool isClass = queryTree.TokenName == TokenName.CLASS_DECLARATION;
                    var desc = isClass ? "Class" : "Interface";
                    var newClassName =
                        (queryTree.TokenName == TokenName.CLASS_DECLARATION
                            ? queryTree.ProductionsList.Single(t => t.TokenName == TokenName.CLASS_NAME)
                            : queryTree)
                            .ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;

                    if (_database.Schema.Classes.Any(c => c.Value.Name.ToUpper() == newClassName.ToUpper()))
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = desc + " with name: " + newClassName + " arleady exists!"
                        };
                    var classId = new ClassId
                    {
                        Name = newClassName,
                        Id = (_database.Schema.Classes.Max(d => (long?) d.Key.Id) ?? 0) + 1
                    };
                    var classDef = new Class
                    {
                        ClassId = classId,
                        Name = newClassName
                    };
                    foreach (var attr in queryTree.ProductionsList.Where(t => t.TokenName == TokenName.ATTRIBUTE_DEC_STM)
                        )
                    {
                        var attrName = (isClass
                            ? attr.ProductionsList.Single(t => t.TokenName == TokenName.ATTRIBUTE_NAME)
                            : attr)
                            .ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;
                        var typeName =
                            attr.ProductionsList.Single(t => t.TokenName == TokenName.DATA_TYPE)
                                .ProductionsList.Single();
                        var propertyId = new PropertyId
                        {
                            Id = 1 + _database.Schema.Properties.Max(p => (long?) p.Key.Id) ?? 0,
                            Name = attrName,
                            ParentClassId = classId.Id
                        };
                        _database.Schema.Properties.TryAdd(propertyId, new Property
                        {
                            ParentClassId = classId.Id,
                            Name = attrName,
                            PropertyId = propertyId,
                            Type = typeName.TokenValue,
                            IsValueType = typeName.TokenName != TokenName.NAME
                        });
                    }
                    _database.Schema.Methods.TryAdd(classId, new List<string>());
                    foreach (var meth in queryTree.ProductionsList.Where(t => t.TokenName == TokenName.METHOD_DEC_STM))
                    {
                        var methName = (isClass
                            ? meth.ProductionsList.Single(t => t.TokenName == TokenName.METHOD_NAME)
                            : meth)
                            .ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;
                        _database.Schema.Methods[classId].Add(methName);
                    }
                    foreach (var rel in queryTree.ProductionsList.Where(t => t.TokenName == TokenName.RELATION_DEC_STM))
                    {
                        var attrName = rel.ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;
                        string typeName =
                            rel.ProductionsList.Single(t => t.TokenName == TokenName.DATA_TYPE)
                                .ProductionsList.Single()
                                .TokenValue;
                        var propertyId = new PropertyId
                        {
                            Id = 1 + (_database.Schema.Properties.Max(p => (long?) p.Key.Id) ?? 0),
                            Name = attrName,
                            ParentClassId = classId.Id
                        };
                        _database.Schema.Properties.TryAdd(propertyId, new Property
                        {
                            ParentClassId = classId.Id,
                            Name = attrName,
                            PropertyId = propertyId,
                            Type = typeName,
                            IsValueType = false
                        });
                    }
                    if (!_database.Schema.Classes.TryAdd(classId, classDef))
                    {
                        _log("Could not define new " + desc.ToLower(), MessageLevel.Error);
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Error ocured while class creation"
                        };
                    }
                    _storage.SaveSchema(_database.Schema);
                    _log("Defined new class: " + newClassName, MessageLevel.QueryExecution);
                    return new DTOQueryResult()
                    {
                        NextResult = null,
                        QueryResults = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "New " + desc.ToLower() + ": " + newClassName + " created."
                    };
                case TokenName.DROP:
                    if (_database == null)
                    {
                        _log("Database is required!", MessageLevel.Error);
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Error ocured while class droping"
                        };
                    }

                    var className = queryTree.ProductionsList.Single(t => t.TokenName == TokenName.CLASS_NAME)
                        .ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;

                    Class classToDrop = GetClass(className);
                    Class dropedClass;

                    if (!_database.Schema.Classes.TryRemove(classToDrop.ClassId, out dropedClass))
                    {
                        _log("Could not define new", MessageLevel.Error);
                        return new DTOQueryResult()
                        {
                            NextResult = null,
                            QueryResults = null,
                            QueryResultType = ResultType.StringResult,
                            StringOutput = "Error ocured while class droping"
                        };
                    }
                    _storage.SaveSchema(_database.Schema);

                    _log("Droped class: " + dropedClass.Name, MessageLevel.QueryExecution);
                    return new DTOQueryResult()
                    {
                        NextResult = null,
                        QueryResults = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Class:" + dropedClass.Name + " droped."
                    };
                case TokenName.NEW_OBJECT:
                    return base.Execute(queryTree);
                default:
                    //if (_doOnDataServers != null)
                    //    _doOnDataServers(queryTree);
                    return base.Execute(queryTree);
            }
        }
    }
}
