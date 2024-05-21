using Scaffold.Core.Enums;
using SkiaSharp;

namespace Scaffold.Core.Models;

public class Formula
{
    private Formula() { }

    public Formula(string reference, string narrative, string conclusion, string expression,
        CalcStatus status = CalcStatus.None)
    {
        Ref = reference;
        Narrative = narrative;
        Conclusion = conclusion;
        Expression.Add(expression);
        Status = status;
    }
    
    public List<string> Expression { get; set; } = [];
    public string Ref { get; set; } = "";
    public string Narrative { get; set; } = "";
    public string Conclusion { get; set; } = "";
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public SKBitmap Image { get; set; } = null;

    
    public static Formula New(string narrative)
    {
        return new Formula { Narrative = narrative };
    }

    public Formula WithConclusion(string conclusion)
    {
        Conclusion = conclusion;
        return this;
    }

    public Formula WithStatus(CalcStatus status)
    {
        Status = status;
        return this;
    }

    public Formula WithReference(string reference)
    {
        Ref = reference;
        return this;
    }

    public Formula AddExpression(string expression)
    {
        Expression.Add(expression);
        return this;
    }

    public Formula AddExpressions(IEnumerable<string> expressions)
    {
        Expression.AddRange(expressions);
        return this;
    }

    public Formula AddImage(SKBitmap image)
    {
        Image = image;
        return this;
    }
}