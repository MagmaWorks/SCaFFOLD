using MagmaWorks.Taxonomy.Serialization;
using MagmaWorks.Taxonomy.Standards;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcStandard : ICalcValue, IStandard
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }
        public StandardBody Body { get; set; }
        public string Title { get; set; }

        public bool TryParse(string strValue)
        {
            try
            {
                CalcStandard calcStandard = strValue.FromJson<CalcStandard>();
                DisplayName = calcStandard.DisplayName;
                Symbol = calcStandard.Symbol;
                Status = calcStandard.Status;
                Body = calcStandard.Body;
                Title = calcStandard.Title;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ValueAsString()
        {
            return this.ToJson();
        }
    }
}
