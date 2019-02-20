using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRelaxation
{
    public class Connector
    {
        public double NaturalLength { get; set; }

        public double Stiffness { get; set; }

        public Node Start { get; }

        public Node End { get; }

        public Connector(Node start, Node end )
        {
            Start = start;
            End = end;
        }
    }
}
