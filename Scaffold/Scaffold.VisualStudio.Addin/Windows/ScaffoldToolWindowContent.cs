using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.AddIn.Windows
{
    /// <summary>
    /// A remote user control to use as tool window UI content.
    /// </summary>
    internal class ScaffoldToolWindowContent : RemoteUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldToolWindowContent" /> class.
        /// </summary>
        public ScaffoldToolWindowContent()
            : base(dataContext: new ScaffoldToolWindowVm())
        {
        }
    }
}
