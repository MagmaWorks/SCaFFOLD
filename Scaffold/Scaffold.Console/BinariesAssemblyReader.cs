using System.Reflection;
using System.Runtime.Loader;

namespace Scaffold.Console
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
                var fullFilePath = file;

                if (Path.GetExtension(file) != ".dll")
                    continue;

                if (fullFilePath.Contains("Scaffold.Core.dll"))
                {
                    continue;
                    //fullFilePath = @"C:\Users\d.growns\Documents\Repos\Web\Scaffold.App\Scaffold.App\LocalDependencies\Scaffold.Core.dll";
                    //fullFilePath = @"C:\Users\d.growns\Documents\Repos\WPF\SCaFFOLD\Scaffold\Scaffold.VisualStudio\bin\Debug\net8.0-windows\Scaffold.Core.dll";
                }

                using var stream = File.OpenRead(fullFilePath);

                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                try
                {
                    var loadedAssembly = context.LoadFromStream(memoryStream);

                    if (primary == null
                        && fullFilePath.Contains("VsTesting.dll"))
                    {
                        primary = loadedAssembly;
                    }
                }
                catch (Exception ex)
                {
                    ;
                    throw;
                }
                finally
                {
                    memoryStream.Dispose();
                    stream.Dispose();
                }

            }

            return primary;
        }
    }
}
