using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.Windows
{
    /// <summary>
    /// A remote user control to use as tool window UI content.
    /// </summary>
    internal class ScaffoldToolWindowContent : RemoteUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldToolWindowContent" /> class.
        /// </summary>
        public ScaffoldToolWindowContent(VisualStudioExtensibility extensibility)
            : base(dataContext: new ScaffoldToolWindowVm(extensibility))
        {
        }
    }
}
