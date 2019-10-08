using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcColor
    {
        Int32 _color;
        public Int32 Color { get { return _color; }  }

        public byte Alpha
        {
            get
            {
                return (byte)((_color >> (8 * 0)) & 0xff);
            }
        }
        public byte Red
        {
            get
            {
                return (byte)((_color >> (8 * 1)) & 0xff);
            }
        }
        public byte Green
        {
            get
            {
                return (byte)((_color >> (8 * 2)) & 0xff);
            }
        }
        public byte Blue
        {
            get
            {
                return (byte)((_color >> (8 * 3)) & 0xff);
            }
        }
        public CalcColor(byte alpha, byte red, byte green, byte blue)
        {
            _color = BitConverter.ToInt32(new byte[] { alpha, red, green, blue }, 0);
        }


    }
}
