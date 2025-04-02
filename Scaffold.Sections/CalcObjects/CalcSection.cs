using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcSection : ICalcValue, ISection
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }
        public IMaterial Material => throw new System.NotImplementedException();
        public IProfile Profile => throw new System.NotImplementedException();

        public bool TryParse(string strValue)
        {
            try
            {
                CalcStandard calcStandard = strValue.FromJson<CalcStandard>();
                DisplayName = calcStandard.DisplayName;
                Symbol = calcStandard.Symbol;
                Status = calcStandard.Status;

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
