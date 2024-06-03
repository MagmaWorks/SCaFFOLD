using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using System.Windows;
using Scaffold.VisualStudio.Models;
//
// Notes
// WPF converters are unfortunately not applying on the Extensibility project from XAML, so I've added them to code behind.
//
namespace Scaffold.VisualStudio.AddIn.Window
{
    [DataContract]
    internal class MainWindowViewModel : NotifyPropertyChangedObject
    {
        private Visibility _onLoadVisibility = Visibility.Visible;
        private Visibility _waitingForTabVisibility = Visibility.Visible;
        private Visibility _hasActiveProjectVisibility = Visibility.Collapsed;
        private Visibility _isWatchingVisibility = Visibility.Collapsed;
        private string _activeProjectPath;
        
        public MainWindowViewModel()
        {
            // HelloCommand = new AsyncCommand((parameter, clientContext, cancellationToken) =>
            // {
            //     Text = $"Hello {parameter as string}!";
            //     return Task.CompletedTask;
            // });
            var wait = "";
        }

        [DataMember]
        public Visibility OnLoadVisibility
        {
            get => _onLoadVisibility;
            set => SetProperty(ref _onLoadVisibility, value);
        }

        [DataMember]
        public Visibility WaitingForTabVisibility
        {
            get => _waitingForTabVisibility;
            set => SetProperty(ref _waitingForTabVisibility, value);
        }

        [DataMember]
        public Visibility HasActiveProjectVisibility
        {
            get => _hasActiveProjectVisibility;
            set => SetProperty(ref _hasActiveProjectVisibility, value);
        }

        [DataMember]
        public Visibility IsWatchingVisibility
        {
            get => _isWatchingVisibility;
            set => SetProperty(ref _isWatchingVisibility, value);
        }
        
        [DataMember]
        public List<TreeItem> Watching { get; }

        [DataMember]
        public string ActiveProjectPath
        {
            get => _activeProjectPath;
            set
            {
                SetProperty(ref _activeProjectPath, value);
                HasActiveProjectVisibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        // [DataMember]
        // public AsyncCommand HelloCommand { get; }
    }
}
