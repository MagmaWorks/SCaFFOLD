using Scaffold.Core.Enums;

namespace Scaffold.Core.Models
{
    public class Formula
    {
        public List<string> Expression { get; set; } = new List<string>() { "" };
        public string Ref { get; set; } = "";
        public string Narrative { get; set; } = "";
        public string Conclusion { get; set; } = "";
        public CalcStatus Status { get; set; } = CalcStatus.NONE;
        public SKBitmap Image { get; set; } = null;

        public Formula()
        {
        }

        public static Formula FormulaWithNarrative(string narrative)
        {
            return new Formula() { Narrative = narrative };
        }

        public Formula AddConclusion(string conc)
        {
            Conclusion = conc;
            return this;
        }

        public Formula AddStatus(CalcStatus status)
        {
            Status = status;
            return this;
        }

        public Formula AddRef(string reference)
        {
            Ref = reference;
            return this;
        }

        public Formula AddFirstExpression(string expression)
        {
            Expression = new List<string> { expression };
            return this;
        }

        public Formula AddExpression(string expression)
        {
            Expression.Add(expression);
            return this;
        }

        public Formula AddImage(SKBitmap image)
        {
            Image = image;
            return this;
        }
    }
}