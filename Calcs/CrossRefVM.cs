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
    public class CrossRefVM : ViewModelBase, ICalcViewParent
    {
        private int _crossRefTotalItems = 0;

        private Thread backgroundCalcs;
        private int totalCalcs;
        private int calculations;

        private DataTable _crossRefOutput = new DataTable();
        public DataView CrossRefOutput
        {
            get
            {
                return _crossRefOutput.DefaultView;
            }
        }

        private double calcTime;

        double _progress = 0;
        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                RaisePropertyChanged(nameof(Progress));
            }
        }

        private bool stopBackgroundCalcs = false;

        private bool _showProgressBar = false;
        public Visibility ProgressVisibility
        {
            get
            {
                if (_showProgressBar)
                {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
        }

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


        public ObservableCollection<CrossRefItem> CrossRefItems { get; set; }

        public int CrossRefTotalItems { get => _crossRefTotalItems; set { _crossRefTotalItems = value; RaisePropertyChanged(nameof(CrossRefTotalItems)); RaisePropertyChanged(nameof(TotalCalcTime)); } }
        public double TotalCalcTime
        { get
            {
                return _crossRefTotalItems * calcTime;
            } }

        ICalc calc;

        List<IOValues> inputs;
        public List<IOValues> Inputs
        {
            get
            {
                return inputs;
            }
            set
            {
                inputs = value;
            }
        }

        ICommand _test;

        public ICommand Test
        {
            get
            {
                return _test ?? (_test = new CommandHandler(() => runBackgroundCalcs(), true));
            }
        }

        void runBackgroundCalcs()
        {
            if (backgroundCalcs.IsAlive)
            {
                stopBackgroundCalcs = true;
                backgroundCalcs.Join();
            }
            stopBackgroundCalcs = false;
            backgroundCalcs = new Thread(new ThreadStart(createDataTable));
            backgroundCalcs.Start();

        }

        ICommand _save;

        public ICommand Save
        {
            get
            {
                return _save ?? (_save = new CommandHandler(() => runCrossRefCalcs(), true));
            }
        }

        public void UpdateOutputs()
        {

        }

        public CrossRefVM(ICalc calc)
        {
            Inputs = new List<IOValues>();
            foreach (var item in calc.GetInputs())
            {
                Inputs.Add(new IOValues(item, calc, this));
            }

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
            backgroundCalcs = new Thread(new ThreadStart(createDataTable));
            _crossRefOutput = new DataTable();
            _crossRefOutput.Columns.Add("No results calculated yet.");
            RaisePropertyChanged(nameof(CrossRefOutput));
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
            _showProgressBar = true; RaisePropertyChanged(nameof(ProgressVisibility));
            List<CrossRefItem> crossRefData = new List<CrossRefItem>();
            crossRefData = CrossRefItems.Where(a => a.IsSelected == true).ToList();
            var crossRefOutput = new DataTable();
            if (calc.GetInputs().Count > 1)
            {
                for (int i = 0; i < calc.GetInputs().Count; i++)
                {
                    crossRefOutput.Columns.Add("I"+i.ToString()+" "+ calc.GetInputs()[i].Name, typeof(string));
                }
            }
            for (int i = 0; i < calc.GetOutputs().Count; i++)
            {
                crossRefOutput.Columns.Add("O"+i.ToString() + " " + calc.GetOutputs()[i].Name, typeof(string));
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
            totalCalcs = 1;
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

            List<string> outputValues = new List<string>();
            calculations = 0;
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Interval = 250;
            myTimer.Elapsed += progressBarUpdate;
            myTimer.Start();

            while (!complete)
            {

                if (stopBackgroundCalcs)
                {
                    Application.Current.Dispatcher.InvokeAsync(() => _showProgressBar = false);
                    Application.Current.Dispatcher.InvokeAsync(() => RaisePropertyChanged(nameof(ProgressVisibility)));
                    return;
                }
                string outputString = "";
                var outputStrings = new ObservableCollection<string>();
                for (int i = 0; i < crossRefData.Count; i++)
                {
                    calc.GetInputs()[crossRefData[i].InputIndex].ValueAsString = inputValues[i][indices[i]].ToString();
                }
                calc.UpdateCalc();

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
                var myRow = crossRefOutput.NewRow();
                for (int i = 0; i < outputStrings.Count; i++)
                {
                    myRow[i] = outputStrings[i];
                }
                crossRefOutput.Rows.Add(myRow);

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
                calculations++;
            }

            Application.Current.Dispatcher.InvokeAsync(() => _crossRefOutput = crossRefOutput);
            Application.Current.Dispatcher.InvokeAsync(() => RaisePropertyChanged(nameof(CrossRefOutput)));
            Application.Current.Dispatcher.InvokeAsync(() => _showProgressBar = false);
            Application.Current.Dispatcher.InvokeAsync(() => RaisePropertyChanged(nameof(ProgressVisibility)));
        }

        private void progressBarUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => Progress = 100 * calculations / totalCalcs);
        }
    }
}
