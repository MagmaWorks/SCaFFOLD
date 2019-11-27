using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calcs;

namespace WindowsAppPlugins
{
    public class ChartVM : ViewModelBase
    {
        CalcCore.ICalc calc;
        private double _inputStartValue = 0;
        private double _inputEndValue = 100;
        private int _steps = 1;
        private int _selectedInputIndex = 0;

        public ObservableCollection<string> InputSelection { get; set; }
        public int SelectedInputIndex { get => _selectedInputIndex; set { _selectedInputIndex = value; UpdateChartValues(); } }
        public double InputStartValue { get => _inputStartValue; set { _inputStartValue = value; UpdateChartValues(); } }
        public double InputEndValue { get => _inputEndValue; set { _inputEndValue = value; UpdateChartValues(); } }
        public int Steps { get => _steps; set { _steps = value; UpdateChartValues(); } }
        public ObservableCollection<ChartOutputSelection> OutputSelection { get; set; }
        public SeriesCollection ChartSeries { get; set; }

        public ChartVM(CalcCore.ICalc calc)
        {
            this.calc = calc;
            InputSelection = new ObservableCollection<string>();
            foreach (var item in calc.GetInputs())
            {
                InputSelection.Add(item.Name);
            }
            OutputSelection = new ObservableCollection<ChartOutputSelection>();
            foreach (var item in calc.GetOutputs())
            {
                OutputSelection.Add(new ChartOutputSelection(this) { IsSelected = false, Name = item.Name });
            }
            UpdateChartValues();
        }

        public void UpdateChartValues()
        {
            ChartSeries = new SeriesCollection();
            CalcCore.CalcValueBase inputValue = calc.GetInputs()[SelectedInputIndex];
            // remember what value this was before running multiple calcs...
            var input1Start = calc.GetInputs()[SelectedInputIndex].ValueAsString;
            List<List<ObservablePoint>> myList = new List<List<ObservablePoint>>();
            for (int i = 0; i < OutputSelection.Count; i++)
            {
                myList.Add(new List<ObservablePoint>());
            }
            double stepSize = (InputEndValue - InputStartValue) / (_steps - 1);
            for (int i = 0; i < _steps; i++)
            {
                var stepValue = (InputStartValue + i * stepSize);
                inputValue.ValueAsString = stepValue.ToString();
                calc.UpdateCalc();
                var results = calc.GetOutputs();
                for (int j = 0; j < OutputSelection.Count; j++)
                {
                    var result = results[j].ValueAsString;
                    if (results[j].Type == CalcCore.CalcValueType.DOUBLE)
                    {
                        myList[j].Add(new ObservablePoint(stepValue, double.Parse(result)));
                    }
                    else myList[j].Add(new ObservablePoint(stepValue, 0));

                }
            }
            for (int i = 0; i < OutputSelection.Count; i++)
            {
                if (OutputSelection[i].IsSelected == true)
                {
                    ChartSeries.Add(new LineSeries()
                    {
                        Values = new ChartValues<ObservablePoint>(myList[i]),
                        LineSmoothness = 0
                    });
                }
            }
            //...restore starting value
            calc.GetInputs()[SelectedInputIndex].ValueAsString = input1Start;

            RaisePropertyChanged(nameof(ChartSeries));
        }

    }
}
