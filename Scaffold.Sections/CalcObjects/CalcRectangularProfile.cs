using System.Globalization;
using System.Linq;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcObjects
{
    public class CalcRectangularProfile : ICalcValue, IRectangle
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public CalcStatus Status { get; set; }
        public Length Width { get; set; }
        public Length Height { get; set; }
        public string Description { get; }

        public CalcRectangularProfile()
        {
        }

        public CalcRectangularProfile(Length width, Length height)
        {
            Width = width;
            Height = height;
            Description = new Rectangle(Width, Height).Description;
        }

        public bool TryParse(string strValue)
        {
            try
            {
                IRectangle profile = strValue.FromJson<IRectangle>();
                Height = profile.Height;
                Width = profile.Width;
                Description = new Rectangle(Width, Height).Description;
                return true;
            }
            catch { }

            var values = strValue.Split('x').Select(s => s.Trim()).ToList();
            if (values.Count != 2)
            {
                return false;
            }

            try
            {
                Width = Length.Parse(values[0], CultureInfo.InvariantCulture);
                Height = Length.Parse(values[1], CultureInfo.InvariantCulture);
            }
            catch { }

            return false;
        }

        public string ValueAsString() => Description ?? "Invalid profile";
    }
}
