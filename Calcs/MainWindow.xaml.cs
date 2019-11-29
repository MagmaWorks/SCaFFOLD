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
        double savedWidth = 0;

        public MainWindow()
        {
            Window startScreen = new Window();
            var calcClasses = CalcCore.FindAssemblies.GetAssemblies();
            var plugins = FindPlugins.GetPlugins();
            startScreen.Content = new DynamicRelaxation(startScreen, calcClasses);
            startScreen.WindowStyle = WindowStyle.None;
            startScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startScreen.ShowDialog();

            appVM = new AppViewModel(calcClasses, plugins) ;
            this.DataContext = appVM;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IODisplay.Width = new GridLength(15);
        }
    }
}
