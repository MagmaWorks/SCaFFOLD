using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcListOfDoubleArrays : CalcValueBase
    {
        public override string ValueAsString
        {
            get
            {
                return convertToString();
            }
            set
            {
                var valueList = new List<double[]>();
                string lines = value;

                if (!lines.StartsWith("{{") || !lines.EndsWith("}}"))
                {
                    valueList.Add(new double[] { double.NaN });
                    return;
                }
                var parts = lines.Split(new string[] { "}{" }, StringSplitOptions.None);
                parts[0] = parts.First().Remove(0, 2);
                parts[parts.Count() - 1] = parts.Last().Remove(parts.Last().Length - 2, 2);

                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    var entries = part.Split(' ');
                    double[] lineEntries = new double[entries.Length];
                    for (int j = 0; j < entries.Length; j++)
                    {
                        var entry = entries[j];
                        double result;
                        double.TryParse(entry, out result);
                        lineEntries[j] = result;
                    }
                    valueList.Add(lineEntries);
                }
                Value = valueList;
            }
        }

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
            s.Append("{");
            foreach (var item in _value)
            {
                string line = "{";
                foreach (var item2 in item)
                {
                    line += item2.ToString() + " ";
                }
                line = line.Trim() + "}";
                s.Append(line);
            }
            s.Append("}");
            return s.ToString();
        }

    }
}
