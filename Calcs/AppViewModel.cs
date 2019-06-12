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

        ObservableCollection<CalculationViewModel> _viewModels = new ObservableCollection<CalculationViewModel>();
        public ObservableCollection<CalculationViewModel> ViewModels
        {
            get
            {
                return _viewModels;
            }
            //set
            //{
            //    _viewModels = value;
            //    RaisePropertyChanged(nameof(ViewModels));
            //}
        }

        public CalculationViewModel ViewModel
        {
            get
            {
                if (_selectedViewModel < 0)
                {
                    _selectedViewModel = 0;
                }
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
                _selectedViewModel = Math.Max(0,value);
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

        ICommand removeCalcCommand;

        public ICommand RemoveCalcCommand
        {
            get
            {
                return removeCalcCommand ?? (removeCalcCommand = new CommandHandlerWithParameter(param => removeCalc(param), true));
            }
        }

        private void removeCalc(Type calcType)
        {
            if (ViewModels.Count>1)
            {
                int removeIndex = SelectedViewModel;
                SelectedViewModel = Math.Min(removeIndex, ViewModels.Count-2);
                _viewModels.RemoveAt(removeIndex);
                RaisePropertyChanged(nameof(ViewModels));
                SelectedViewModel=SelectedViewModel;
            }
            else
            {
                System.Windows.MessageBox.Show("You need at least one calculation");
            }
        }

        ICommand saveAllCommand;

        public ICommand SaveAllCommand
        {
            get
            {
                return saveAllCommand ?? (saveAllCommand = new CommandHandler(() => saveAll(), true));
            }
        }

        public void SaveAllOnClose(object sender, EventArgs e)
        {
            var vm = new SaveOnQuitVM();
            Window1 myWin = new Window1(vm);
            myWin.ShowDialog();
            if (vm.Save)
            {
                saveAll();
            }
        }

        public void saveAll()
        {
            for (int i = 0; i < ViewModels.Count; i++)
            {
                var vm = ViewModels[i];
                var saveObj = Newtonsoft.Json.JsonConvert.SerializeObject(ViewModel.Calc, Newtonsoft.Json.Formatting.Indented);
                string filePath = "";
                try
                {
                    var saveDialog = new SaveFileDialog();
                    saveDialog.Filter = @"JSON files |*.JSON";
                    if (vm.Filepath == "")
                    {
                        SelectedViewModel = i;
                        System.Windows.MessageBox.Show("Your calculation " + vm.Calc.InstanceName + " has not been saved yet. You have the option to save it now.");
                        saveDialog.FileName = vm.Calc.InstanceName + @".JSON";
                        saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            filePath = saveDialog.FileName;
                            System.IO.File.WriteAllText(filePath, saveObj);
                            vm.Filepath = filePath;
                        }
                    }
                    else
                    {
                        filePath = vm.Filepath;
                        System.IO.File.WriteAllText(filePath, saveObj);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                    return;
                }
            }
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
                saveDialog.Filter = @"JSON files |*.JSON";
                if (ViewModel.Filepath == "")
                {
                    saveDialog.FileName = ViewModel.Calc.InstanceName + @".JSON";
                }
                else
                {
                    saveDialog.FileName = ViewModel.Filepath;
                }
                
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveDialog.ShowDialog() != DialogResult.OK) return;
                filePath = saveDialog.FileName;
                System.IO.File.WriteAllText(filePath, saveObj);
                ViewModel.Filepath = filePath;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
            }
        }

        ICommand copyCalcCommand;

        public ICommand CopyCalcCommand
        {
            get
            {
                return copyCalcCommand ?? (copyCalcCommand = new CommandHandler(() => copyCalc(), true));
            }
        }

        private void copyCalc()
        {
            CalcCore.ICalc newCalc = (CalcCore.ICalc)Activator.CreateInstance(ViewModel.Calc.GetType());
            var oldInputs = ViewModel.Calc.GetInputs();
            var newInputs = newCalc.GetInputs();
            for (int i = 0; i < oldInputs.Count; i++)
            {
                newInputs[i].ValueAsString = oldInputs[i].ValueAsString;
            }

            bool uniqueName = false;
            var proposedName = ViewModel.Calc.InstanceName + "_copy";
            while (!uniqueName)
            {
                if (ViewModels.Where(a => a.CalcInstanceName == proposedName).ToList().Count > 0)
                {
                    proposedName += "_copy";
                }
                else
                {
                    uniqueName = true;
                }
            }
            newCalc.InstanceName = proposedName;


            _viewModels.Add(new CalculationViewModel(newCalc));
            RaisePropertyChanged(nameof(ViewModels));
            SelectedViewModel = _viewModels.Count - 1;
        }

        ICommand megaSaveCalcCommand;

        public ICommand MegaSaveCalcCommand
        {
            get
            {
                return megaSaveCalcCommand ?? (megaSaveCalcCommand = new CommandHandler(() => megaSaveCalc(), true));
            }
        }

        private void megaSaveCalc()
        {
            var saveObj = Newtonsoft.Json.JsonConvert.SerializeObject(ViewModel.Calc, Newtonsoft.Json.Formatting.Indented);
            string filePath = "";
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"JSON files |*.JSON";
                if (ViewModel.Filepath == "")
                {
                    saveDialog.FileName = ViewModel.Calc.InstanceName + @".JSON";
                }
                else
                {
                    saveDialog.FileName = ViewModel.Filepath;
                }
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveDialog.ShowDialog() != DialogResult.OK) return;
                filePath = saveDialog.FileName;
                System.IO.File.WriteAllText(filePath, saveObj);
                ViewModel.Filepath = filePath;
                var drawings = ViewModel.Calc.GetDrawings();
                for (int i = 0; i < drawings.Count; i++)
                {
                    var dxfDrawing = drawings[i];
                    string filePathDxf = Path.GetFileNameWithoutExtension(filePath);
                    filePathDxf = Path.GetDirectoryName(filePath) + @"\" + filePathDxf;
                    filePathDxf += @" drawing" + (i + 1).ToString("D3") + ".dxf";
                    dxfDrawing.Save(filePathDxf);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
            }
        }

        ICommand saveCalcDxfCommand;

        public ICommand SaveCalcDxfCommand
        {
            get
            {
                return saveCalcDxfCommand ?? (saveCalcDxfCommand = new CommandHandler(() => saveCalcDxf(), true));
            }
        }

        private void saveCalcDxf()
        {
            string filePath = "";
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"DXF files | *.dxf";
                saveDialog.FileName = ViewModel.Calc.InstanceName + @".dxf";
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveDialog.ShowDialog() != DialogResult.OK) return;
                filePath = saveDialog.FileName;
                var drawings = ViewModel.Calc.GetDrawings();
                for (int i = 0; i < drawings.Count; i++)
                {
                    var dxfDrawing = drawings[i];
                    string filePathDxf = Path.GetFileNameWithoutExtension(filePath);
                    filePathDxf = Path.GetDirectoryName(filePath) + @"\" + filePathDxf;
                    filePathDxf += @" drawing" + (i + 1).ToString("D3") + ".dxf";
                    dxfDrawing.Save(filePathDxf);
                }
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
                bool inputMissing = false;
                string inputMissingMessage = "";
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
                    // Need to find a tidy way to do this...
                    var input = calcInstance.Inputs.Find(a => a.Name == item.Name);
                    if (input != null)
                    {
                        input.ValueAsString = item.ValueAsString;
                    }
                    else
                    {
                        inputMissing = true;
                        inputMissingMessage += "The input \"" + item.Name + "\" in file does not exist in calc" + Environment.NewLine;

                    }

                }
                calcInstance.InstanceName = deserialiseObj.InstanceName;
                var newCalcVM = new CalculationViewModel(calcInstance);
                newCalcVM.Filepath = filePath;
                ViewModels.Add(newCalcVM);
                SelectedViewModel = ViewModels.Count - 1;
                if (inputMissing)
                {
                    System.Windows.MessageBox.Show(inputMissingMessage);
                }

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
                    if (input != null)
                    {
                        input.ValueAsString = lineItems[i];
                    }
                }
                number++;
                newCalc.UpdateCalc();
                ViewModels.Add(new CalculationViewModel(newCalc));
            }
            SelectedViewModel = ViewModels.Count - 1;
        }

        public AppViewModel(List<CalcCore.CalcAssembly> calcs)
        {
            Assemblies = calcs;
            CalcCore.ICalc calcInstance = (CalcCore.ICalc)Activator.CreateInstance(Assemblies[0].Class);
            ViewModels.Add(new CalculationViewModel(calcInstance));
        }

        private class deserialiseCalcValue
        {
            public string ValueAsString { get; set; } = "";
            public string Name { get; set; } = "";
        }

    }
}
