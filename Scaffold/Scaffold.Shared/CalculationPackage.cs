using System.Reflection;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;

namespace Scaffold.Shared;

public class CalculationPackage
{
    private List<Assembly> Assemblies { get; } = new();

    public bool IsLoaded => Assembly != null;
    public bool HasErrors => Errors.Count > 0;

    public Assembly Assembly { get; private set; }
    public List<LibraryCalculation> CalculationQualifiedNames { get; } = new();
    public List<string> Errors { get; } = new();

    public void SetPrimaryAssembly(Assembly assembly)
        => Assembly = assembly;

    public void AddAssembly(Assembly assembly)
        => Assemblies.Add(assembly);

    public List<Assembly> FindCalculationAssemblies()
    {
        var list = new List<Assembly>();
        foreach (var item in Assemblies)
        {
            if (item.GetTypes().Any(x => x.GetInterfaces().Contains(typeof(ICalculation))))
                list.Add(item);
        }

        return list;
    }

    public CalculationBase GetCalculation(string qualifiedName)
    {
        var hasCalculation = CalculationQualifiedNames.Any(x => x.QualifiedTypeName == qualifiedName);
        if (hasCalculation == false)
            return null;

        CalculationBase instance = null;
        try
        {
            instance = (CalculationBase)Assembly.CreateInstance(qualifiedName);
            instance?.LoadIoCollections();
        }
        catch (Exception ex)
        {
            throw;
        }

        return instance;
    }
}