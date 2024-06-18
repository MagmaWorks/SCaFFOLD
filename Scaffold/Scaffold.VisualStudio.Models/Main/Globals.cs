using System.Reflection;
using System.Text.Json;

namespace Scaffold.VisualStudio.Models.Main;

public static class Globals
{
    public static string SettingsFileName => "settings.json";
    
    /// <summary>
    /// Executing assembly from VS appears to read \ and . as final characters even after last index of.
    /// This adds additional sanitization to reduce the string further to only textual characters without another directory break.
    /// </summary>
    private static string SanitizeBasePath(string path)
    {
        while (true)
        {
            if (path.Last() == '.' || path.Last() == '\\')
            {
                path = path[..^1];
                continue;
            }

            return path;
        }
    }

    public static string GetWorkingDirectory()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var lastIndex = executingAssembly.Location.LastIndexOf(@"\", StringComparison.Ordinal);
        var workingDirectory = executingAssembly.Location[..lastIndex];

        return SanitizeBasePath(workingDirectory);
    }
    
    public static string GetSettingsPath()
    {
        var workingDirectory = GetWorkingDirectory();
        return $@"{workingDirectory}\{SettingsFileName}";
    }

    public static T GetSettings<T>() where T : ISettings, new()
    {
        var settingsPath = GetSettingsPath();
        var settings = default(T);

        if (File.Exists(settingsPath))
        {
            var jsonStr = File.ReadAllText(settingsPath);
            settings = JsonSerializer.Deserialize<T>(jsonStr);
        }
        
        settings ??= new T();
        return settings;
    }
}