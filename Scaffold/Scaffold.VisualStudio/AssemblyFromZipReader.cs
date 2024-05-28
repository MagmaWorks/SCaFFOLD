using System.IO.Compression;
using Scaffold.Core.Images.Models;

namespace Scaffold.VisualStudio;

public class AssemblyFromZipReader : CalculationAssemblyReader, ICalculationAssemblyReader
{
    private List<AssemblyImage> _images = new();
    
    public AssemblyFromZipReader(string primaryAssemblyName) 
        : base(primaryAssemblyName) { }

    public IReadOnlyList<AssemblyImage> Images => _images;

    public CalculationPackage Get(Stream fileStream)
    {
        using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read))
        {
            foreach(var entry in zip.Entries)
            {
                var extension = Path.GetExtension(entry.FullName);

                var isImage = false;
                if (extension != ".dll")
                {
                    switch (extension)
                    {
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        case ".gif":
                            isImage = true;
                            break;
                        
                        default:
                            continue;
                    }
                }
                    

                using var stream = entry.Open();

                if (isImage)
                {
                    var memoryStream = ToMemoryStream(stream);
                    _images.Add(new AssemblyImage {Path = entry.FullName, Data = memoryStream.ToArray()});
                }
                else
                {
                    LoadIntoContext(entry.FullName, stream);
                }
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