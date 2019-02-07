using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcAssembly
    {
        public Type Class { get; private set; }
        public string Assembly { get; private set; }

        public CalcAssembly(Type type, string assembly)
        {
            Class = type;
            Assembly = assembly;
        }
    }
}
