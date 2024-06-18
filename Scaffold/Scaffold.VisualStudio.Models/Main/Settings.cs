using System.Runtime.Serialization;
using System.Text.Json;

namespace Scaffold.VisualStudio.Models.Main;

public class Settings : ISettings
{
    public bool AlwaysExpandCalculations { get; set; }
    public bool RememberLastExpansion { get; set; }
    public bool DotnetBuild { get; set; }
    public bool DotnetBuildNoRestore { get; set; }
}