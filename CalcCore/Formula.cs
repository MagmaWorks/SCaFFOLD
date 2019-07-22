using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Media.Imaging;
using System.Media;
using SkiaSharp;

namespace CalcCore
{
    public class Formula
    {
        public List<string> Expression { get; set; } = new List<string>() { "" };
        public string Ref { get; set; } = "";
        public string Narrative { get; set; } = "";
        public string Conclusion { get; set; } = "";
        public CalcStatus Status { get; set; } = CalcStatus.NONE;
        public SkiaSharp.SKBitmap Image { get; set; } = null;

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
