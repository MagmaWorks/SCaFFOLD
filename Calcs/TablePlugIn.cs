using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CalcCore;

namespace Calcs
{
    public class TablePlugIn : CalcPluginBase
    {
        UserControl _control;
        public override UserControl Control
        {
            get
            {
                return _control;
            }
        }

        TableVM _viewModel;

        public TablePlugIn(ICalc calc)
        {
            CalcCore.ICalc _calc = (CalcCore.ICalc)Activator.CreateInstance(calc.GetType());
            CopyValuesFrom(calc);
            _viewModel = new TableVM(_calc);
            _control = new CrossRefTable();
            _control.DataContext = _viewModel;
        }
    }
}
