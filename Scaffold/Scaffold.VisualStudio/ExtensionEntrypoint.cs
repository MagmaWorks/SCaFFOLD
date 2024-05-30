using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

// Note: Add reference to Microsoft.VisualStudio.Shell.15.0 for better XAML editing. TODO REMOVE THIS PACKAGE BEFORE A RELEASE.
namespace Scaffold.VisualStudio
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "Scaffold.VisualStudio.d9ffba73-6bab-4d74-8863-d33f8ac837b5",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Publisher name",
                    displayName: "Scaffold.VisualStudio",
                    description: "Extension description"),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
        }
    }
}
