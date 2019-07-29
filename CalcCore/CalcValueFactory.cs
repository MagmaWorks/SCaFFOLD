using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcValueFactory
    {
        Dictionary<string, CalcValueBase> values;
        public Dictionary<string, CalcValueBase> Values
        {
            get
            {
                return values;
            }
        }

        public CalcValueFactory()
        {
            values = new Dictionary<string, CalcValueBase>();
        }

        public CalcDouble CreateDoubleCalcValue(string name, string symbol, string unit, double value)
        {
            var myVal = new CalcDouble(name, symbol, unit, value);
            values.Add(getAvailableKeyName(name),myVal);
            return myVal;
        }

        public CalcSelectionList CreateCalcSelectionList(string name, string selectedValue, IEnumerable<string> values)
        {
            var myVal = new CalcSelectionList(name, selectedValue, values);
            this.values.Add(getAvailableKeyName(name), myVal);
            return myVal;
        }

        public CalcFilePath CreateCalcFilePath(string name, string path)
        {
            var myVal = new CalcFilePath(name, path);
            this.values.Add(getAvailableKeyName(name), myVal);
            return myVal;
        }

        public CalcFolderPath CreateCalcFolderPath(string name, string path)
        {
            var myVal = new CalcFolderPath(name, path);
            this.values.Add(getAvailableKeyName(name), myVal);
            return myVal;
        }

        //public CalcDoubleList CreateCalcDoubleList(string name, List<List<double>> doubles)
        //{
        //    var myVal = new CalcDoubleList(name, doubles);
        //    this.values.Add(getAvailableKeyName(name), myVal);
        //    return myVal;
        //}

        public CalcListOfDoubleArrays CreateCalcListOfDoubleArrays(string name, List<double[]> doubles)
        {
            var myVal = new CalcListOfDoubleArrays(name, doubles);
            this.values.Add(getAvailableKeyName(name), myVal);
            return myVal;
        }

        public List<CalcValueBase> GetValues()
        {
            return values.Values.ToList();
        }

        private string getAvailableKeyName(string proposedName)
        {
            bool nameOK = false;
            int suffix = 1;
            string name = proposedName;
            while (!nameOK)
            {
                if (values.ContainsKey(name))
                {
                    name = proposedName + "_" + suffix;
                    suffix++;
                }
                else
                {
                    nameOK = true;
                }
            }
            return name;
        }

    }
}
