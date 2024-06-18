namespace Scaffold.VisualStudio.Models.Main;

public interface ISettings
{
    public bool AlwaysExpandCalculations { get; set; }
    public bool RememberLastExpansion { get; set; }
    public bool DotnetBuild { get; set; }
    public bool DotnetBuildNoRestore { get; set; }
}