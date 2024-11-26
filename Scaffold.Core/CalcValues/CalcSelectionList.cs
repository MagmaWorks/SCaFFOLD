using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public class CalcSelectionList : CalcValue<List<string>>
{
    private readonly List<string> _selectionList;

    public CalcSelectionList(string name, string selectedValue, IEnumerable<string> values)
        : base(name)
    {
        _selectionList = values.ToList();

        var existing = SelectionList.FirstOrDefault(x => x == selectedValue);
        Value = existing == null ? SelectionList[0] : selectedValue;
    }

    public IReadOnlyList<string> SelectionList => _selectionList;
    public string Value { get; private set; }

    public override void SetValue(string strValue)
    {
        var exists = SelectionList.FirstOrDefault(x => x == strValue);
        if (exists == null)
            return;

        Value = strValue;
    }

    public override string ToString() => string.Join(", ", Value);
}
