using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcCore3DModel
    {
        public List<CalcCoreMesh> Meshes { get; private set; }

        public CalcCore3DModel(CalcCoreMesh mesh)
        {
            Meshes = new List<CalcCoreMesh> { mesh };
        }
    }
}
