using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;

namespace Scaffold.VisualStudio.AddIn.Window
{
    /// <summary>
    /// ViewModel for the MainWindowContent remote user control.
    /// </summary>
    [DataContract]
    internal class MainWindowViewModel : NotifyPropertyChangedObject
    {
        public MainWindowViewModel()
        {
            HelloCommand = new AsyncCommand((parameter, clientContext, cancellationToken) =>
            {
                Text = $"Hello {parameter as string}!";
                return Task.CompletedTask;
            });
        }

        private string _name = string.Empty;
        [DataMember]
        public string Name
        {
            get => _name;
            set => SetProperty(ref this._name, value);
        }

        private string _text = string.Empty;
        [DataMember]
        public string Text
        {
            get => _text;
            set => SetProperty(ref this._text, value);
        }

        [DataMember]
        public AsyncCommand HelloCommand { get; }
    }
}
