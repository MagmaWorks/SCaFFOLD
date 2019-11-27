using CalcCore;
using Calcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WindowsAppPlugins
{
    [PluginName("Chart")]
    public class ChartPlugIn : CalcPluginBase
    {
        UserControl _control;
        public override UserControl Control
        {
            get
            {
                return _control;
            }
        }

        ChartVM _viewModel;

        public ChartPlugIn()
        {

        }

        public ChartPlugIn(ICalc calc)
        {
            _calc = calc;
            _viewModel = new ChartVM(_calc);
            _control = new ChartControl();
            _control.DataContext = _viewModel;
        }

        public override void Initialise(ICalc calc)
        {
            _calc = calc;
            _viewModel = new ChartVM(_calc);
            _control = new ChartControl();
            _control.DataContext = _viewModel;
        }

        public override void Update()
        {
            _viewModel.UpdateChartValues();
        }
    }
}
