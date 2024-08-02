using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace Scaffold.Calculations
{
    public class PunchingShear : ICalculation
    {
        string _title = "Example";
        public string Title { get => _title; set => _title = value; }
        string _type = "Punching Shear to EC2";
        public string Type { get => _type; set => _type = value; }

        [InputCalcValue("F", "Punching Force")]
        public double _force { get; set; } = 1000;

        [OutputCalcValue("R", "Resistance")]
        public double _resistance { get; set; } = 0;

        public CalcStatus Status => CalcStatus.None;

        public IEnumerable<Formula> GetFormulae()
        {
            return new List<Formula>();
        }
        public PunchingShear()
        {

        }

        public void Update()
        {
            _resistance = _force * 2;
        }
    }
}
