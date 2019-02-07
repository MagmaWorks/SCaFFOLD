using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcMonkey
{
    public static class AvailableCalcsLoader
    {
        static List<CalcCore.CalcAssembly> _availableCalcs;

        public static List<CalcCore.CalcAssembly> AvailableCalcs
        {
            get
            {
                return _availableCalcs ?? (_availableCalcs = CalcCore.FindAssemblies.GetAssemblies());
            }
        }
    }
}
