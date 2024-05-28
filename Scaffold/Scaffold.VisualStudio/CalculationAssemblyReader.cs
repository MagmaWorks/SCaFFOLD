using System.Reflection;
using System.Runtime.Loader;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;

namespace Scaffold.VisualStudio;
//
// Below is the reason why this class ignores the core library, allowing us to use design time typecasting of calcs.
//
// https://github.com/dotnet/samples/blob/main/core/tutorials/Unloading/Host/Program.cs
// The Load method override causes all the dependencies present in the plugin's binary directory to get loaded
// into the HostAssemblyLoadContext together with the plugin assembly itself.
// NOTE: The Interface assembly must not be present in the plugin's binary directory, otherwise we would
// end up with the assembly being loaded twice. Once in the default context and once in the HostAssemblyLoadContext.
// The types present on the host and plugin side would then not match even though they would have the same names.
//
public abstract class CalculationAssemblyReader
{
    private string PrimaryAssemblyName { get; }
    private CalculationPackage Package { get; }
    protected AssemblyLoadContext Context { get; }

    protected CalculationAssemblyReader(string primaryAssemblyName)
    {
        PrimaryAssemblyName = primaryAssemblyName;
        Context = new AssemblyLoadContext(null, true);
        Package = new CalculationPackage();
    }

    protected static MemoryStream ToMemoryStream(Stream stream)
    {
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }

    protected void LoadIntoContext(string fullFileName, Stream stream)
    {
        if (fullFileName.Contains("Scaffold.Core.dll") || fullFileName.Contains("SkiaSharp.dll"))
        {
            stream.Dispose();
            return;
        }

        var memoryStream = ToMemoryStream(stream);

        try
        {
            var loadedAssembly = Context.LoadFromStream(memoryStream);
            Package.AddAssembly(loadedAssembly);

            if (Package.Assembly == null
                && string.IsNullOrEmpty(PrimaryAssemblyName) == false
                && fullFileName.Contains(PrimaryAssemblyName))
            {
                Package.SetPrimaryAssembly(loadedAssembly);
            }
        }
        catch (Exception ex)
        {
            Package.Errors.Add($"{ex.Message} - {fullFileName}");
        }
        finally
        {
            memoryStream.Dispose();
            stream.Dispose();
        }
    }

    protected IEnumerable<LibraryCalculation> AvailableCalculations(Assembly assembly)
    {
        var list = new List<LibraryCalculation>();
        var types = assembly.GetTypes().ToList();

        foreach (var type in types)
        {
            var isCalc = type.GetInterfaces().Any(x => x.FullName == typeof(ICalculation).FullName);
            if (isCalc == false)
                continue;

            var metadata = type.GetMetadata();

            list.Add(
                new LibraryCalculation
                {
                    AssemblyName = assembly.ManifestModule.ScopeName,
                    QualifiedTypeName = type.FullName,
                    Type = metadata.Type,
                    Title = metadata.Title
                });
        }

        return list;
    }

    protected CalculationPackage GetPackage()
    {
        if (string.IsNullOrEmpty(PrimaryAssemblyName) == false && Package.Assembly == null)
            throw new ArgumentException("The primary calculation assembly was not found.");

        if (Package.Assembly != null)
            Package.CalculationQualifiedNames
                .AddRange(AvailableCalculations(Package.Assembly));

        return Package;
    }
}