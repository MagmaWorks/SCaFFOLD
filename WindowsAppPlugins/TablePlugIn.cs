using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CalcCore;
using Calcs;

namespace WindowsAppPlugins
{
    [PluginName("Table 2D")]
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

        public TablePlugIn()
        {

        }

        public TablePlugIn(ICalc calc)
        {
            _calc = calc;
            _viewModel = new TableVM(_calc);
            _control = new CrossRefTable();
            _control.DataContext = _viewModel;
        }

        public override void Initialise(ICalc calc)
        {
            _calc = calc;
            _viewModel = new TableVM(_calc);
            _control = new CrossRefTable();
            _control.DataContext = _viewModel;
        }

        public override void Update()
        {
            _viewModel.CalcResultsTable();
        }
    }
}
