using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DynamicRelaxation
{
    public class DynamicRelaxationSystem
    {
        public List<Node> Nodes { get; set; }
        public List<Connector> Connectors { get; set; }
        public List<MeshElement> Mesh {get;set;}
        public double Gravity { get; set; }

        public DynamicRelaxationSystem()
        {
            this.Nodes = new List<Node>();
            this.Connectors = new List<Connector>();
            this.Mesh = new List<MeshElement>();

            Gravity = 0;
            double stiffness = 100;

            Nodes.Add(new Node(10, new Point3D(0, 0, 0)) { Fixed = true });
            for (int i = 1; i < 10; i++)
            {
                Nodes.Add(new Node(10, new Point3D(i * 1000, 0, 0)));
                Connectors.Add(new Connector(Nodes[i - 1], Nodes[i]) { NaturalLength = 1000, Stiffness = stiffness });
            }
            for (int i = 1; i < 10; i++)
            {
                Nodes.Add(new Node(10, new Point3D(0, i*1000, 0)));
                Connectors.Add(new Connector(Nodes[i * 10], Nodes[(i-1) * 10]) { NaturalLength = 1000, Stiffness = stiffness });
                for (int j = 1; j < 10; j++)
                {
                    Nodes.Add(new Node(10, new Point3D(j * 1000, i*1000, 0)));
                    Connectors.Add(new Connector(Nodes[i*10 + j - 1], Nodes[i*10+j]) { NaturalLength = 1000, Stiffness = stiffness });
                    Connectors.Add(new Connector(Nodes[i * 10 + j], Nodes[(i - 1) * 10 + j]) { NaturalLength = 1000, Stiffness = stiffness });
                    Mesh.Add(new MeshElement((i-1)*10 + j -1, (i-1)*10 + j, i*10+j-1));
                    Mesh.Add(new MeshElement((i - 1) * 10 + j, i*10+j, i*10+j-1));

                }
            }
            Nodes[99].Position = new Point3D(Nodes[99].Position.X, Nodes[99].Position.Y, 0);
            Nodes[99].Fixed = true;
            Nodes[9].Position = new Point3D(Nodes[9].Position.X, Nodes[9].Position.Y, 0);
            Nodes[9].Fixed = true;
            Nodes[90].Position = new Point3D(Nodes[90].Position.X, Nodes[90].Position.Y, 0);
            Nodes[90].Fixed = true;

            double minX = Nodes.Min(a => a.Position.X);
            double minY = Nodes.Min(a => a.Position.Y);
            double maxX = Nodes.Max(a => a.Position.X);
            double maxY = Nodes.Max(a => a.Position.Y);
             
            foreach (var node in Nodes)
            {
                node.uvPoint = new Point((int)(node.Position.X - minX) / (maxX - minX), (node.Position.Y - minY) / (maxY - minY));
            }

        }

        public void OneStep(float timeStep)
        {
            float innerTimeStep = timeStep / 10;
            for (int i = 0; i < 10; i++)
            {
                foreach (var node in Nodes)
                {
                    node.Force = new Vector3D();
                }
                foreach (var connector in Connectors)
                {
                    Vector3D direction = connector.End.Position - connector.Start.Position;
                    double factor = connector.Stiffness * (1 - direction.Length / connector.NaturalLength);
                    connector.Start.Force += direction * -factor;
                    connector.End.Force += direction * factor;
                }
                foreach (var node in Nodes)
                {
                    node.Force += node.Force + new Vector3D(0, 0, Gravity);
                    if (!node.Fixed)
                    {
                        node.Velocity += node.Force * innerTimeStep;
                        if (node.Velocity.Length > 0)
                        {
                            Vector3D dampingForce = node.Force;
                            dampingForce.Normalize();
                            dampingForce = dampingForce * 20 * -node.Velocity.Length;
                            node.Velocity += dampingForce * innerTimeStep;
                        }
                        node.Position += node.Velocity * innerTimeStep;
                    }
                }
            }

        }

        public void RunSteps(int steps, float timeStep)
        {
            for (int i = 0; i < steps; i++)
            {
                OneStep(timeStep);
            }
        }
    }
}


