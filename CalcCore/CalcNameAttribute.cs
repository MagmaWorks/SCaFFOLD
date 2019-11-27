using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    [System.AttributeUsage(AttributeTargets.Class)]
    public class CalcNameAttribute : Attribute
    {
        public string CalcName { get; private set; }

        public CalcNameAttribute(string name)
        {
            CalcName = name;
        }
    }

    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CalcAlternativeNameAttribute : Attribute
    {
        public string CalcAlternativeName { get; private set; }

        public CalcAlternativeNameAttribute(string name)
        {
            CalcAlternativeName = name;
        }
    }
}
