using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace DynamicRelaxation
{
    public class Node
    {
        public double Mass { get; set; }

        public Point3D Position { get; set; }

        public Vector3D Velocity { get; set; }

        public Vector3D Force { get; set; }

        public System.Windows.Point uvPoint { get; set; }

        public bool Fixed { get; set; } = false;
        
        public Node(double mass, Point3D position)
        {
            Mass = mass;
            Position = position;
            Velocity = new Vector3D(0, 0, 0);
            Force = new Vector3D(0, 0, 0);
            uvPoint = new System.Windows.Point(0,0);
        }
    }
}
