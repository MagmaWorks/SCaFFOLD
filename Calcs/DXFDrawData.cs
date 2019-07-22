using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Calcs
{
    public class DXFDrawData
    {
        public Brush LayerBrush { get; private set; }
        public double LayerBrushThickness { get; private set; }
        public Geometry Geometry { get; private set; }

        public DXFDrawData(Brush layerBrush, double thickness, Geometry geometry)
        {
            LayerBrush = layerBrush;
            LayerBrushThickness = thickness;
            Geometry = geometry;
        }
    }
}
