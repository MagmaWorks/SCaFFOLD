using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.VisualStudio.AddIn.Classes
{
    internal class ProjectDetails
    {
        public bool IsExecutable { get; set; }
        public string TargetFramework { get; set; }
        public string AssemblyName { get; set; }
        public string ProjectFilePath { get; set; }

        public string AssemblyPath()
        {
            var fileType = IsExecutable ? ".exe" : ".dll";
            return $@"{ProjectFilePath}\bin\Debug\{TargetFramework}\{AssemblyName}{fileType}";
        }
    }
}
