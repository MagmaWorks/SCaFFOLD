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
    /// Interaction logic for DynamicRelaxation.xaml
    /// </summary>
    public partial class DynamicRelaxation : UserControl
    {
        Window parentWindow;

        DynamicRelaxationViewModel _dynamicRelaxation;
        public DynamicRelaxationViewModel DynamicRelaxationVM
        {
            get
            {
                return _dynamicRelaxation;
            }
            set
            {
                _dynamicRelaxation = value;
            }
        }

        public List<string> Calcs { get; private set; }

        public List<string> Assemblies { get; private set; }

        public DynamicRelaxation(Window parent)
        { 
            parentWindow = parent;
            _dynamicRelaxation = new DynamicRelaxationViewModel();

            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();

            DataContext = this;

            var calcClasses = CalcCore.FindAssemblies.GetAssemblies();
            var calcs = new List<CalculationViewModel>();
            Assemblies = new List<string>();
            //foreach (var calc in calcClasses)
            //{
            //    CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(calc.Class);
            //    calcs.Add(new CalculationViewModel(calcInstance));
            //    if (!Assemblies.Contains(calc.Assembly))
            //    {
            //        Assemblies.Add(calc.Assembly);
            //    }
            //}
            Calcs = new List<string>();
            foreach (var calc in calcs)
            {
                Calcs.Add(calc.CalcTypeName);
            }

            InitializeComponent();
        }

        void dispatcherTimer_Tick(object sender, EventArgs args)
        {
            DynamicRelaxationVM.Update();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.Close();
        }
    }
}
