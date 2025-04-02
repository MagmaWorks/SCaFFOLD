using Scaffold.Core.Enums;
using Scaffold.Core.Images.Interfaces;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Models;

public class Formula : IFormula
{
    public string Markdown { get; set; } = "";
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public ICalcImage Image { get; set; }

    public Formula() { }

    public Formula(string markdown, CalcStatus status = CalcStatus.None)
    {
        Markdown = markdown;
        Status = status;
    }

    public static Formula New(string markdown)
    {
        return new Formula { Markdown = markdown };
    }

    public Formula SetImage(ICalcImage image)
    {
        Image = image;
        return this;
    }
}
