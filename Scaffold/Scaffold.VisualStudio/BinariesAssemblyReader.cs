using System.IO.Packaging;
using System.Reflection;
using System.Runtime.Loader;

namespace Scaffold.VisualStudio
{
    public class BinariesAssemblyReader(string projectPath)
    {
        private string ProjectPath { get; } = projectPath;

        public Assembly GetAssembly()
        {
            // TODO: Folder selection - we can't guarantee the .NET version.
            var binariesFolder = $@"{ProjectPath}\bin\Debug\net8.0";
            if (Directory.Exists(binariesFolder) == false)
                return null;

            var context = new AssemblyLoadContext(null, true);
            Assembly primary = null;

            foreach (var file in Directory.GetFiles(binariesFolder))
            {
                if (Path.GetExtension(file) != ".dll")
                    continue;

                //if (file.Contains("Scaffold.Core.dll"))
                //    continue;

                using var stream = File.OpenRead(file);

                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                try
                {
                    var loadedAssembly = context.LoadFromStream(memoryStream);

                    if (primary == null
                        && file.Contains("VsTesting.dll"))
                    {
                        primary = loadedAssembly;
                    }

                    memoryStream.Dispose();
                    stream.Dispose();
                }
                catch (Exception ex)
                {
                    memoryStream.Dispose();
                    stream.Dispose();

                }

            }

            return primary;
        }
    }
}
