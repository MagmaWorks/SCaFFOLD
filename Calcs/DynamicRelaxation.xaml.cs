using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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


        public string Version
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString()+
                    "."+
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString()+
                    "."
                    +
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString()
                    ;
            }
        }

        public string Build
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public DynamicRelaxation(Window parent, List<CalcCore.CalcAssembly> calcClasses)
        { 
            parentWindow = parent;
            _dynamicRelaxation = new DynamicRelaxationViewModel();

            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();

            DataContext = this;

            Assemblies = new List<string>();
            Calcs = new List<string>();
            foreach (var calc in calcClasses)
            {
                Calcs.Add(calc.Name);
                if (!Assemblies.Contains(calc.Assembly))
                {
                    Assemblies.Add(calc.Assembly);
                }
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
