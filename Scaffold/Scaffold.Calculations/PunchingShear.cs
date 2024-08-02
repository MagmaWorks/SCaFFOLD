using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Calculations
{
    public class PunchingShear : ICalculation
    {
        public string Title { get => "test"; set => throw new NotImplementedException(); }

        public string Type => "punching";

        public CalcStatus Status => CalcStatus.None;

        public IEnumerable<Formula> GetFormulae()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ICalcValue> GetInputs()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ICalcValue> GetOutputs()
        {
            throw new NotImplementedException();
        }
    }
}
