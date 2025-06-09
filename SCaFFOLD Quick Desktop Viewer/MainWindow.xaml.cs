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

        public MainWindow()
        {
            InitializeComponent();

            calc = new TestCalculation();
            calc.Calculate();
            viewModel = new CalcViewModel(calc);
            this.DataContext = viewModel;
        }
    }
}
