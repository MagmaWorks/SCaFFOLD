using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcDoubleList : CalcValueBase
    {
        public override string ValueAsString
        {
            get
            {
                string lines = "{";
                foreach (var item in Values)
                {
                    string line = "{";
                    foreach (var entry in item)
                    {
                        line += entry.ToString() + " ";
                    }
                    line = line.TrimEnd(' ');
                    line += "}";
                    lines += line;
                }
                lines += "}";
                return lines;
            }
            set
            {
                Values = new List<List<double>>();
                string lines = value;
                if (true)
                {
                    if (!lines.StartsWith("{{") || !lines.EndsWith("}}"))
                    {
                        Values.Add(new List<double> { double.NaN });
                        return;
                    }
                    var parts = lines.Split(new string[] { "}{" }, StringSplitOptions.None);
                    parts[0] = parts.First().Remove(0, 2);
                    parts[parts.Count()-1] = parts.Last().Remove(parts.Last().Length - 2, 2);

                    foreach (var part in parts)
                    {
                        var entries = part.Split(' ');
                        List<double> lineEntries = new List<double>();
                        foreach (var entry in entries)
                        {
                            double result;
                            double.TryParse(entry, out result);
                            lineEntries.Add(result);
                        }
                        Values.Add(lineEntries);
                    }
                }
            }
        }

        public override CalcValueType Type => CalcValueType.DOUBLE;

        List<List<double>> _values;

        public List<List<double>> Values { get; set; }

        

        public CalcDoubleList(string name, List<List<double>> doubles)
        {
            this.Name = name;
            Values = doubles;
            this.Symbol = "";
            this.Unit = "";
        }
    }
}
