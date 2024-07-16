using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.Models.Main;

namespace Scaffold.VisualStudio.Models.Xaml;

/// <summary>
/// Save settings only needs to be called on one item in each radio group
/// </summary>
[DataContract]
public class DisplaySettings : NotifyPropertyChangedObject, ISettings
{
    private bool _alwaysExpandCalculations = true;
    private bool _rememberLastExpansion;
    
    private bool _dotnetBuild = true;
    private bool _dotnetBuildNoRestore;
    
    [DataMember]
    public bool AlwaysExpandCalculations
    {
        get => _alwaysExpandCalculations;
        set
        {
            SetProperty(ref _alwaysExpandCalculations, value);
            SaveSettings();
        }
    }

    [DataMember]
    public bool RememberLastExpansion
    {
        get => _rememberLastExpansion;
        set => SetProperty(ref _rememberLastExpansion, value);
    }

    [DataMember]
    public bool DotnetBuild
    {
        get => _dotnetBuild;
        set
        {
            SetProperty(ref _dotnetBuild, value);
            SaveSettings();
        }
    }

    [DataMember]
    public bool DotnetBuildNoRestore
    {
        get => _dotnetBuildNoRestore;
        set => SetProperty(ref _dotnetBuildNoRestore, value);
    }

    private void SaveSettings()
    {
        var obj = JsonSerializer.Serialize(this);
        File.WriteAllText(Globals.GetSettingsPath(), obj);
    }
}