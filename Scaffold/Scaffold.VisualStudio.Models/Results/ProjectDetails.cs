namespace Scaffold.VisualStudio.Models.Results;

public class ProjectDetails
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