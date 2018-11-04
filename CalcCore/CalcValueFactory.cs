using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcValueFactory
    {
        List<CalcValueBase> values;

        public CalcValueFactory()
        {
            values = new List<CalcValueBase>();
        }

        public CalcDouble CreateDoubleCalcValue(string name, string symbol, string unit, double value)
        {
            var myVal = new CalcDouble(name, symbol, unit, value);
            values.Add(myVal);
            return myVal;
        }

        public CalcSelectionList CreateCalcSelectionList(string name, string selectedValue, IEnumerable<string> values)
        {
            var myVal = new CalcSelectionList(name, selectedValue, values);
            this.values.Add(myVal);
            return myVal;
        }

        public CalcFilePath CreateCalcFilePath(string name, string path)
        {
            var myVal = new CalcFilePath(name, path);
            this.values.Add(myVal);
            return myVal;
        }

        public List<CalcValueBase> GetValues()
        {
            return values.ToList();
        }

    }
}
