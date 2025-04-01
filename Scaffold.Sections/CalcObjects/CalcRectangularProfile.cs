using MagmaWorks.Geometry;
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ValueAsString() => Description ?? "Invalid profile";
    }
}
