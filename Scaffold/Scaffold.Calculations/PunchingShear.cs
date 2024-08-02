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
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CalcStatus Status => throw new NotImplementedException();

        public IEnumerable<Formula> GetFormulae()
        {
            throw new NotImplementedException();
        }

        public PunchingShear()
            {

        
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
