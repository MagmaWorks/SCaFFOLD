// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scaffold.Calculations;
using Scaffold.Calculations.Eurocode.Steel;
using Scaffold.Core;
using Scaffold.Core.Interfaces;


namespace SCaFFOLD_Quick_Desktop_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ICalculation calc;
        CalcViewModel viewModel;

        //temp testing drag and move geometry
        Point pt;
        Path myShape;
        bool isMoving = false;

        public MainWindow()
        {
            InitializeComponent();

            calc = new SteelCatalogueProfile();
            calc.Calculate();
            viewModel = new CalcViewModel(calc);
            this.DataContext = viewModel;
        }

        void LeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            var canvas = sender as Canvas;
            pt = args.GetPosition(canvas);
            HitTestResult result = VisualTreeHelper.HitTest(canvas, pt);
            myShape = result.VisualHit as Path;
            if (myShape != null)
            {
                isMoving = true;
            }
        }

        void LeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            if (!isMoving) return;
            var canvas = sender as Canvas;
            var pt2 = args.GetPosition(canvas);
            (myShape.Data as EllipseGeometry).Center = pt + (pt2 - pt);
            isMoving = false;
        }

        void MouseOnTheMove(object sender, MouseEventArgs args)
        {
            if (!isMoving) return;

            var canvas = sender as Canvas;
            var pt2 = args.GetPosition(canvas);
            (myShape.Data as EllipseGeometry).Center = pt + (pt2 - pt);

        }

        void MouseLeavesCanvas(object sender, MouseEventArgs args)
        {
            if (!isMoving) return;

            (myShape.Data as EllipseGeometry).Center = pt;
            isMoving = false;

        }

    }
}
