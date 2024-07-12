using Scaffold.Core.Enums;
using Scaffold.Core.Models;

namespace Scaffold.Core.Interfaces;

public interface ICalculation
{
    /// <summary>
    /// e.g. Column 3A
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// By default, this will use CalcNameAttribute if it exists, otherwise it will use the value you specify.
    /// If you do not specify a value, the class name will be used.
    /// e.g. Punching Shear to EC2
    /// </summary>
    public string Type { get; set; }
    
    public CalcStatus Status { get; }
    
    public IEnumerable<Formula> GetFormulae();
    
    public void Update();
}