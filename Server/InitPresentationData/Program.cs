using System.Collections.Concurrent;
using System.Collections.Generic;
using MutDood.Storage.Core.MetadataElements;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.Storage;
using MUTDOD.Server.Common.Storage.MetadataElements;

namespace InitPresentationData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = new HardcodedSettings();
            var s = new Storage(settings);
            foreach (var d in s.GetDatabases())
            {
                s.RemoveDatabase(new DatabaseRemoveParameters {DatabaseToRemove = d.DatabaseId});
            }
            var dbClasses = new ConcurrentDictionary<ClassId, Class>();
            long id = 0;
            var classId = new ClassId {Name = "IOsoba", Id = ++id};
            dbClasses.TryAdd(classId,
                new Class
                {
                    ClassId = classId,
                    Name = "IOsoba",
                    Fields = new List<string>(new[] {"Imie", "Nazwisko", "PESEL"}),
                    Methods = new List<string>()
                });
            classId = new ClassId {Name = "Student", Id = ++id};
            dbClasses.TryAdd(classId,
                new Class
                {
                    ClassId = classId,
                    Name = "Student",
                    Fields = new List<string>(new[] {"Imie", "Nazwisko", "PESEL", "NrIndeksu"}),
                    Methods = new List<string>()
                });
            classId = new ClassId {Name = "Wykładowca", Id = ++id};
            dbClasses.TryAdd(classId,
                new Class
                {
                    ClassId = classId,
                    Name = "Wykładowca",
                    Fields = new List<string>(new[] {"Imie", "Nazwisko", "PESEL", "StopienNaukowy"}),
                    Methods = new List<string>(new[] {"PrzedstawSie"})
                });
            classId = new ClassId {Name = "Przedmiot", Id = ++id};
            dbClasses.TryAdd(classId,
                new Class
                {
                    ClassId = classId,
                    Name = "Przedmiot",
                    Fields = new List<string>(new[] {"Wykładowca", "Nazwa"}),
                    Methods = new List<string>()
                });
            classId = new ClassId {Name = "Wykład", Id = ++id};
            dbClasses.TryAdd(classId,
                new Class
                {
                    ClassId = classId,
                    Name = "Wykład",
                    Fields = new List<string>(new[] {"Przedmiot", "Studenci", "Sala"}),
                    Methods = new List<string>()
                });
            classId = new ClassId {Name = "Dziekan", Id = ++id};
            dbClasses.TryAdd(classId,
                new Class
                {
                    ClassId = classId,
                    Name = "Dziekan",
                    Fields = new List<string>(new[] {"Imie", "Nazwisko", "PESEL", "StopienNaukowy", "Wydział"}),
                    Methods = new List<string>(new[] {"PrzedstawSie"})
                });
            s.CreateDatabase(new DatabaseParameters("PresentationDB", settings)
            {
                Schema = new DatabaseSchema()
                {
                    Classes = dbClasses
                }
            });
        }
    }
}
