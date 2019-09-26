using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using DynamicRelaxation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Calcs
{
    public class DynamicRelaxationViewModel : ViewModelBase
    {
        Model3D model;
        Model3D modelOriginal;
        Model3D model2;
        ImageBrush brush;
        DiffuseMaterial mat;
        int steps = 0;

        public Model3D Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(nameof(Model)); }
        }


        public Model3D Model2
        {
            get { return model2; }
            set { model2 = value; RaisePropertyChanged(nameof(Model2)); }
        }

        DynamicRelaxationSystem dynamicRelaxationSystem;

        public DynamicRelaxationViewModel()
        {
            dynamicRelaxationSystem = new DynamicRelaxationSystem();
            brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Calcs;component/CalcPage.png"));
            brush.TileMode = TileMode.None;
            brush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            brush.Stretch = Stretch.Fill;
            brush.RelativeTransform = new ScaleTransform(-1, 1) { CenterX = 0.5, CenterY = 0.5 };
            brush.AlignmentX = AlignmentX.Left;
            brush.AlignmentY = AlignmentY.Top;
            brush.Viewport = new Rect(0, 0, 1, 1);
            mat = new DiffuseMaterial(brush);

            //UN COMMENT THIS SECTION FOR MAGMA MADE LOGO
            //ObjReader reader = new ObjReader();
            //Model3DGroup group1 = reader.Read(@"C:\Users\Alex Baalham\Documents\magmaworks1.obj");
            //ObjReader reader2 = new ObjReader();
            //Model3DGroup group2 = reader2.Read(@"C:\Users\Alex Baalham\Documents\magmaworks2.obj");
            //ObjReader reader3 = new ObjReader();
            //Model3DGroup group3 = reader3.Read(@"C:\Users\Alex Baalham\Documents\magmaworks3.obj");

            ////ObjReader reader2 = new ObjReader();
            ////Model3DGroup group4 = reader2.Read(@"C:\Users\Alex Baalham\Documents\calcapp.obj");

            //Model3DGroup group = new Model3DGroup();
            //group.Children.Add(group1);
            //group.Children.Add(group2);
            //group.Children.Add(group3);

            //Model = group;

            //Model = importedModel;
            //StLReader stlRead = new StLReader();
            //importedModel = stlRead.Read(@"C:\Users\Alex Baalham\Documents\53 Gough Way\test2.stl");
            //Model = importedModel;
            Update();
        }

        public void Update()
        {
            dynamicRelaxationUpdate();
            //magmamadeUpdate();

        }


        private void magmamadeUpdate()
        {
            steps++;
            RaisePropertyChanged(nameof(Model));
        }

        private void dynamicRelaxationUpdate()
        {
            steps++;
            if (steps > 700)
            {
                dynamicRelaxationSystem.Gravity = 20000;
                steps = -250;
                foreach (var item in dynamicRelaxationSystem.Connectors)
                {
                    item.NaturalLength = 1000;
                }
            }
            else if (steps > 500)
            {
                if (steps == 501)
                {
                    foreach (var item in dynamicRelaxationSystem.Connectors)
                    {
                        item.NaturalLength = 700;
                    }
                }
                dynamicRelaxationSystem.Nodes[0].Position += new Vector3D(0, 0, -20);
                dynamicRelaxationSystem.Nodes[9].Position += new Vector3D(0, 0, 20);
                dynamicRelaxationSystem.Nodes[90].Position += new Vector3D(0, 0, 20);
                dynamicRelaxationSystem.Nodes[99].Position += new Vector3D(0, 0, -20);
                dynamicRelaxationSystem.Gravity = 0;
            }
            else if (steps > 300)
            {
                if (steps == 301)
                {
                    foreach (var item in dynamicRelaxationSystem.Connectors)
                    {
                        item.NaturalLength = 700;
                    }
                }
                dynamicRelaxationSystem.Nodes[0].Position += new Vector3D(0, 0, 20);
                dynamicRelaxationSystem.Nodes[9].Position += new Vector3D(0, 0, -20);
                dynamicRelaxationSystem.Nodes[90].Position += new Vector3D(0, 0, -20);
                dynamicRelaxationSystem.Nodes[99].Position += new Vector3D(0, 0, 20);
                dynamicRelaxationSystem.Gravity = 0;
            }
            else if (steps == 25)
            {
                dynamicRelaxationSystem.Gravity = -20000;
            }


            var m = new Model3DGroup();
            var gm = new MeshBuilder();
            dynamicRelaxationSystem.OneStep(0.1f);
            //foreach (var item in dynamicRelaxationSystem.Nodes)
            //{
            //    gm.AddSphere(item.Position, 100,4,4);
            //}

            MeshBuilder meshBuilder = new MeshBuilder(false, true);

            foreach (var pos in dynamicRelaxationSystem.Nodes)
            {
                meshBuilder.Positions.Add(pos.Position);
                meshBuilder.TextureCoordinates.Add(pos.uvPoint);
            }
            foreach (var meshElement in dynamicRelaxationSystem.Mesh)
            {
                meshBuilder.AddTriangle(meshElement.Nodes);
            }


            //m.Children.Add(new GeometryModel3D(gm.ToMesh(), Materials.Gold));
            m.Children.Add(new GeometryModel3D(meshBuilder.ToMesh(true), mat));

            Model = m;
        }
    }
}
