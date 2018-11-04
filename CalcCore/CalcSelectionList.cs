using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcSelectionList : CalcValueBase
    {
        string selectedValue = "";
        List<string> values;

        public List<string> SelectionList
        {
            get
            {
                return values.ToList();
            }
        }

        public override string ValueAsString
        {
            get
            {
                return selectedValue;
            }
            set
            {
                assignValueIfInList(value);
            }
        }

        public override CalcValueType Type { get { return CalcValueType.SELECTIONLIST; } }

        public CalcSelectionList(string name, string selectedValue, IEnumerable<string> values)
        {
            this.values = values.ToList();
            this.selectedValue = this.values[0];
            assignValueIfInList(selectedValue);
            this.Name = name;
            this.Symbol = "";
            this.Unit = "";
        }

        void assignValueIfInList(string attemptedSelectedValue)
        {
            foreach (var item in values)
            {
                if (attemptedSelectedValue == item)
                {
                    selectedValue = attemptedSelectedValue;
                }
            }
        }
    }
}
