using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class FormulaeVM : ViewModelBase
    {
        public List<string> Expression { get; set; }
        public string Ref { get; set; }
        public string Narrative { get; set; }
        public string Conclusion { get; set; }
        public CalcCore.CalcStatus Status { get; set; }
    }
}
