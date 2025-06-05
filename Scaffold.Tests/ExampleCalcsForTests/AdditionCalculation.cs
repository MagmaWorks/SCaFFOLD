﻿using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;

namespace Scaffold.Tests.ExampleCalcsForTests;

[CalculationMetadata("Add values", "Core library tester")]
public class AdditionCalculation : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    [InputCalcValue] public CalcDouble LeftAssignment { get; set; }

    [InputCalcValue] public CalcDouble RightAssignment { get; set; }

    [OutputCalcValue] public CalcDouble Result { get; set; }

    public AdditionCalculation()
    {
        LeftAssignment = new CalcDouble(2, "Left assignment");
        RightAssignment = new CalcDouble(3);
        Result = new CalcDouble(Add(), "Result");
    }

    public void Calculate()
    {
        Result.Value = Add();
    }

    public IList<IFormula> GetFormulae()
    {
        var keyImage = new SKBitmap(400, 400);
        using (var canvas = new SKCanvas(keyImage))
        {
            //var paintText = new SKPaint { TextSize = 25, TextAlign = SKTextAlign.Left, Color = SKColors.Black };
            //canvas.DrawText("Drawn from SKBitmap", 25, 25, paintText);

            var font = new SKFont()
            {
                Size = 25
            };

            var paint = new SKPaint()
            {
                Color = SKColors.Black
            };

            canvas.DrawText("Drawn from SKBitmap", 25, 25, SKTextAlign.Left, font, paint);
        }

        var list = new List<IFormula>
        {
            Formula.New("Narrative to appear above the expression" +
                "Some text here" +
                "Some ref here" +
                "x &=& a + b")
                .SetImage(new ImageFromEmbeddedResource<AdditionCalculation>("ImageAsEmbeddedResource.png")),
            Formula.New("2. Narrative to appear above the expression" +
                "2. Some text here" +
                "2. Some ref here" +
                "x &=& a + b")
                .SetImage(new ImageFromSkBitmap(keyImage)),
            Formula.New("Final narrative" +
                "3.a")
                .SetImage(new ImageFromRelativePath("ImageAsRelativePath.png"))
        };

        return list;
    }

    private double Add() => LeftAssignment + RightAssignment;
}
