using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Calcs
{
    public class BatchVM : ViewModelBase
    {
        List<SimpleCalcVM> _calcs;
        public List<SimpleCalcVM> Calcs
        {
            get
            {
                return _calcs;
            }
            set
            {
                _calcs = value;
                RaisePropertyChanged(nameof(Calcs));
            }
        }

        ObservableCollection<string> _calcNames;
        public ObservableCollection<string> CalcNames
        {
            get
            {
                return _calcNames;
            }
            set
            {
                _calcNames = value;
                RaisePropertyChanged(nameof(CalcNames));
            }
        }

        int _selectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
                }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged(nameof(SelectedIndex));
                RaisePropertyChanged(nameof(SelectedCalc));
            }
        }

        public SimpleCalcVM SelectedCalc
        {
            get
            {
                return _calcs[Math.Max(_selectedIndex, 0)];
            }
        }

        ICommand _processBatchCommand;

        public ICommand ProcessBatchCommand
        {
            get
            {
                return _processBatchCommand ?? (_processBatchCommand = new CommandHandler(() => runBatchCalcs(), true));
            }
        }

        List<CalcCore.CalcAssembly> _calcAssemblies;

        public BatchVM(List<CalcCore.CalcAssembly> calcAssemblies)
        {
            _calcs = new List<SimpleCalcVM> { new SimpleCalcVM(new TestCalcs.RC_Beam())};
            _calcNames = new ObservableCollection<string> { "Calc 1" };
            this._calcAssemblies = calcAssemblies;
        }

        private void runBatchCalcs()
        {
            _calcs = new List<SimpleCalcVM>();
            _calcNames = new ObservableCollection<string>();
            var fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.ShowDialog();
            var filePath = fileDialog.FileName;

            List<CalcCore.ICalc> returnList = new List<CalcCore.ICalc>();
            string line = "";
            string[] lineItems;
            var fs = File.OpenRead(filePath);
            var reader = new StreamReader(fs);
            var headerLine = reader.ReadLine();//assumes first row is headers
            var listHeaders = headerLine.Split(',');
            int number = 0;
            while (!reader.EndOfStream)
            {
                CalcCore.ICalc newCalc = new TestCalcs.PunchingShear();
                line = reader.ReadLine();
                lineItems = line.Split(',');
                foreach (var calc in _calcAssemblies)
                {
                    if (calc.Class.ToString() == lineItems[0]) newCalc = (CalcCore.ICalc)Activator.CreateInstance(calc.Class);
                }
                newCalc.InstanceName = lineItems[1];
                var inputs = newCalc.GetInputs();
                for (int i = 2; i < lineItems.Count(); i++)
                {
                    var input = inputs.Find(a => a.Name == listHeaders[i]);
                    input.ValueAsString = lineItems[i];
                }
                number++;
                newCalc.UpdateCalc();
                _calcs.Add(new SimpleCalcVM(newCalc));
                _calcNames.Add(newCalc.InstanceName);
            }
            RaisePropertyChanged(nameof(Calcs));
            RaisePropertyChanged(nameof(CalcNames));
            SelectedIndex = 0;
            SelectedCalc.UpdateOutputs();
            
        }
    }
}
