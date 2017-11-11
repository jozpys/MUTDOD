using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace OdraIDE.Core
{
    public static class AppEnvironment
    {
        public static void RestartApplication()
        {
            Process.Start(Assembly.GetEntryAssembly().Location);
            Application.Current.Shutdown();
        }

        public static string UserFileDirectory()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                + "\\" + System.Windows.Forms.Application.CompanyName + "\\" + System.Windows.Forms.Application.ProductName;

            // If the directory doesn't exist, create it.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string CommonFileDirectory()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                + "\\" + System.Windows.Forms.Application.CompanyName + "\\" + System.Windows.Forms.Application.ProductName;

            // If the directory doesn't exist, create it.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

    }
}
