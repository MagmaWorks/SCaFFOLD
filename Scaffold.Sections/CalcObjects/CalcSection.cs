using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcSection : ICalcValue
    {
        public string DisplayName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Symbol { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public CalcStatus Status => throw new System.NotImplementedException();

        public bool TryParse(string strValue) => throw new System.NotImplementedException();
        public string ValueAsString() => throw new System.NotImplementedException();
    }
}
