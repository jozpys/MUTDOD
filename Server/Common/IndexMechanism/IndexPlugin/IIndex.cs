using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Communication;

namespace IndexPlugin
{
    public delegate void settingsChangedHandler(IIndex<object> source, string newSettingsXML);

    public interface IIndex<T> : IDisposable
    {
        string Name { get; }
        IndexData EmptyIndexData { get; }

        Type[] AvailableIndexingTypes { get; }

        //delegat wywolywany jeżeli zarządca indeksów ma zapisać ustawienia tego indeksu
        settingsChangedHandler SettingsChanged { get; set; }
        //przekazani zapisanych ustawień z zarzadcy indeksami
        void SetSettings(string xml);

        //informacja czy indeks jest poprawny
        bool isValid(IndexData indexData);
        //przebudowanie indeksu z uwzględnieniem tylko obiektów z argumentu - wymagane gdy indeks jest nie poprawny
        IndexData rebuildIndex(IndexData indexData, Oid[] objs);
        //przebudowanie indeksu z uwzględnieniem tylko ról dla obiektów z argumentu - wymagane gdy indeks jest nie poprawny
        IndexData rebuildIndex(IndexData indexData, Dictionary<Oid, DynamicRole[]> objs);
        //przebudowa struktury - indeks jest prawidłowy, przebudowa ma poprawić wydajność
        IndexData rebuildIndex(IndexData indexData);

        //zindeksowanie podanych atrybutów w obiekcie
        IndexData AddObject(IndexData indexData, Oid obj, String[] attributes, QueryParameters parameters);
        //zindeksowanie wszystkich atrybutów w obiekcie
        IndexData AddObject(IndexData indexData, Oid obj, QueryParameters parameters);

        //zindeksowanie podanych atrybutów dynamicznej roli związanej z podanym obiektem
        IndexData AddDynamicRole(IndexData indexData, Oid obj, DynamicRole role, String[] attributes);
        //zindeksowanie wszystkich atrybutów dynamicznej roli związanej z podanym obiektem
        IndexData AddDynamicRole(IndexData indexData, Oid obj, DynamicRole role);

        //usunięcie podanych zindeksowanych atrybutów obiektu
        IndexData RemoveObject(IndexData indexData, Oid obj, String[] attributes);
        //usuniecie wszystkich zindeksowanych atrybutów obiektu
        IndexData RemoveObject(IndexData indexData, Oid obj);

        //usunięcie podanych zindeksowanych atrybutów dynamicznej roli związanej z podanym obiektem
        IndexData RemoveDynamicRole(IndexData indexData, Oid obj, DynamicRole role, String[] attributes);
        //usuniecie wszystkich zindeksowanych atrybutów dynamicznej roli związanej z podanym obiektem
        IndexData RemoveDynamicRole(IndexData indexData, Oid obj, DynamicRole role);

        //wyczyszczenie indeksu
        IndexData RemoveObjects(IndexData indexData);

        //zwrócenie zbioru OID zindeksowanych obiektów o licznosci równemu packageSize
        Guid[] GetIndexedObjects(IndexData indexData, int? packageSize, int skipItemsCount);
        //zwrócenie zbioru OID zindeksowanych dynamicznych ról o licznosci równemu packageSize
        Guid[] GetIndexedDynamicRoles(IndexData indexData, int? packageSize, int skipItemsCount);

        //zwrócenie OID obiektów które są danej klasy - complexExtension na true oznacza także obiekty które dziedziczą ze wskazanego typu
        Guid[] FindObjects(IndexData indexData, T OIDClass, bool complexExtension, out int? readedObjects);
        //zwrócenie OID obiektów które są danej klasy oraz mają wskazane atrybuty zgodne z porównywanymi wartosciami - complexExtension na true oznacza także obiekty które dziedziczą ze wskazanego typu
        Guid[] FindObjects(IndexData indexData, T OIDClass, bool complexExtension, String[] attributes,
                          object[] values, CompareType[] compareTypes, out int? readedObjects);

