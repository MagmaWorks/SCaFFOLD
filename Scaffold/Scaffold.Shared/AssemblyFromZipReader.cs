using System.IO.Compression;

namespace Scaffold.Console;

public class AssemblyFromZipReader : CalculationAssemblyReader, ICalculationAssemblyReader
{
    public AssemblyFromZipReader(string primaryAssemblyName) 
        : base(primaryAssemblyName) { }

    public CalculationPackage Get(Stream fileStream)
    {
        using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read))
        {
            foreach(var entry in zip.Entries)
            {
                var extension = Path.GetExtension(entry.FullName);

                if (extension != ".dll")
                    continue;
                
                using var stream = entry.Open();
                LoadIntoContext(entry.FullName, stream);
                
            }
        }
        
        return GetPackage();
    }

    public IEnumerable<LibraryCalculation> AvailableCalculations()
    {
        var list = new List<LibraryCalculation>();
        
        foreach (var assembly in Context.Assemblies)
            list.AddRange(AvailableCalculations(assembly));
        
        return list;
    }
}