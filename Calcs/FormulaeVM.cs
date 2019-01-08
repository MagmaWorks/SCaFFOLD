using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Calcs
{
    public class FormulaeVM : ViewModelBase
    {
        public List<string> Expression { get; set; }
        public string Ref { get; set; }
        public string Narrative { get; set; }
        public string Conclusion { get; set; }
        public CalcCore.CalcStatus Status { get; set; }
        public BitmapSource Image { get; set; }
    }
}
