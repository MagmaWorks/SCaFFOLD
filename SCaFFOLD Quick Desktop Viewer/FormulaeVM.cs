using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Scaffold.Core;
using Scaffold.Core.Enums;

namespace SCaFFOLD_Quick_Desktop_Viewer
{
    public class FormulaeVM : ViewModelBase
    {
        public List<string> Expression { get; set; }
        public string Ref { get; set; }
        public string Narrative { get; set; }
        public string Conclusion { get; set; }
        public CalcStatus Status { get; set; }
        public BitmapSource Image { get; set; }
    }
}
