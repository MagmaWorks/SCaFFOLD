using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Calculations
{
    public class PunchingShear : ICalculation
    {
        public string Title { get => "test"; set => throw new NotImplementedException(); }
        string ICalculation.Type { get; set; }

        public string Type => "punching";

        public CalcStatus Status => CalcStatus.None;
        public IEnumerable<Formula> GetFormulae()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
