using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcProfile : ICalcValue, IProfile
    {
        public string DisplayName { get => Description; set => Description = value; }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }
        public string Description { get; set; }

        public CalcProfile(string description)
        {
            Description = description;
        }

        public bool TryParse(string description)
        {
            if (description == null)
            {
                return false;
            }

            Description = description;
            return true;
        }

        public string ValueAsString() => Description ?? "Invalid Profile";
    }
}
