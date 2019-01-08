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
        public List<IOSelectionItems> InputSelection { get; set; }
        public int Input1Index { get => _input1Index; set { _input1Index = value; CalcResultsTable(); } }
        public CalcCore.CalcValueType Input1Type { get { return InputSelection[Input1Index].CalcValueType; } }
        public double Start1 { get => _start1; set { _start1 = value; CalcResultsTable(); } }
        public List<string> Start1Items { get { return InputSelection[Input1Index].Items; } }
        public int Start1_1 { get { return _start1_1; } set {_start1_1 = value; CalcResultsTable(); } }
        public int Start1_2 { get { return _start1_2; } set { _start1_2 = value; CalcResultsTable(); } }
        public double Step1 { get => _step1; set { _step1 = value; CalcResultsTable(); } }
        public int Steps1 { get => _steps1; set { _steps1 = value; CalcResultsTable(); } }
        public int Input2Index { get => _input2Index; set { _input2Index = value; CalcResultsTable(); } }
        public CalcCore.CalcValueType Input2Type { get { return InputSelection[Input2Index].CalcValueType; } }
        public double Start2 { get => _start2; set { _start2 = value; CalcResultsTable(); } }
        public List<string> Start2Items { get { return InputSelection[Input2Index].Items; } }
        public int Start2_1 { get { return _start2_1; } set { _start2_1 = value; CalcResultsTable(); } }
        public int Start2_2 { get { return _start2_2; } set { _start2_2 = value; CalcResultsTable(); } }
        public double Step2 { get => _step2; set { _step2 = value; CalcResultsTable(); } }
        public int Steps2 { get => _steps2; set { _steps2 = value; CalcResultsTable(); } }
        public int OutputIndex { get => _outputIndex; set { _outputIndex = value; CalcResultsTable(); } }
        public List<string> OutputSelection { get; set; }
        public ObservableCollection<string> Input1Headers { get; set; }
        public ObservableCollection<string> Input2Headers { get; set; }
        public string Input1TextHeader { get { return InputSelection[Input1Index].Name; } }
        public string Input2TextHeader { get { return InputSelection[Input2Index].Name; } }
        CalcCore.ICalc calc;

        ObservableCollection<ObservableCollection<TableResult>> results;
        private int _input1Index = 0;
        private double _start1 = 0;
        private int _start1_1 = 0;
        private int _start1_2 = 0;
        private double _step1 = 1;
        private int _steps1 = 10;
        private int _input2Index = 0;
        private double _start2 = 0;
        private int _start2_1 = 0;
        private int _start2_2 = 0;
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
            InputSelection = new List<IOSelectionItems>();
            foreach (var item in calc.GetInputs())
            {
                if (item.Type == CalcCore.CalcValueType.DOUBLE)
                {
                    InputSelection.Add(new IOSelectionItems() { Name = item.Name, IsEnabled = true, CalcValueType=item.Type });
                }
                else if (item.Type == CalcCore.CalcValueType.SELECTIONLIST)
                {
                    InputSelection.Add(new IOSelectionItems() { Name = item.Name, IsEnabled = true, CalcValueType = item.Type , Items = (item as CalcCore.CalcSelectionList).SelectionList});
                }
                else
                {
                    InputSelection.Add(new IOSelectionItems() { Name = item.Name, IsEnabled = false, CalcValueType = item.Type });
                }
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
            var input1Start = calc.GetInputs()[Input1Index].ValueAsString;
            var input2Start = calc.GetInputs()[Input2Index].ValueAsString;
            int steps1 = Steps1;
            int steps2 = Steps2;
            if (Input1Type == CalcCore.CalcValueType.SELECTIONLIST)
            {
                steps1 = _start1_2 - _start1_1 + 1;
            }
            if (Input2Type == CalcCore.CalcValueType.SELECTIONLIST)
            {
                steps2 = _start2_2 - _start2_1 + 1;
            }
            for (int i = 0; i < steps1; i++)
            {
                ObservableCollection<TableResult> resultRow = new ObservableCollection<TableResult>();
                for (int j = 0; j < steps2; j++)
                {
                    if (Input1Type == CalcCore.CalcValueType.DOUBLE)
                    {
                        calc.GetInputs()[Input1Index].ValueAsString = (Start1 + Step1 * i).ToString();
                    }
                    else
                    {
                        calc.GetInputs()[Input1Index].ValueAsString = Start1Items[_start1_1 + i];
                    }
                    if (Input2Type == CalcCore.CalcValueType.DOUBLE)
                    {
                        calc.GetInputs()[Input2Index].ValueAsString = (Start2 + Step2 * j).ToString();
                    }
                    else
                    {
                        calc.GetInputs()[Input2Index].ValueAsString = Start2Items[_start2_1 + j];
                    }
                    calc.UpdateCalc();
                    if (calc.GetOutputs()[OutputIndex].Type == CalcCore.CalcValueType.DOUBLE)
                    {
                        resultRow.Add(new TableResult() {
                            Value = (calc.GetOutputs()[OutputIndex] as CalcCore.CalcDouble).Value.ToString("N2"),
                            Status= calc.GetOutputs()[OutputIndex].Status
                        });
                    }
                    else
                        resultRow.Add(new TableResult() {
                            Value = calc.GetOutputs()[OutputIndex].ValueAsString,
                            Status = calc.GetOutputs()[OutputIndex].Status
                        });
                }
                results.Add(resultRow);
            }
            Input1Headers = new ObservableCollection<string>();
            for (int i = 0; i < steps1; i++)
            {
                if (Input1Type == CalcCore.CalcValueType.DOUBLE)
                {
                    Input1Headers.Add((Start1 + Step1 * i).ToString());
                }
                else
                {
                    Input1Headers.Add(Start1Items[_start1_1 + i]);
                }
            }
            Input2Headers = new ObservableCollection<string>();
            for (int i = 0; i < steps2; i++)
            {
                if (Input2Type == CalcCore.CalcValueType.DOUBLE)
                {
                    Input2Headers.Add((Start2 + Step2 * i).ToString());
                }
                else
                {
                    Input2Headers.Add(Start2Items[_start2_1 + i]);
                }
            }

            calc.GetInputs()[Input1Index].ValueAsString = input1Start;
            calc.GetInputs()[Input2Index].ValueAsString = input2Start;
            calc.UpdateCalc();

            RaisePropertyChanged(nameof(Input1Headers));
            RaisePropertyChanged(nameof(Input2Headers));
            RaisePropertyChanged(nameof(Results));
            RaisePropertyChanged(nameof(Input1TextHeader));
            RaisePropertyChanged(nameof(Input2TextHeader));
            RaisePropertyChanged(nameof(Input1Type));
            RaisePropertyChanged(nameof(Start1Items));
            RaisePropertyChanged(nameof(Start1_1));
            RaisePropertyChanged(nameof(Start1_2));
            RaisePropertyChanged(nameof(Input2Type));
            RaisePropertyChanged(nameof(Start2Items));
            RaisePropertyChanged(nameof(Start2_1));
            RaisePropertyChanged(nameof(Start2_2));
        }
    }
}
