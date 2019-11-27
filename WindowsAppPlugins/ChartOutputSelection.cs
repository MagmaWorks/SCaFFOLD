using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAppPlugins
{
    public class ChartOutputSelection
    {
        public bool IsSelected { get => _isSelected; set { _isSelected = value; chartvm.UpdateChartValues(); } }
        public string Name { get; set; }
        public ChartVM chartvm;
        private bool _isSelected;

        public ChartOutputSelection(ChartVM chartvm)
        {
            this.chartvm = chartvm;
        }
    }
}
