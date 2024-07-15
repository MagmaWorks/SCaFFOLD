using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;

namespace Scaffold.XUnitTests.Core;

public class AdditionCalculationFluent : ICalculation, ICalculationConfiguration<AdditionCalculationFluent>
{
    public AdditionCalculationFluent()
    {
        LeftAssignment = new CalcDouble("Left assignment", 2);
        RightAssignment = new CalcDouble(3);
        Result = new CalcDouble(Add());
    }
    
    public CalcDouble LeftAssignment { get; set; }
    public CalcDouble RightAssignment { get; set; }
    
    public CalcDouble Result { get; set; }

    private double Add()
        => LeftAssignment.Value + RightAssignment.Value;

    public void Update()
    {
        Result.Value = Add();
    }
    
    public IEnumerable<Formula> GetFormulae()
    {
        var keyImage = new SKBitmap(400,400);
        using (var canvas = new SKCanvas(keyImage))
        {
            var paintText = new SKPaint { TextSize = 25, TextAlign = SKTextAlign.Left, Color = SKColors.Black};
            canvas.DrawText("Drawn from SKBitmap", 25, 25, paintText);
        }
        
        var list = new List<Formula>
        {
            Formula.New("Narrative to appear above the expression")
                .WithConclusion("Some text here")
                .WithReference("Some ref here")
                .AddExpression("x &=& a + b")
                .AddImage(new ImageFromEmbeddedResource<AdditionCalculation>("ImageAsEmbeddedResource.png")),
            
            
            Formula.New("2. Narrative to appear above the expression")
                .WithConclusion("2. Some text here")
                .WithReference("2. Some ref here")
                .AddExpression("x &=& a + b")
                .AddImage(new ImageFromSkBitmap(keyImage)),
            
            Formula.New("Final narrative")
                .WithReference("3.a")
                .AddImage(new ImageFromRelativePath("ImageAsRelativePath.png"))
        };

        return list;
    }

    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    
    public void Configure(CalculationConfigurationBuilder<AdditionCalculationFluent> configurationBuilder)
    {
        configurationBuilder
            .Define(x => new { x.LeftAssignment, x.RightAssignment })
            .AsInput();
        
        configurationBuilder.Define(x => x.Result)
            .WithDisplayName("Result")
            .AsOutput();
    }
}