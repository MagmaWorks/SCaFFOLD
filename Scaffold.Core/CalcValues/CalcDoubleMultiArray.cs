﻿using System.Text;
using Scaffold.Core.Abstract;

// TODO: output is not currently formatting.
namespace Scaffold.Core.CalcValues;

public class CalcDoubleMultiArray : CalcValue<List<double[]>>//(name)
{
    public List<double[]> Value { get; private set; }   // TODO: Default value from type param.
    public void ResetArray() => Value.Clear();

    public CalcDoubleMultiArray(List<double[]> value, string name)
        : base(value, name, "" , "") 
    {
        Value = value;
        DisplayName = name;
    }

    public override bool TryParse(string strValue)
    {
        var valueList = new List<double[]>();

        if (!strValue.StartsWith("{{") || !strValue.EndsWith("}}"))
        {
            valueList.Add(new[] { double.NaN });
            return true;
        }

        var parts = strValue.Split(new string[] { "}{" }, StringSplitOptions.None);
        parts[0] = parts.First().Remove(0, 2);
        parts[parts.Count() - 1] = parts.Last().Remove(parts.Last().Length - 2, 2);

        foreach (var part in parts)
        {
            var entries = part.Split(',');
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
        return true;
    }

    public override string ToString()
    {
        var innerStr = new StringBuilder();
        foreach (var item in Value)
        {
            innerStr.Append('{');

            foreach (var item2 in item)
            {
                innerStr.Append($"{item2},");
            }

            innerStr.Append('}');
        }

        var final = new StringBuilder();
        final.Append('{').Append(innerStr).Append('}');

        return final.ToString();
    }

}
