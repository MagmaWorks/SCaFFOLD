using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calcs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var calcClasses = FindAssemblies.GetAssemblies();
            var calcs = new List<CalculationViewModel>();
            foreach (var calc in calcClasses)
            {
                CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(calc.Class);
                calcs.Add(new CalculationViewModel(calcInstance));
            }
            AppViewModel myAppVM = new AppViewModel() { ViewModels = calcs, Batcher = new BatchVM(calcClasses) };
            this.DataContext = myAppVM;
            InitializeComponent();
        }
    }
}
