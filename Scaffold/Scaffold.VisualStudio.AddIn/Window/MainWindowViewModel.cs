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
        private Visibility _inactiveWatcherVisibility = Visibility.Visible;
        private Visibility _waitingForTabVisibility = Visibility.Collapsed;
        private Visibility _hasActiveProjectVisibility = Visibility.Collapsed;
        
        private string _activeProjectPath;
        
        public MainWindowViewModel()
        {
            // HelloCommand = new AsyncCommand((parameter, clientContext, cancellationToken) =>
            // {
            //     Text = $"Hello {parameter as string}!";
            //     return Task.CompletedTask;
            // });
        }

        public Visibility InactiveWatcherVisibility
        {
            get => _inactiveWatcherVisibility;
            set => SetProperty(ref _inactiveWatcherVisibility, value);
        }
        
        public Visibility WaitingForTabVisibility
        {
            get => _waitingForTabVisibility;
            set => SetProperty(ref _waitingForTabVisibility, value);
        }

        public Visibility HasActiveProjectVisibility
        {
            get => _hasActiveProjectVisibility;
            set => SetProperty(ref _hasActiveProjectVisibility, value);
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
                
                InactiveWatcherVisibility = Visibility.Collapsed;
                WaitingForTabVisibility = Visibility.Visible;
            }
        }

        // [DataMember]
        // public AsyncCommand HelloCommand { get; }
    }
}
