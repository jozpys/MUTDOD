using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace OdraIDE.Core
{
    public sealed class DllDirectoryCatalog : ComposablePartCatalog 
    {
        private DirectoryCatalog directoryCatalog;
        private string binPath;
        private string path; 
        private string extension = ".dll";
        private IList<string> dllFiles;

        public DllDirectoryCatalog(string path, IList<string> dllFiles) 
        { 
            Initialize(path, dllFiles); 
        }

        private void Initialize(string path, IList<string> dllFiles) 
        { 
            this.path = path;
            this.dllFiles = dllFiles;
            this.binPath = Path.Combine(path, "bin");
            if (Directory.Exists(binPath))
            {
                // Delete all files in bin folder
                foreach (string file in Directory.GetFiles(binPath))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(binPath);
            }
            Refresh();            
        }

        private void Refresh()
        {
            // Copy files to /bin 
            foreach (string file in dllFiles)
            {
                string filePath = Path.Combine(path, file + extension);
                try
                {
                    File.Copy(filePath, Path.Combine(binPath, Path.GetFileName(file + extension)), true);
                }
                catch (Exception ex)
                {
                    // Not that big deal... 
                }
            }
            // Create new directory catalog 
            directoryCatalog = new DirectoryCatalog(binPath, "*.dll");
        }

        public override IQueryable<ComposablePartDefinition> Parts 
        { 
            get { return directoryCatalog.Parts; } 
        }

    }
}
