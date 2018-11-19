using CalcCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Data;

namespace Calcs
{
    public class CrossRefVM : ViewModelBase
    {
        private int _crossRefTotalItems = 0;

        private DataTable _crossRefOutput = new DataTable();
        public DataView CrossRefOutput
        {
            get
            {
                return _crossRefOutput.DefaultView;
            }
        }

        private double calcTime;

        private string _filePath = Environment.CurrentDirectory + @"\Output.csv";
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        private List<CrossRefItem> crossRefData = new List<CrossRefItem>();

        public ObservableCollection<CrossRefItem> CrossRefItems { get; set; }

        public int CrossRefTotalItems { get => _crossRefTotalItems; set { _crossRefTotalItems = value; RaisePropertyChanged(nameof(CrossRefTotalItems)); RaisePropertyChanged(nameof(TotalCalcTime)); } }
        public double TotalCalcTime
        { get
            {
                return _crossRefTotalItems * calcTime;
            } }

        ICalc calc;

        ICommand _test;

        public ICommand Test
        {
            get
            {
                return _test ?? (_test = new CommandHandler(() => createDataTable(), true));
            }
        }

        ICommand _save;

        public ICommand Save
        {
            get
            {
                return _save ?? (_save = new CommandHandler(() => runCrossRefCalcs(), true));
            }
        }

        public CrossRefVM(ICalc calc)
        {
            CrossRefItems = new ObservableCollection<CrossRefItem>();
            var inputs = calc.GetInputs();
            this.calc = calc;
            for (int i = 0; i < inputs.Count; i++)
            {
                // add check on input type here in due course...
                CrossRefItems.Add(new CrossRefItem(calc, i, this));
            }
            Stopwatch myTimer = new Stopwatch();
            myTimer.Start();
            this.calc.UpdateCalc();
            calcTime = myTimer.Elapsed.TotalSeconds;
            myTimer.Stop();
        }

        public void calcCrossRefInputs()
        {
            int totalCalcs = 1;
            for (int i = 0; i < CrossRefItems.Count; i++)
            {
                if (CrossRefItems[i].IsSelected)
                {
                    if (CrossRefItems[i].CalcType == CalcValueType.DOUBLE)
                    {
                        totalCalcs = totalCalcs * (CrossRefItems[i].Steps + 1);
                    }
                    else if (CrossRefItems[i].CalcType == CalcValueType.SELECTIONLIST)
                    {
                        totalCalcs = totalCalcs * (CrossRefItems[i].EndIndex - CrossRefItems[i].StartIndex + 1);
                    }
                }
            }
            CrossRefTotalItems = totalCalcs;

        }

        private void runCrossRefCalcs()
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "csv";
            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.ShowDialog();
            var fileName = dialog.FileName;

            string headings = _crossRefOutput.Columns[0].ColumnName;
            if (_crossRefOutput.Columns.Count > 1)
            {
                for (int i = 1; i < _crossRefOutput.Columns.Count; i++)
                {
                    headings += "," + _crossRefOutput.Columns[i].ColumnName;
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine(headings);
                foreach (DataRow row in _crossRefOutput.Rows)
                {
                    string temp = (string)row[0];
                    if (row.ItemArray.Length > 1)
                    {
                        for (int i = 1; i < row.ItemArray.Length; i++)
                        {
                            temp += "," + row.ItemArray[i];
                        }
                    }
                    file.WriteLine(temp);
                }
            }
        }

