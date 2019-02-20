using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRelaxation
{
    public class MeshElement
    {
        public int Node1 { get; }
        public int Node2 { get; }
        public int Node3 { get; }

        public MeshElement(int node1, int node2, int node3)
        {
            Node1 = node1;
            Node2 = node2;
            Node3 = node3;
            _nodes = new List<int> { Node1, Node2, Node3 };
        }

        List<int> _nodes;
        public List<int> Nodes
        {
            get
            {
                return _nodes;
            }
        }
    }
}
