using System;
using MUTDOD.Server.CentralServer.CentralServerBase;

namespace MUTDOD.Server.CentralServer.CSRunnable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CSRunnableProgram.SetConsoleWindowPosition(0, 50);
            Console.Title = CSRunnableProgram.Main(args, Console.WriteLine);
            Console.ReadKey();
        }
    }
}
