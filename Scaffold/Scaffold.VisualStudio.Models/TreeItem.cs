using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Scaffold.VisualStudio.Models
{
    public class TreeItem : INotifyPropertyChanged
    {
        private bool _isExpanded;

        public bool HasIcon { get; set; }
        public bool Success => Result is { Failure: null };
        public string Name { get; set; }
        public CalculationResult Result { get; set; }

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
