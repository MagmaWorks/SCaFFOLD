using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Scaffold.VisualStudio.Models
{
    public class TreeItem : INotifyPropertyChanged
    {
        private Visibility _isSuccessVisibility;
        private Visibility _isFailedVisibility;
        private bool _isExpanded;

        public TreeItem(CalculationResult result)
        {
            Result = result;
            IsSuccessVisibility = result.IsSuccess ? Visibility.Visible : Visibility.Collapsed;
            IsFailedVisibility = result.IsSuccess ? Visibility.Collapsed : Visibility.Visible;
        }
        
        public bool HasIcon { get; set; }
        public string Name { get; set; }
        public CalculationResult Result { get; }

        public Visibility IsSuccessVisibility
        {
            get => _isSuccessVisibility;
            set => SetField(ref _isSuccessVisibility, value);
        }
        
        public Visibility IsFailedVisibility
        {
            get => _isFailedVisibility;
            set => SetField(ref _isFailedVisibility, value);
        }
        
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetField(ref _isExpanded, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
