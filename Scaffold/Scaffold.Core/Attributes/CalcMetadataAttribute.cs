namespace Scaffold.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CalcMetadataAttribute : Attribute
{
    public string TypeName { get; set; }
    public string Title { get; set; }
    
    /// <summary>
    /// Define type and title using property names.
    /// </summary>
    public CalcMetadataAttribute(){}
    
    /// <summary>
    /// Define just the type name of this calculation, independent of the class name.
    /// e.g. Punching Shear to EC2
    /// </summary>
    public CalcMetadataAttribute(string name) => TypeName = name;
    
    /// <summary>
    /// Define just the type name of this calculation, independent of the class name.
    /// e.g. Column 3A
    /// </summary>
    public CalcMetadataAttribute(string name, string title)
    {
        TypeName = name;
        Title = title;
    }
}