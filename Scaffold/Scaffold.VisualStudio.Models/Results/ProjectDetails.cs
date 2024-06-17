namespace Scaffold.VisualStudio.Models.Results;

public class ProjectDetails
{
    public bool IsExecutable { get; set; }
    public string TargetFramework { get; set; }
    public string AssemblyName { get; set; }
    public string ProjectFilePath { get; set; }
    public string CsProjFile { get; set; }

    public string BinariesPath()
        => $@"{ProjectFilePath}\bin\Debug\{TargetFramework}";
    
    public string CsProjPath()
        => $@"{ProjectFilePath}\{CsProjFile}";
    
    public string AssemblyPath()
        => $@"{BinariesPath()}\{PackageName()}";
    
    public string PackageName()
    {
        var fileType = IsExecutable ? ".exe" : ".dll";
        return $"{AssemblyName}{fileType}";
    }
}