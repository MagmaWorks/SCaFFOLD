using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace Scaffold.Core.CalcObjects
{
    public class CalcConcreteSection : ConcreteSection, ICalcValue
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }

        public CalcConcreteSection(IProfile profile, IMaterial material) : base(profile, material)
        {
        }

        public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link) : base(profile, material, link)
        {
        }

        public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, Length cover) : base(profile, material, link, cover)
        {
        }

        public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, Length cover, IList<ILongitudinalReinforcement> rebars) : base(profile, material, link, cover, rebars)
        {
        }

        public bool TryParse(string strValue)
        {
            try
            {
                CalcConcreteSection section = strValue.FromJson<CalcConcreteSection>();
                DisplayName = section.DisplayName;
                Symbol = section.Symbol;
                Status = section.Status;

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
