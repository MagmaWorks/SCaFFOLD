using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class IOSelectionItems
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public CalcCore.CalcValueType CalcValueType { get; set; }
        public List<string> Items { get; set; }
    }
}
