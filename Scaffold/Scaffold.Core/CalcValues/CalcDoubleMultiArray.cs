using Scaffold.Core.CalcValues.Abstract;
using Scaffold.Core.Interfaces;

// TODO: output is not currently formatting.
namespace Scaffold.Core.CalcValues
{
    public class CalcDoubleMultiArray : CalcValue<List<double[]>>
    {
        public CalcDoubleMultiArray(IoDirection group, string name, List<double[]> multiDimensionalArray)
            : base(group, name, "", null)
        {
            Value = multiDimensionalArray;
        }

        public override CalcValueType GetCalcType() => CalcValueType.LISTOFDOUBLEARRAYS;

        public override void SetValueFromString(string strValue)
        {
            var valueList = new List<double[]>();
            
            if (!strValue.StartsWith("{{") || !strValue.EndsWith("}}"))
            {
                valueList.Add(new[] { double.NaN });
                return;
            }
            
            var parts = strValue.Split(new string[] { "}{" }, StringSplitOptions.None);
            parts[0] = parts.First().Remove(0, 2);
            parts[parts.Count() - 1] = parts.Last().Remove(parts.Last().Length - 2, 2);

            foreach (var part in parts)
            {
                var entries = part.Split(' ');
                var lineEntries = new double[entries.Length];
                for (var j = 0; j < entries.Length; j++)
                {
                    var entry = entries[j];
                    double.TryParse(entry, out var result);
                    lineEntries[j] = result;
                }
                valueList.Add(lineEntries);
            }
            
            Value = valueList;
        }
        
    }
}