using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.Models.Xaml;

namespace Scaffold.VisualStudio.AddIn.Window;

internal class MainWindowContent() : RemoteUserControl(dataContext: new MainWindowViewModel());