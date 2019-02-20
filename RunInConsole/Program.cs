using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunInConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var myDynMod = new DynamicRelaxation.DynamicRelaxationSystem();
            myDynMod.RunSteps(100, 0.1f);
            Console.ReadLine();
        }
    }
}
