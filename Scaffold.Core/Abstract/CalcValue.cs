﻿using System.ComponentModel;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcValue<T> : ICalcValue, IEquatable<CalcValue<T>>
{
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }
    public T Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string ValueAsString() => ToString();

    protected CalcValue(T value, string name, string symbol, string unit = "")
    {
        Value = value;
        DisplayName = name;
        Symbol = symbol?.Trim();
        Unit = unit?.Trim();
    }

    public static implicit operator T(CalcValue<T> value) => value.Value;

    public virtual bool TryParse(string input)
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                Value = (T)converter.ConvertFromString(input);
                return true;
            }
        }
        catch (NotSupportedException) { }

        return false;
    }

    internal static (string name, string symbol, string unit) OperatorMetadataHelper(
        CalcValue<T> x, CalcValue<T> y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        string unit = x.Unit == y.Unit ? x.Unit : string.Empty;
        return (name, symbol, unit);
    }

    public override string ToString() => $"{Value}{Unit}";

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        if (obj is CalcValue<T> other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return DisplayName.GetHashCode() ^ Symbol.GetHashCode() ^ Status.GetHashCode()
            ^ Value.GetHashCode() ^ Unit.GetHashCode();
    }

    public bool Equals(CalcValue<T> other) => Value.Equals(other.Value) && Unit == other.Unit;
}
