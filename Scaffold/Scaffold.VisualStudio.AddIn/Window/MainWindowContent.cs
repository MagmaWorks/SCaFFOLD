using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.AddIn.Window;

internal class MainWindowContent : RemoteUserControl
{
    public MainWindowContent()
        : base(dataContext: new MainWindowViewModel())
    {
    }
}