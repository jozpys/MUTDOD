using System;
using System.Runtime.InteropServices;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace MUTDOD.Server.CentralServer.CentralServerBase
{
    public class CSRunnableProgram
    {
        private static IWindsorContainer _container = new WindsorContainer();
        public static void Init()
        {
            _container.Install(FromAssembly.This());
        }

        public static void Register<T, K>()
            where K : T
            where T: class
        {
            _container.Register(Component.For<T>().ImplementedBy<K>());
        }

        public static void Register<T, K>(string dependantName,object dependencyValue)
            where K : T
            where T : class
        {
            _container.Register(
                Component.For<T>().ImplementedBy<K>().DependsOn(Dependency.OnValue(dependantName, dependencyValue)));
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static void SetConsoleWindowPosition(int x, int y)
        {
           SetWindowPos(GetConsoleWindow(), 0, x, y, 0, 0, 0x0001);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr wnd, int wndInsertAfter, int x, int y, int cx, int cy, int flags);
        
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public static class Constant
        {
            public const string MutdodCentralServer = "MUTDOD Central Server\n\n"; 
            public const string MutdodCentralServerConsoleTitleFormat = "MUTDOD Central Server: {0}";
        }
    }
}
