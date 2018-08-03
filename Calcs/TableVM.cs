using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calcs
{
    public class TableVM : ViewModelBase
    {
        public List<string> InputSelection { get; set; }
        public int Input1Index { get => _input1Index; set { _input1Index = value; CalcResultsTable(); } }
        public double Start1 { get => _start1; set { _start1 = value; CalcResultsTable(); } }
        public double Step1 { get => _step1; set { _step1 = value; CalcResultsTable(); } }
        public int Steps1 { get => _steps1; set { _steps1 = value; CalcResultsTable(); } }
        public int Input2Index { get => _input2Index; set { _input2Index = value; CalcResultsTable(); } }
        public double Start2 { get => _start2; set { _start2 = value; CalcResultsTable(); } }
        public double Step2 { get => _step2; set { _step2 = value; CalcResultsTable(); } }
        public int Steps2 { get => _steps2; set { _steps2 = value; CalcResultsTable(); } }
        public int OutputIndex { get => _outputIndex; set { _outputIndex = value; CalcResultsTable(); } }
        public List<string> OutputSelection { get; set; }
        public ObservableCollection<string> Input1Headers { get; set; }
        public ObservableCollection<string> Input2Headers { get; set; }
        public string Input1TextHeader { get { return InputSelection[Input1Index]; } }
        public string Input2TextHeader { get { return InputSelection[Input2Index]; } }
        CalcCore.ICalc calc;

        ObservableCollection<ObservableCollection<TableResult>> results;
        private int _input1Index = 0;
        private double _start1 = 0;
        private double _step1 = 1;
        private int _steps1 = 10;
        private int _input2Index = 1;
        private double _start2 = 0;
        private double _step2 = 2;
        private int _steps2 = 5;
        private int _outputIndex = 0;

        public ObservableCollection<ObservableCollection<TableResult>> Results
        {
            get
            {
                return results;
            }
        }

        public TableVM(CalcCore.ICalc calc)
        {
            InputSelection = new List<string>();
            foreach (var item in calc.GetInputs())
            {
                InputSelection.Add(item.Name);
            }
            OutputSelection = new List<string>();
            foreach (var item in calc.GetOutputs())
            {
                OutputSelection.Add(item.Name);
            }
            this.calc = calc;
            CalcResultsTable();
        }

        public void CalcResultsTable()
        {
            results = new ObservableCollection<ObservableCollection<TableResult>>();
            for (int i = 0; i < Steps1; i++)
            {
                ObservableCollection<TableResult> resultRow = new ObservableCollection<TableResult>();
                for (int j = 0; j < Steps2; j++)
                {
                    calc.GetInputs()[Input1Index].ValueAsString = (Start1 + Step1 * i).ToString();
                    calc.GetInputs()[Input2Index].ValueAsString = (Start2 + Step2 * j).ToString();
                    calc.UpdateCalc();
                    resultRow.Add(new TableResult() { Value = calc.GetOutputs()[OutputIndex].ValueAsString });
                }
                results.Add(resultRow);
            }
            Input1Headers = new ObservableCollection<string>();
            for (int i = 0; i < Steps1; i++)
            {
                Input1Headers.Add((Start1 + Step1 * i).ToString());
            }
            Input2Headers = new ObservableCollection<string>();
            for (int i = 0; i < Steps2; i++)
            {
                Input2Headers.Add((Start2 + Step2 * i).ToString());
            }

            RaisePropertyChanged(nameof(Input1Headers));
            RaisePropertyChanged(nameof(Input2Headers));
            RaisePropertyChanged(nameof(Results));
        }
    }
}
