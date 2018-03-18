using MUTDOD.Common;
using OdraIDE.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdraIDE.SolutionExplorer.Connections.CompositionPoints;
using OdraIDE.Core.Services;

namespace OdraIDE.SolutionExplorer.Connections.Commands
{
    [Export(Workbench.TreeLoader, typeof(TreeLoader))]
    public class TreeLoader
    {
        [Import(Connection.ConnectionService, typeof(IConnectionService))]
        private IConnectionService connectionService { get; set; }

        [Import("CentralServerNode")]
        private ExportFactory<CentralServerNode> csnFactory { get; set; }

        [Import("DataServerNode")]
        private ExportFactory<DataServerNode> dsnFactory { get; set; }

        [Import("DatabasesFolderNode")]
        private ExportFactory<DatabasesFolderNode> dfnFactory { get; set; }

        [Import("DatabaseNode")]
        private ExportFactory<DatabaseNode> dfFactory { get; set; }

        public CentralServerNode load(SystemInfo systemInfo)
        {
            CentralServerNode centralServerNode = csnFactory.CreateExport().Value;
            centralServerNode.Properties = CentralServerProperties.From(systemInfo.CentralServer);

            DatabasesFolderNode databasesFolderNode = dfnFactory.CreateExport().Value;

            connectionService.DatabasesChanged += delegate (object s, EventArgs e)
            {
                databasesFolderNode.Children.Clear();
                foreach (string database in connectionService.Databases)
                {
                    DatabaseNode databaseNode = dfFactory.CreateExport().Value;
                    databaseNode.DatabaseName = database;
                    databasesFolderNode.Children.Add(databaseNode);
                }
            };

            foreach (var database in systemInfo.Databases.OrderBy(db => db.Name))
            {
                DatabaseNode databaseNode = dfFactory.CreateExport().Value;
                databaseNode.DatabaseName = database.Name;
                if (database.Classes != null)
                    foreach (var @class in database.Classes.OrderBy(c => c.Name))
                    {
                        var cn = new ClassNode(@class.Name);
                        foreach (var f in @class.Fields.OrderBy(f => f))
                        {
                            var fn = new FieldNode(f);
                            cn.Children.Add(fn);
                        }
                        foreach (var m in @class.Methods.OrderBy(m => m))
                        {
                            var mn = new MethodNode(m);
                            cn.Children.Add(mn);
                        }
                        databaseNode.Children.Add(cn);
                    }
                databasesFolderNode.Children.Add(databaseNode);
            }

            centralServerNode.Children.Add(databasesFolderNode);

            DataServersFolderNode dataServersFolderNode = new DataServersFolderNode();
            foreach (var dataServer in systemInfo.DataServer)
            {
                DataServerNode dataServerNode = dsnFactory.CreateExport().Value;
                dataServerNode.Properties = DataServerProperties.From(dataServer);


                //DatabasesFolderNode databasesFolderNode2 = dfnFactory.CreateExport().Value;

                //connectionService.DatabasesChanged += delegate(object s, EventArgs e)
                //{
                //    databasesFolderNode2.Children.Clear();
                //    foreach (string database in connectionService.Databases)
                //    {
                //        DatabaseNode databaseNode = new DatabaseNode(database);
                //        databasesFolderNode2.Children.Add(databaseNode);
                //    }
                //};

                //foreach (string database in databasesList)
                //{
                //    DatabaseNode databaseNode = new DatabaseNode(database);
                //    databasesFolderNode2.Children.Add(databaseNode);
                //}
                //dataServerNode.Children.Add(databasesFolderNode2);

                dataServersFolderNode.Children.Add(dataServerNode);
            }
            centralServerNode.Children.Add(dataServersFolderNode);

            return centralServerNode;
        }
    }
}