        //zwrócenie OID obiektów które posiadają podaną dynamiczną rolę
        Guid[] FindObjects(IndexData indexData, DynamicRole dynamicRole, out int? readedObjects);
        //zwrócenie OID obiektów które posiadają podaną dynamiczną rolę oraz mają wskazane atrybuty zgodne z porównywanymi wartosciami
        Guid[] FindObjects(IndexData indexData, DynamicRole dynamicRole, String[] attributes, object[] values,
                          CompareType[] compareTypes, out int? readedObjects);


        //zwrócenie kosztu dodania jednego obiektu do indeksu przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost ObjectIndexingCost(int indexedObjects);
        //zwrócenie kosztu aktualizacji zindeksowanych atrybutów dla jednego obiekt przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost ObjectIndexRefreshCost(int indexedObjects);
        //zwrócenie kosztu usunięcia zindeksowanych atrybutów dla jednego obiekt przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost ObjectIndexRemoveCost(int indexedObjects);
        //zwrócenie kosztu znalezienia zindeksowanych obiektów spełniających zadane warunki przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost ObjectFindCost(int indexedObjects);

        //zwrócenie kosztu dodania jednej dynamicznej roli do indeksu przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost RoleIndexingCost(int indexedObjects);
        //zwrócenie kosztu aktualizacji zindeksowanych atrybutów dla jednej dynamicznej roli przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost RoleIndexRefreshCost(int indexedObjects);
        //zwrócenie kosztu usunięcia zindeksowanych atrybutów dla jednej dynamicznej roli przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost RoleIndexRemoveCost(int indexedObjects);
        //zwrócenie kosztu znalezienia zindeksowanych obiektów spełniających zadane warunki przy posiadaniu już podanej liczby zindeksowany obiektów
        IndexOperationCost RoleFindCost(int indexedObjects);
        //zwraca typy zindeksowanych obiektów
        string[] GetTypesNameIndexedObjects(IndexData indexData);
        //zwraca atrybuty po których indeksowane są obiekty danego typu
        List<string> GetIndexedAttribiutesForType(T t);
    }

    public enum CompareType
    {
        equal,
        notEqual,
        greater,
        greaterOrEqual,
        less,
        lessOrEqual,
        like,
        notLike
    }

    public class WrongTypeToIndexException : Exception
    {
        private Type _wrongTypeToIndex;
        private string _msg;

        public WrongTypeToIndexException(Type wrongTypeToIndex, string msg)
            : base()
        {
            _wrongTypeToIndex = wrongTypeToIndex;
            _msg = msg;
        }

        public override string Message
        {
            get
            {
                return string.Format("Index throwed '{0}' exception for object type {1}", _msg,
                                     _wrongTypeToIndex.FullName);
            }
        }
    }

    public class WrongCompareTypeException : Exception
    {
        private CompareType _wrongCompareType;
        private string _msg;

        public WrongCompareTypeException(CompareType compareType, string msg) : base()
        {
            _wrongCompareType = compareType;
            _msg = msg;
        }

        public override string Message
        {
            get { return string.Format("{0} for CompareType {1}", _msg, _wrongCompareType.ToString()); }
        }
    }

    public class WrongIndexDataException : Exception
    {
        private Type _wrongIndexDataType;
        private string _msg;
        private Type _expectedType;

        public WrongIndexDataException(Type expectedType, Type wrongIndexDataType, string msg)
            : base()
        {
            _wrongIndexDataType = wrongIndexDataType;
            _msg = msg;
            _expectedType = expectedType;
        }

        public override string Message
        {
            get
            {
                return string.Format("{0}. Expected {1} but got {2}", _msg, _expectedType.FullName,
                                     _wrongIndexDataType.FullName);
            }
        }
    }

    public class WrongAttributeException : Exception
    {
        private string _msg;
        private Type _objcetType;
        private String _wrongAttribute;

        public WrongAttributeException(Type objcetType, String wrongAttribute, string msg)
            : base()
        {
            _wrongAttribute = wrongAttribute;
            _msg = msg;
            _objcetType = objcetType;
        }

        public override string Message
        {
            get
            {
                return string.Format("{0}. Unable to find {1} attribiute in {2} class", _msg, _wrongAttribute,
                                     _objcetType.FullName);
            }
        }
    }

    public class WrongSettingsException : Exception
    {
        public WrongSettingsException(string msg)
            : base(msg)
        {
        }
    }
}