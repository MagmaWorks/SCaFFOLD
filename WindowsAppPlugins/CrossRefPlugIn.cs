using Calcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CalcCore;

namespace WindowsAppPlugins
{
    [PluginName("Cross Ref")]
    public class CrossRefPlugIn : CalcPluginBase
    {
        UserControl _control;
        public override UserControl Control
        {
            get
            {
                return _control;
            }
        }

        CrossRefVM _viewModel;

        public CrossRefPlugIn()
        {

        }

        public CrossRefPlugIn(ICalc calc)
        {
            _calc = (CalcCore.ICalc)Activator.CreateInstance(calc.GetType());
            this.CopyValuesFrom(calc);
            _viewModel = new CrossRefVM(_calc);
            _control = new CrossRef();
            _control.DataContext = _viewModel;
        }

        public override void Initialise(ICalc calc)
        {
            _calc = (CalcCore.ICalc)Activator.CreateInstance(calc.GetType());
            this.CopyValuesFrom(calc);
            _viewModel = new CrossRefVM(_calc);
            _control = new CrossRef();
            _control.DataContext = _viewModel;
        }

        public override void Update()
        {
        }
    }
}
