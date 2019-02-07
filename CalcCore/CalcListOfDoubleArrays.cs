using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcListOfDoubleArrays : CalcValueBase
    {
        public override string ValueAsString { get => convertToString(); set => throw new NotImplementedException(); }

        public override CalcValueType Type { get { return CalcValueType.LISTOFDOUBLEARRAYS; } }

        private List<double[]> _value;

        public List<double[]> Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public CalcListOfDoubleArrays(string name, List<double[]> doubles)
        {
            this.Name = name;
            this._value = doubles;
        }

        private string convertToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in _value)
            {
                string line = "{";
                foreach (var item2 in item)
                {
                    line += item2.ToString() + " ";
                }
                line.TrimEnd(',');
                line += "}";
                s.Append(line);
            }
            return s.ToString();
        }

    }
}
