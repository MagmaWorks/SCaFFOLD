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

namespace Calcs
{
    public class CrossRefVM : ViewModelBase
    {
        private int _crossRefTotalItems = 0;

        public ObservableCollection<CrossRefItem> CrossRefItems { get; set; }

        public int CrossRefTotalItems { get => _crossRefTotalItems; set { _crossRefTotalItems = value; RaisePropertyChanged(nameof(CrossRefTotalItems)); } }

        ICalc calc;

        ICommand _test;

        public ICommand Test
        {
            get
            {
                return _test ?? (_test = new CommandHandler(() => runCrossRefCalcs(), true));
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
                CrossRefItems.Add(new CrossRefItem(calc, i));
            }
        }

        private void runCrossRefCalcs()
        {
            List<CrossRefItem> crossRefData = CrossRefItems.Where(a => a.IsSelected == true).ToList();

            int[] indices = new int[crossRefData.Count];

            bool complete = false;
            List<double>[] inputValues = new List<double>[crossRefData.Count];
            for (int i = 0; i < crossRefData.Count; i++)
            {
                double stepSize = (crossRefData[i].MaxVal - crossRefData[i].MinVal) / crossRefData[i].Steps;
                inputValues[i] = new List<double>();
                for (int j = 0; j < crossRefData[i].Steps + 1; j++)
                {
                    inputValues[i].Add(crossRefData[i].MinVal + stepSize * j);
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
            CrossRefTotalItems = totalCalcs;
            //this.Dispatcher.Invoke(() => { CrossRefTotalItems = totalCalcs; });
            totalCalcs = 0;

            List<string> outputValues = new List<string>();

            while (!complete)
            {
                string outputString = "";
                for (int i = 0; i < crossRefData.Count; i++)
                {
                    // add switch on input type in due course...
                        calc.GetInputs()[crossRefData[i].InputIndex].ValueAsString = inputValues[i][indices[i]].ToString();
                }
                calc.UpdateCalc();
                totalCalcs++;
                //this.Dispatcher.Invoke(() =>
                //{
                //    myModel.Progress = totalCalcs;
                //});

                outputString = calc.GetInputs()[0].ValueAsString;
                if (calc.GetInputs().Count > 1)
                {
                    for (int j = 1; j < calc.GetInputs().Count; j++)
                    {
                        outputString = outputString + ", " + calc.GetInputs()[j].ValueAsString;
                    }
                }
                for (int j = 0; j < calc.GetOutputs().Count; j++)
                {
                    outputString = outputString + ", " + calc.GetOutputs()[j].ValueAsString;
                }

                outputValues.Add(outputString);

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
                    if (indices[i] < crossRefData[i].Steps + 1)
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

            string headings = calc.GetInputs()[0].Name;
            if (calc.GetInputs().Count > 1)
            {
                for (int i = 1; i < calc.GetInputs().Count; i++)
                {
                    headings = headings + ", " + calc.GetInputs()[i].Name;
                }
            }
            for (int i = 0; i < calc.GetOutputs().Count; i++)
            {
                headings = headings + ", " + calc.GetOutputs()[i].Name;
            }


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C: \Users\Alex Baalham\Documents" + @"\test.csv"))
            {
                file.WriteLine(headings);
                foreach (var item in outputValues)
                {
                    file.WriteLine(item);
                }
            }

            //this.Dispatcher.Invoke(() => { myModel.EnableView = true; myModel.ProgressBarShowing = Visibility.Hidden; });
        }
    }
}
