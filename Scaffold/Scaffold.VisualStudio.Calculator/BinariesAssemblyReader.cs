using System.Reflection;
using System.Runtime.Loader;

namespace Scaffold.VisualStudio.Calculator;

public class BinariesAssemblyReader(string binariesFolder, string packageName)
{
    private string BinariesFolder { get; } = binariesFolder;
    private string PackageName { get; } = packageName;

    public Assembly GetAssembly()
    {
        if (Directory.Exists(BinariesFolder) == false)
            return null;

        var context = new AssemblyLoadContext(null, true);
        Assembly primary = null;

        foreach (var file in Directory.GetFiles(BinariesFolder))
        {
            if (Path.GetExtension(file) != ".dll")
                continue;

            using var stream = File.OpenRead(file);

            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            try
            {
                var loadedAssembly = context.LoadFromStream(memoryStream);

                if (primary == null && file.EndsWith(PackageName))
                    primary = loadedAssembly;
            }
            finally
            {
                memoryStream.Dispose();
            }
        }

        return primary;
    }
}