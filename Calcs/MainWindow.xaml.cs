using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        AppViewModel appVM;

        public MainWindow()
        {
            Window startScreen = new Window();
            startScreen.Content = new DynamicRelaxation(startScreen);
            startScreen.WindowStyle = WindowStyle.None;
            startScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startScreen.ShowDialog();

            var calcClasses = CalcCore.FindAssemblies.GetAssemblies();
            var calcs = new ObservableCollection<CalculationViewModel>();
                CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(calcClasses[0].Class);
                calcs.Add(new CalculationViewModel(calcInstance));
            appVM = new AppViewModel() { Assemblies = calcClasses, ViewModels = calcs };
            this.DataContext = appVM;
            InitializeComponent();
        }
    }
}