        private void createDataTable()
        {
            this.crossRefData.Clear();
            this.crossRefData = CrossRefItems.Where(a => a.IsSelected == true).ToList();

            _crossRefOutput = new DataTable();
            if (calc.GetInputs().Count > 1)
            {
                for (int i = 0; i < calc.GetInputs().Count; i++)
                {
                    _crossRefOutput.Columns.Add("I"+i.ToString()+" "+ calc.GetInputs()[i].Name, typeof(string));
                }
            }
            for (int i = 0; i < calc.GetOutputs().Count; i++)
            {
                _crossRefOutput.Columns.Add("O"+i.ToString() + " " + calc.GetOutputs()[i].Name, typeof(string));
            }
            

            int[] indices = new int[crossRefData.Count];

            bool complete = false;
            List<string>[] inputValues = new List<string>[crossRefData.Count];
            for (int i = 0; i < crossRefData.Count; i++)
            {
                if (crossRefData[i].CalcType == CalcValueType.DOUBLE)
                {
                    double stepSize = (crossRefData[i].MaxVal - crossRefData[i].MinVal) / crossRefData[i].Steps;
                    inputValues[i] = new List<string>();
                    for (int j = 0; j < crossRefData[i].Steps + 1; j++)
                    {
                        inputValues[i].Add((crossRefData[i].MinVal + stepSize * j).ToString());
                    }
                }
                else if (crossRefData[i].CalcType == CalcValueType.SELECTIONLIST)
                {
                    inputValues[i] = new List<string>();
                    for (int j = 0; j < crossRefData[i].EndIndex - crossRefData[i].StartIndex + 1; j++)
                    {
                        inputValues[i].Add(crossRefData[i].SelectionList[j + crossRefData[i].StartIndex]);
                    }
                }

            }
            int totalCalcs = 1;
            for (int i = 0; i < CrossRefItems.Count; i++)
            {
                if (CrossRefItems[i].IsSelected)
                {
                    totalCalcs = totalCalcs * (CrossRefItems[i].Steps + 1);
                }
            }

            List<string> outputValues = new List<string>();

            while (!complete)
            {
                string outputString = "";
                var outputStrings = new ObservableCollection<string>();
                for (int i = 0; i < crossRefData.Count; i++)
                {
                    calc.GetInputs()[crossRefData[i].InputIndex].ValueAsString = inputValues[i][indices[i]].ToString();
                }
                calc.UpdateCalc();

                //this.Dispatcher.Invoke(() =>
                //{
                //    myModel.Progress = totalCalcs;
                //});

                outputStrings.Add(calc.GetInputs()[0].ValueAsString);
                if (calc.GetInputs().Count > 1)
                {
                    for (int j = 1; j < calc.GetInputs().Count; j++)
                    {
                        outputStrings.Add(calc.GetInputs()[j].ValueAsString);
                    }
                }
                for (int j = 0; j < calc.GetOutputs().Count; j++)
                {
                    outputStrings.Add(calc.GetOutputs()[j].ValueAsString);
                }

                outputValues.Add(outputString);
                var myRow = _crossRefOutput.NewRow();
                for (int i = 0; i < outputStrings.Count; i++)
                {
                    myRow[i] = outputStrings[i];
                }
                _crossRefOutput.Rows.Add(myRow);

                //string[] tempStrings = new string[myModel.CrossRefData.Count];
                //for (int i = 0; i < myModel.CrossRefData.Count; i++)
                //{
                //    tempStrings[i] = inputValues[i][indices[i]].ToString();
                //}
                //outputValues.Add(tempStrings);

                for (int i = 0; i < indices.Length; i++)
                {
                    complete = true;
                    indices[i]++;
                    if (indices[i] < inputValues[i].Count)
                    {
                        complete = false;
                        break;
                    }
                    else
                    {
                        indices[i] = 0;
                    }
                }
            }

            //string displayString = "";
            //foreach (var item in outputValues)
            //{
            //    displayString = displayString + "[";
            //    foreach (var item2 in item)
            //    {
            //        displayString = displayString + item2  + ", ";
            //    }
            //    displayString = displayString + "]; ";
            //}
            //MessageBox.Show(displayString);
            RaisePropertyChanged(nameof(CrossRefOutput));
            //this.Dispatcher.Invoke(() => { myModel.EnableView = true; myModel.ProgressBarShowing = Visibility.Hidden; });
        }

    }
}
