using System;
using System.Collections.Generic;
using System.Linq;
using MUTDOD.Common;
using MUTDOD.Server.Common.QueryAnalyzer.MetamodelHelper;
using MUTDOD.Server.Common.QueryAnalyzer.SyntaxAnalyzer;

namespace MUTDOD.Server.Common.QueryAnalyzer.SemanticAnalyzer
{
    internal class SemanticAnalyzer
    {

        public void CheckQuerySemantic(QueryTree queryTree, Database databaseMetamodel)
        {
            Environment environment = new Environment{
                classList = databaseMetamodel.classes,
                parent = null,
                section= Section.Target,
                labelList = new List<string>(),
                fields = new Dictionary<string,string>()
            };

            List<SemanticExceptionItem> exceptions = analize(queryTree, environment);
            if (exceptions.Count > 0)
                throw new SemanticException
                {
                    exceptionList = exceptions
                };
        }

        private List<SemanticExceptionItem> analize(IQueryTree queryTree, Environment environment)
        {
            List<SemanticExceptionItem> exceptions = new List<SemanticExceptionItem>();

            string label;
            int value;
            string className;
            string parentClassName;
            Environment innerSection;

            switch (queryTree.TokenName)
            {
                case "Target":
                case "TermBlockTail":
                case "Attributes_Block_Tail":
                case "Loop":
                    exceptions.AddRange(analize(queryTree.ProductionsList.Single(), environment));
                    break;
                case "Block_Expr":
                case "Expression":
                    if(queryTree.ProductionsList.Where(p => p.TokenValue == string.Empty).Count() == 1)
                        exceptions.AddRange(analize(queryTree.ProductionsList.Where(p => p.TokenValue == string.Empty).Single(), environment));
                    break;
                case "Term_Block":
                case "Expr":
                case "Get_Expression":
                    foreach (QueryTree qt in queryTree.ProductionsList)
                    {
                        exceptions.AddRange(analize(qt, environment));
                    }
                    break;
                case "COMMIT":
                case "ROLLBACK":
                case "Transaction_Expr":
                    break;
                case "Goto_Expr":
                    label = queryTree.ProductionsList.Where(p => p.TokenName=="Label_Name").Single().ProductionsList.Single().TokenValue; 
                    if(!CheckEnvironmentForLabel(label, environment))
                        exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, string.Format("Unknown label '{0}'", label), queryTree.TokenLine, queryTree.TokenCol));
                    break;
                case "Label_Statement":
                    label = queryTree.ProductionsList.Where(p => p.TokenName == "Label_Name").Single().ProductionsList.Single().TokenValue;
                    IQueryTree labelQuery = queryTree.ProductionsList.Where(p => p.TokenName == "Label_Name").Single().ProductionsList.Single();
                    switch (environment.section)
                    {
                        case Section.Class_Declaration:
                            exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, "Label declaration is forbidden", queryTree.TokenLine, queryTree.TokenCol));
                            break;
                        case Section.Method_Declaration:
                        case Section.For_Statement:
                        case Section.Foreach_Statement:
                        case Section.If_Statement:
                        case Section.Target:
                        case Section.Try_Catch:
                        case Section.While_Statement:
                        case Section.Parallel:
                            if (environment.labelList.Contains(label))
                                exceptions.Add(new SemanticExceptionItem(labelQuery.TokenName, string.Format("Duplicated label name '{0}'", label), labelQuery.TokenLine, labelQuery.TokenCol));
                            else
                                environment.labelList.Add(label);
                            break;
                        default:
                            throw new Exception(string.Format("Unexpected section '{0}'", environment.section.ToString()));
                    }
                    break;
                case "Parallel_Statement":
                    if (Int32.Parse(queryTree.ProductionsList.Where(p => p.TokenName == "INTEGER_VALUE").Single().TokenValue) <= 0)
                        exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, "Parallel number must be positive", queryTree.TokenLine, queryTree.TokenCol));                    
                    innerSection = new Environment{
                        parent = environment,
                        section = Section.Parallel,
                        labelList = new List<string>(),
                        fields = new Dictionary<string,string>(),
                        classList = new List<Class>()};
                    exceptions.AddRange(analize(queryTree.ProductionsList.Where(p => p.TokenName == "Term_Block").Single(), innerSection));
                    break;

                case "Class_Declaration":
                    exceptions.AddRange(analize(queryTree.ProductionsList.Where(p => p.TokenName == "Class_Header").Single(), environment));                    
                    break;
                case "Class_Header":
                    className = queryTree.ProductionsList.Where(p => p.TokenName == "NAME").Single().ProductionsList.Single().TokenValue;
                    if (CheckEnvironmentForClass(className, environment))
                        exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, string.Format("Duplicated class name '{0}'", className), queryTree.TokenLine, queryTree.TokenCol));
                    if (queryTree.ProductionsList.Where(p => p.TokenName == "Base_Class").Count() > 0)
                    {
                        parentClassName = queryTree.ProductionsList.Where(p => p.TokenName == "Base_Class").Single().ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenValue;
                        if (!CheckEnvironmentForClass(parentClassName, environment))
                            exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, string.Format("Unknown type '{0}'", parentClassName), queryTree.TokenLine, queryTree.TokenCol));
                    }
                    break;
                case "Get_Factor":
                    foreach (QueryTree qt in queryTree.ProductionsList.Where(p=>(p.ProductionsList == null ? 0 : p.ProductionsList.Count) > 0))
                    {
                        exceptions.AddRange(analize(qt, environment));
                    }
                    break; 
                case "Element_Cardinality":
                    if (Int32.Parse(queryTree.ProductionsList.Where(p => p.TokenName == "Cardinality_Expr").Single().ProductionsList.Where(p=>p.TokenName == "INTEGER_VALUE").Single().TokenValue) <= 0)
                        exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, "Number of objects must be positive", queryTree.TokenLine, queryTree.TokenCol));
                    break;
                case "Get_Statement":
                    if(queryTree.ProductionsList.Where(p => p.TokenName == "Element_Cardinality").Count() > 0)
                        exceptions.AddRange(analize(queryTree.ProductionsList.Where(p => p.TokenName == "Element_Cardinality").Single(), environment));

                    className = queryTree.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenValue;
                    if (!CheckEnvironmentForClass(className, environment))
                        exceptions.Add(new SemanticExceptionItem(queryTree.TokenName, string.Format("Unknown class name '{0}'", className), queryTree.TokenLine, queryTree.TokenCol));
                    else
                    {
                        Class selectedClass = GetClassFromEnvironment(className, environment);
                        if (selectedClass != null)
                        {
                            IQueryTree getListItems = queryTree.ProductionsList.Where(p => p.TokenName == "Get_List").Single().ProductionsList.Where(p => p.TokenName == "Get_List_Item").Single();
                            exceptions.AddRange(CheckGetListItemsInEnvironment(selectedClass, getListItems, environment));
                            if (queryTree.ProductionsList.Where(p => p.TokenName == "Where_Clause").Count() > 0)
                                exceptions.AddRange(CheckClauseExpressionInEnvironment(selectedClass, queryTree.ProductionsList.Where(p => p.TokenName == "Where_Clause").Single().ProductionsList.Where(p => p.TokenName == "Clause_Expr").Single(), environment));
                        }
                    }                    
                    break;
                case "Foreach_Statement":
                    innerSection = new Environment
                    {
                        parent = environment,
                        section = Section.Foreach_Statement,
                        labelList = new List<string>(),
                        fields = new Dictionary<string, string>(),
                        classList = new List<Class>()
                    };
                    IQueryTree forachHead = queryTree.ProductionsList.Where(p => p.TokenName == "Foreach_Head").Single();
                    if (CheckEnvironmentForClass(forachHead.ProductionsList.Where(p => p.TokenName == "Type").Single().ProductionsList.First().TokenValue, environment))
                    {
                        string typeName = forachHead.ProductionsList.Where(p => p.TokenName == "Type").Single().ProductionsList.First().TokenValue;
                        string classN = forachHead.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenValue;
                        innerSection.fields.Add(classN, typeName);
                    }

                    IQueryTree collSource = forachHead.ProductionsList.Where(p => p.TokenName == "Collection_Source").Single();
                    foreach (QueryTree q in collSource.ProductionsList)
                    {
                        if(q.TokenName == "Get_Expression")
                            exceptions.AddRange(analize(q, environment));
                        else if (q.TokenName == "Expression_Factor")
                        {
                            IQueryTree exItem = q.ProductionsList.Where(p=>p.TokenValue != string.Empty).First();
                            switch (exItem.TokenName)
                            {
                                case "Expression_Atom":
                                    if(!CheckEnvironmentForFields(exItem.ProductionsList.First().TokenValue, environment))
                                        exceptions.Add(new SemanticExceptionItem(exItem.ProductionsList.First().TokenName, String.Format("Incorrect data collection: {}", exItem.ProductionsList.First().TokenValue), exItem.ProductionsList.First().TokenLine, exItem.ProductionsList.First().TokenCol));
                                    break;
                                case "Object_Selection_Path":
                                    Class returnedClass;
                                    if(CheckEnvironmentForClass(exItem.ProductionsList.First().TokenValue, environment))
                                    {
                                        Class newClass = GetClassFromEnvironment(exItem.ProductionsList.First().TokenValue, environment);
                                        exceptions.AddRange(CheckPathTailInEnvironment(newClass, exItem.ProductionsList.Where(p=>p.TokenName == "Path_Tail").Single(), environment, out returnedClass));                                         
                                    }
                                    else if (CheckEnvironmentForFields(exItem.ProductionsList.First().TokenValue, environment))
                                    {
                                        string fieldType = environment.fields[exItem.ProductionsList.First().TokenValue];
                                        Class newClass = GetClassFromEnvironment(fieldType, environment);
                                        exceptions.AddRange(CheckPathTailInEnvironment(newClass, exItem.ProductionsList.Where(p => p.TokenName == "Path_Tail").Single(), environment, out returnedClass));
                                    }
                                    break;
                                case "Binary_Expr":
                                    exceptions.Add(new SemanticExceptionItem(exItem.ProductionsList.First().TokenName, String.Format("Incorrect data collection: {}", exItem.ProductionsList.First().TokenValue), q.ProductionsList.First().TokenLine, q.ProductionsList.First().TokenCol+1));
                                    break;
                            }
                        }
                    }

                    if (queryTree.ProductionsList.Where(p => p.TokenName == "Term_Block").Count() == 1)
                    {
                        IQueryTree term = queryTree.ProductionsList.Where(p => p.TokenName == "Term_Block").Single();
                        exceptions.AddRange(analize(term, innerSection));
                    }
                    break;


                default:
                    break;
            }

            return exceptions;
        }

        private bool CheckEnvironmentForLabel(string label, Environment ev)
        {
            bool ret = false;
            Environment e = ev;

            while (!ret && e != null)
            {
                if (e.labelList.Contains(label))
                    ret = true;
                e = e.parent;
            }

            return ret;
        }

        private bool CheckEnvironmentForClass(string className, Environment ev)
        {
            bool ret = false;
            Environment e = ev;

            while (!ret && e != null && e.classList != null)
            {
                if (e.classList.Where(p => p.name == className).Count() > 0)
                    ret = true;
                e = e.parent;
            }

            return ret;
        }

        private bool CheckEnvironmentForFields(string field, Environment ev)
        {
            bool ret = false;
            Environment e = ev;

            while (!ret && e != null)
            {
                if (e.fields.Where(p => p.Key == "field").Count() > 0)
                    ret = true;
                if(e.section != Section.Class_Declaration)
                    e = e.parent;
            }

            return ret;
        }

        private Class GetClassFromEnvironment(string className, Environment ev)
        {
            Class seletedClass = null;
            Environment e = ev;
            int genericMarkPosition = className.IndexOf('<');

            while (seletedClass == null && e != null)
            {
                if (genericMarkPosition > 0 &&
                    e.classList.Where(p => p.name == className.Substring(0, genericMarkPosition)).Count() > 0)
                {
                    seletedClass = e.classList.Where(p => p.name == className.Substring(0, genericMarkPosition)).Single();
                }
                else if (e.classList.Where(p => p.name == className).Count() > 0)
                    seletedClass = e.classList.Where(p => p.name == className).Single();
                e = e.parent;
            }

            return seletedClass;
        }

        private MUTDODQLProtectionLevel GetMUTDODQLProtectionLevelEnum(string proLevel)
        {
            MUTDODQLProtectionLevel result = MUTDODQLProtectionLevel.Private;

            Array MUTDODQLProtectionLevels = Enum.GetValues(typeof(MUTDODQLProtectionLevel));
            for (int i = 0; i < MUTDODQLProtectionLevels.Length; i++)
            {
                if (MUTDODQLProtectionLevels.GetValue(i).ToString() == proLevel.ToLower())
                    result = (MUTDODQLProtectionLevel)MUTDODQLProtectionLevels.GetValue(i);
            }

            return result;
        }

        private List<SemanticExceptionItem> CheckGetListItemsInEnvironment(Class selectedClass, IQueryTree qt, Environment ev)
        {
            if (selectedClass == null)
                throw new Exception("Unexcpected operation!");

            List<SemanticExceptionItem> exceptions = new List<SemanticExceptionItem>();
            Class newClass = null;

            if(qt == null)
                exceptions.Add(new SemanticExceptionItem(qt.TokenName, "Get list can not be empty", qt.TokenLine, qt.TokenCol));
            else
            {
                IQueryTree gli = qt.ProductionsList.First();

                switch (gli.TokenName)
                {
                    case "NAME":
                        if (selectedClass.fields.Where(p => p.name == gli.TokenValue).Count() != 1)
                        {
                            exceptions.Add(new SemanticExceptionItem(gli.TokenName, string.Format("Unknown attribute name '{0}'", gli.TokenValue), gli.TokenLine, gli.TokenCol));
                        }
                        else 
                        {
                            string newCls = selectedClass.fields.Where(p => p.name == gli.TokenValue).Single().type;
                            newClass = GetClassFromEnvironment(newCls, ev);
                        }
                        break;
                    case "Object_Selection_Path":
                        exceptions.AddRange(CheckObjectSelectionPathInEnvironment(selectedClass, gli, ev, out newClass));
                        break;
                    case "Agregation_Function":
                        if (qt.ProductionsList[2].TokenName == "NAME")
                        {
                            if (selectedClass.fields.Where(p => p.name == qt.ProductionsList[2].TokenValue).Count() == 0)
                            {
                                exceptions.Add(new SemanticExceptionItem(qt.ProductionsList[2].TokenName, string.Format("Unknown attribute name '{0}'", qt.ProductionsList[2].TokenValue), qt.ProductionsList[2].TokenLine, qt.ProductionsList[2].TokenCol));
                            }
                            else
                            {
                                string newCls = selectedClass.fields.Where(p => p.name == gli.TokenValue).Single().type;
                                newClass = GetClassFromEnvironment(newCls, ev);
                            }
                        }
                        else if (qt.ProductionsList[2].TokenName == "Object_Selection_Path")
                        {
                            exceptions.AddRange(CheckObjectSelectionPathInEnvironment(selectedClass, qt.ProductionsList[2], ev, out newClass));
                        }
                        else
                            exceptions.Add(new SemanticExceptionItem(qt.ProductionsList[2].TokenName, "Unknown token", qt.ProductionsList[2].TokenLine, qt.ProductionsList[2].TokenCol));
                        break;
                    case "MUL":
                        break;
                    case "COUNT":
                        break;
                    default:
                        break;
                }

                if(qt.ProductionsList.Where(p=>p.TokenName == "Get_List_Item").Count() == 1)
                    exceptions.AddRange(CheckGetListItemsInEnvironment(newClass, qt.ProductionsList.Where(p=>p.TokenName == "Get_List_Item").Single(), ev));
            }

            return exceptions;
        }

        private List<SemanticExceptionItem> CheckObjectSelectionPathInEnvironment(Class selectedClass, IQueryTree osp, Environment ev, out Class cls)
        {
            List<SemanticExceptionItem> exceptions = new List<SemanticExceptionItem>();
            cls = selectedClass;

            if (osp != null && cls != null)
            {
                string attributeName = osp.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenValue;
                if (selectedClass.fields.Where(p => p.name == attributeName).Count() == 0)
                {
                    exceptions.Add(new SemanticExceptionItem(osp.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenName, string.Format("Unknown attribute name '{0}'", attributeName), osp.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenLine, osp.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenCol));
                }
                else
                {
                    string newClass = cls.fields.Where(p => p.name == attributeName).Single().type;
                    cls = GetClassFromEnvironment(newClass, ev);

                    if (cls == null)
                        exceptions.Add(new SemanticExceptionItem(osp.TokenName, string.Format("Unknown type {0}", newClass), osp.TokenLine, osp.TokenCol));
                    else
                    {
                        IQueryTree tail = osp.ProductionsList.Where(p => p.TokenName == "Path_Tail").Single();
                        exceptions.AddRange(CheckPathTailInEnvironment(cls, tail, ev, out cls));                        
                    }
                }                
            }

            return exceptions;
        }

        private List<SemanticExceptionItem> CheckPathTailInEnvironment(Class selectedClass, IQueryTree tail, Environment ev, out Class cls)
        {
            List<SemanticExceptionItem> exceptions = new List<SemanticExceptionItem>();
            cls = selectedClass;

            var firstElem = tail.ProductionsList.First();

            switch (firstElem.TokenName)
            {
                case "NAME":
                    if (selectedClass.fields.Where(p => p.name == tail.TokenValue).Count() != 1)
                    {
                        exceptions.Add(new SemanticExceptionItem(tail.TokenName, string.Format("Unknown attribute name '{0}'", tail.TokenValue), tail.TokenLine, tail.TokenCol));
                    }
                    else
                    {
                        cls = GetClassFromEnvironment(selectedClass.fields.Where(p => p.name == tail.TokenValue).Single().type, ev);
                    }
                    break;
                case "Executable_Method":
                    string methodName = firstElem.ProductionsList.Where(p => p.TokenName == "NAME").Single().TokenValue;
                    if (selectedClass.methods.Where(p => p.name == methodName).Count() == 0)
                    {
                        exceptions.Add(new SemanticExceptionItem(firstElem.TokenName, string.Format("Unknown method name '{0}'", methodName), firstElem.ProductionsList.First().TokenLine, firstElem.ProductionsList.First().TokenCol));
                    }
                    /*
                        UWAGA: uproszczenie!
                            brak sprawdzania argumentów dla wywoływanych metod
                            jeżeli istnieje więcej, niż jedna metoda o identycznej nazwie, to uznaję, że zwracanym typem 
                            przez jej wszystkie implmentacje jest typ, który wystąpi jako pierwszy w modelu 
                    */
                    else
                    {
                        cls = GetClassFromEnvironment(selectedClass.methods.Where(p => p.name == methodName).First().returnedType, ev);
                        if (tail.ProductionsList.Where(p => p.TokenName == "Path_Tail").Count() == 1)
                            exceptions.AddRange(CheckPathTailInEnvironment(cls, tail.ProductionsList.Where(p => p.TokenName == "Path_Tail").Single(), ev, out cls));
                    }
                    break;
                case "Object_Selection_Path":
                    exceptions.AddRange(CheckObjectSelectionPathInEnvironment(selectedClass, firstElem, ev, out cls));
                    break;
                default:
                    break;
            }

            return exceptions;
        }

        private List<SemanticExceptionItem> CheckExpressionFactorInEnvironment(Class selectedClass, IQueryTree query, Environment ev, out Class cls)
        {
            List<SemanticExceptionItem> exceptions = new List<SemanticExceptionItem>();
            cls = selectedClass;
            IQueryTree qt = query.ProductionsList.Where(p => p.TokenValue == string.Empty).Single();

            switch (qt.TokenName)
            {
                case "Expression_Atom":
                    IQueryTree atom = qt.ProductionsList.First();
                    switch (atom.TokenName)
                    {
                        case "NAME":
                            if (!CheckClassForField(cls, atom.TokenValue))
                                exceptions.Add(new SemanticExceptionItem(atom.TokenName, string.Format("Unknown field '{0}'", atom.TokenValue), atom.TokenLine, atom.TokenCol));
                            else
                            {
                                cls = GetClassFromEnvironment(selectedClass.fields.Where(p => p.name == atom.TokenValue).Single().type, ev);
                            }
                            break;
                        case "Literal":
                            IQueryTree constant = atom.ProductionsList.First();
                            switch (constant.TokenName)
                            {
                                case "STRING_VALUE":
                                    cls = GetClassFromEnvironment("String", ev);
                                    break;
                                case "INTEGER_VALUE":
                                    cls = GetClassFromEnvironment("Int", ev);
                                    break;
                                case "FLOAT_VALUE":
                                    cls = GetClassFromEnvironment("Float", ev);
                                    break;
                                case "BOOL_VALUE":
                                    cls = GetClassFromEnvironment("Bool", ev);
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "Object_Selection_Path":
                    exceptions.AddRange(CheckObjectSelectionPathInEnvironment(cls, qt, ev, out cls));
                    break;
                case "Binary_Expr":
                    foreach (IQueryTree q in qt.ProductionsList)
                    {
                        if(q.TokenName == "Expression_Factor")
                            exceptions.AddRange(CheckExpressionFactorInEnvironment(cls, q, ev, out cls));
                        else if(q.TokenName == "Right_Expr")
                            exceptions.AddRange(CheckExpressionFactorInEnvironment(cls, q.ProductionsList.Where(p => p.TokenName == "Expression_Factor").Single(), ev, out cls));
                    }
                    break;
                default:
                    break;
            }

            return exceptions;
        }

        private List<SemanticExceptionItem> CheckClauseExpressionInEnvironment(Class selectedClass, IQueryTree clause, Environment ev)
        {
            List<SemanticExceptionItem> exceptions = new List<SemanticExceptionItem>();
            Class cls = selectedClass;

            foreach (IQueryTree qt in clause.ProductionsList)
            {
                
                if (qt.TokenName == "Clause")
                {
                    foreach (IQueryTree q in qt.ProductionsList.Where(p=>p.TokenValue == string.Empty))
                    {
                        if (q.TokenName == "Expression_Factor")
                            exceptions.AddRange(CheckExpressionFactorInEnvironment(cls, q, ev, out cls));
                        else if (q.TokenName == "Collection_Source")
                        {
                            exceptions.AddRange(analize(q.ProductionsList.Where(p=>p.TokenName == "Get_Expression").Single(),ev));
                            exceptions.AddRange(CheckExpressionFactorInEnvironment(cls, q.ProductionsList.Where(p => p.TokenName == "Expression_Factor").Single(), ev, out cls));
                        }
                    }
                }
                else if(qt.TokenName == "Clause_Tail")
                    exceptions.AddRange(CheckClauseExpressionInEnvironment(cls, qt.ProductionsList.Where(p=>p.TokenName == "Clause_Expr").Single(), ev));
            }

            return exceptions;
        }

        private bool CheckClassForField(Class selectedClass, string field)
        {
            if (selectedClass.fields.Where(p => p.name == field).Count() > 0)
                return true;
            else
                return false;
        }

    }
}
