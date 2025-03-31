using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcProfile : ICalcValue
    {
        IProfile Profile { get; set; }
        public virtual string DisplayName
        {
            get
            {
                return ValueAsString();
            }

            set
            {
                TryParse(value);
            }
        }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }

        public bool TryParse(string strValue)
        {
            try
            {
                Profile = strValue.FromJson<IProfile>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ValueAsString() => Profile.Description ?? "Invalid Profile";
    }
}
