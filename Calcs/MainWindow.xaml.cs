using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(calcClasses[1].Class);
                calcs.Add(new CalculationViewModel(calcInstance));
            appVM = new AppViewModel(calcClasses) ;
            this.DataContext = appVM;
            //this.Closing += appVM.SaveAllOnClose;//
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = new SaveOnQuitVM();
            Window1 myWin = new Window1(vm);
            myWin.ShowDialog();
            if (vm.Save)
            {
                appVM.saveAll();
            }
            if (vm.Cancel)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}
