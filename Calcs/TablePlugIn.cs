using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CalcCore;

namespace Calcs
{
    public class TablePlugIn : ICalcPlugin
    {
        ICalc _calc;
        public ICalc Calc => _calc;

        UserControl _control;
        public UserControl Control
        {
            get
            {
                return _control;
            }
        }

        TableVM _viewModel;

        public TablePlugIn()
        {
            _viewModel = new TableVM(_calc);
            _control = new CrossRefTable();
        }
    }
}
