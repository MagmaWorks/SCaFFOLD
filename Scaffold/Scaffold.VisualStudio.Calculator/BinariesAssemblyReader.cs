using System.Reflection;
using System.Runtime.Loader;

namespace Scaffold.VisualStudio.Calculator;

public class BinariesAssemblyReader(string binariesFolder)
{
    private string BinariesFolder { get; } = binariesFolder;

    public Assembly GetAssembly()
    {
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
                continue;
                
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