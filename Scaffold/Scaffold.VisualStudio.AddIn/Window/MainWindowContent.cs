using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.AddIn.Window
{
    /// <summary>
    /// A remote user control to use as tool window UI content.
    /// </summary>
    internal class MainWindowContent : RemoteUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowContent" /> class.
        /// </summary>
        public MainWindowContent()
            : base(dataContext: new MainWindowData())
        {
        }
    }
}
