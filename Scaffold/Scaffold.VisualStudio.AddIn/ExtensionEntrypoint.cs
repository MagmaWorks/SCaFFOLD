using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace Scaffold.VisualStudio.AddIn;

[VisualStudioContribution]
internal class ExtensionEntrypoint : Extension
{
    /// <inheritdoc/>
    public override ExtensionConfiguration ExtensionConfiguration => new()
    {
        Metadata = new(
            id: "Scaffold.VisualStudio.AddIn.741b8077-d032-40d6-89f5-d6a46e618877",
            version: ExtensionAssemblyVersion,
            publisherName: "Magma Works",
            displayName: "Scaffold.VisualStudio.AddIn",
            description: "SCaFFOLD assistant plugin for rapid development of calculations, providing instant dotnet watch style feedback to your calculations."),
    };
        
    protected override void InitializeServices(IServiceCollection serviceCollection)
    {
        base.InitializeServices(serviceCollection);

        // You can configure dependency injection here by adding services to the serviceCollection.
    }
}