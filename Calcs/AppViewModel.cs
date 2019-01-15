using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class AppViewModel : ViewModelBase
    {
        List<CalculationViewModel> _viewModels;
        public List<CalculationViewModel> ViewModels
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

        BatchVM _batcher;
        public BatchVM Batcher
        {
            get
            {
                return _batcher;
            }
            set
            {
                _batcher = value;
                RaisePropertyChanged(nameof(Batcher));
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
    }
}
