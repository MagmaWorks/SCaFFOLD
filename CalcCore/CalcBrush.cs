using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcBrush
    {
        public ShadingType Shading { get; }
        public CalcColor Color { get; }

        public CalcBrush(byte red, byte green, byte blue)
        {
            Shading = ShadingType.SOLID;
            Color = new CalcColor(255, red, green, blue);
        }
    }

    public enum ShadingType
    {
        SOLID,

    }

    
}
