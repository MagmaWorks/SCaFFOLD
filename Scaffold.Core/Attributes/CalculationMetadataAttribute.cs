namespace Scaffold.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CalculationMetadataAttribute : Attribute
{
    public string TypeName { get; set; }
    public string Title { get; set; }
    
    /// <summary>
    /// Define type and title using property names.
    /// </summary>
    public CalculationMetadataAttribute(){}
    
    /// <summary>
    /// Define just the type name of this calculation, independent of the class name.
    /// e.g. Punching Shear to EC2
    /// </summary>
    public CalculationMetadataAttribute(string typeName) => TypeName = typeName;
    
    /// <summary>
    /// Define just the type name of this calculation, independent of the class name.
    /// e.g. Column 3A
    /// </summary>
    public CalculationMetadataAttribute(string typeName, string title)
    {
        TypeName = typeName;
        Title = title;
    }
}