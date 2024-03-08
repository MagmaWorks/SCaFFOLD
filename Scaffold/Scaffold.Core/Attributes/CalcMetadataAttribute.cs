namespace Scaffold.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CalcMetadataAttribute : Attribute
{
    public string TypeName { get; }
    public string Title { get; set; }
    
    public CalcMetadataAttribute(string name) => TypeName = name;
    
    public CalcMetadataAttribute(string name, string title)
    {
        TypeName = name;
        Title = title;
    }
}