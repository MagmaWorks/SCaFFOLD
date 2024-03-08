using Scaffold.Core.CalcValues.Abstract;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcValues
{
    public class CalcSelectionList : CalcValue<string>
    {
        private List<string> _selectionList;
        public IReadOnlyList<string> SelectionList => _selectionList;

        public CalcSelectionList(IoDirection group, string name, string selectedValue, IEnumerable<string> values) 
            : base(group, name, "", null)
        {
            _selectionList = values.ToList();
            
            var existing = SelectionList.FirstOrDefault(x => x == selectedValue);
            Value = existing == null ? SelectionList[0] : selectedValue;
        }

        public override CalcValueType GetCalcType() => CalcValueType.SELECTIONLIST;
        public override void SetValueFromString(string strValue)
        {
            var exists = SelectionList.FirstOrDefault(x => x == strValue);
            if (exists == null)
                return;

            Value = strValue;
        }
    }
}