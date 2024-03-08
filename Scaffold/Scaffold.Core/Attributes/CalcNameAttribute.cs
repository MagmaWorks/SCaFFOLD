namespace Scaffold.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CalcNameAttribute : Attribute
    {
        public string CalcName { get; }
        public CalcNameAttribute(string name) => CalcName = name;
    }
}