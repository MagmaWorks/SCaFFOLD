using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Calcs
{
    public class AppViewModel : ViewModelBase
    {
        public List<CalcCore.CalcAssembly> Assemblies { get; set; }

        ObservableCollection<CalculationViewModel> _viewModels;
        public ObservableCollection<CalculationViewModel> ViewModels
        {
            get
            {
                return _viewModels;
            }
            set
            {
                _viewModels = value;
                RaisePropertyChanged(nameof(ViewModels));
            }
        }

        public CalculationViewModel ViewModel
        {
            get
            {
                return ViewModels[_selectedViewModel];
            }
        }

        int _selectedViewModel = 0;
        public int SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged(nameof(SelectedViewModel));
                RaisePropertyChanged(nameof(ViewModel));
            }
        }

        ICommand addCalcCommand;

        public ICommand AddCalcCommand
        {
            get
            {
                return addCalcCommand ?? (addCalcCommand = new CommandHandlerWithParameter(param => addCalc(param), true));
            }
        }

        private void addCalc(Type calcType)
        {
            CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(calcType);
            ViewModels.Add(new CalculationViewModel(calcInstance));
            SelectedViewModel = ViewModels.Count-1;
        }

        ICommand saveCalcCommand;

        public ICommand SaveCalcCommand
        {
            get
            {
                return saveCalcCommand ?? (saveCalcCommand = new CommandHandler(() => saveCalc(), true));
            }
        }

        private void saveCalc()
        {
            var saveObj = Newtonsoft.Json.JsonConvert.SerializeObject(ViewModel.Calc, Newtonsoft.Json.Formatting.Indented);
            string filePath = "";
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"Word files |*.JSON";
                saveDialog.FileName = ViewModel.Calc.InstanceName + @".JSON";
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveDialog.ShowDialog() != DialogResult.OK) return;
                filePath = saveDialog.FileName;
                System.IO.File.WriteAllText(filePath, saveObj);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
            }
        }

        ICommand openCalcCommand;

        public ICommand OpenCalcCommand
        {
            get
            {
                return openCalcCommand ?? (openCalcCommand = new CommandHandler(() => openCalc(), true));
            }
        }

        private void openCalc()
        {
            string filePath = "";
            try
            {
                var openDialog = new OpenFileDialog();
                openDialog.Filter = @"Calc files |*.JSON";
                openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (openDialog.ShowDialog() != DialogResult.OK) return;
                filePath = openDialog.FileName;
                string openObj = System.IO.File.ReadAllText(filePath);
                var deserialiseType = new { InstanceName = "", TypeName = "", ClassName = "", Inputs = new List<deserialiseCalcValue>()};
                var deserialiseObj = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(openObj, deserialiseType);

                var calcType = Assemblies.Where(a => a.Class.FullName == deserialiseObj.ClassName).First();
                CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(calcType.Class);
                foreach (var item in deserialiseObj.Inputs)
                {
                    calcInstance.Inputs.First(a => a.Name == item.Name).ValueAsString = item.ValueAsString;
                }
                calcInstance.InstanceName = deserialiseObj.InstanceName;
                ViewModels.Add(new CalculationViewModel(calcInstance));
                SelectedViewModel = ViewModels.Count - 1;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
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

        private void runBatchCalcs()
        {
            //var _calcs = new List<SimpleCalcVM>();
            var _calcNames = new ObservableCollection<string>();
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
                foreach (var calc in Assemblies)
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
                ViewModels.Add(new CalculationViewModel(newCalc));
            }
            SelectedViewModel = ViewModels.Count - 1;
        }

        private class deserialiseCalcValue
        {
            public string ValueAsString { get; set; } = "";
            public string Name { get; set; } = "";
        }

    }
}
