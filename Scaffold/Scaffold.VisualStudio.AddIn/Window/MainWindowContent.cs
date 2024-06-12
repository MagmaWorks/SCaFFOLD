using System.Text;
using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.Models.Xaml;

namespace Scaffold.VisualStudio.AddIn.Window;

internal class MainWindowContent : RemoteUserControl
{
    public MainWindowContent() : base(dataContext: new MainWindowViewModel()) { }

    private string GetProperty(string xaml, string propertyName, bool returnValueOnly = false)
    {
        var start = xaml.IndexOf(propertyName, StringComparison.Ordinal);
        if (start == -1)
            return null;

        var substring = xaml[start..];

        var propertyValue = new StringBuilder();
        var hitFirstQuote = false;
        foreach (var charVal in substring)
        {
            if (charVal == '"')
            {
                if (hitFirstQuote)
                    break; // 2nd quote hit

                hitFirstQuote = true;
                continue;
            }

            if (hitFirstQuote)
                propertyValue.Append(charVal);
        }
        
        return returnValueOnly ? propertyValue.ToString() : $"{propertyName}=\"{propertyValue}\"";
    }

    private string AddNamespace(string xaml, string xamlNamespace)
    {
        if (xaml.Contains(xamlNamespace))
            return xaml;
        
        var dtHeadStart = xaml.IndexOf("<DataTemplate", StringComparison.Ordinal);
        var dtHeadEnd = xaml.IndexOf('>', dtHeadStart);

        var originalDataTemplateXaml = xaml[dtHeadStart..dtHeadEnd];
        var dataTemplateXaml = new StringBuilder();

        dataTemplateXaml
            .Append(xaml.AsSpan(dtHeadStart, dtHeadEnd - dtHeadStart))
            .Append($" {xamlNamespace}");

        return xaml.Replace(originalDataTemplateXaml, dataTemplateXaml.ToString());
    }

    public override async Task<string> GetXamlAsync(CancellationToken cancellationToken)
    {
        var xaml = await base.GetXamlAsync(cancellationToken);

        const string imagingNamespace = "xmlns:imaging=\"clr-namespace:Microsoft.VisualStudio.Imaging;assembly=Microsoft.VisualStudio.Imaging\"";
        const string catalogNamespace = "xmlns:catalog=\"clr-namespace:Microsoft.VisualStudio.Imaging;assembly=Microsoft.VisualStudio.ImageCatalog\"";

        xaml = AddNamespace(xaml, imagingNamespace);
        xaml = AddNamespace(xaml, catalogNamespace);

        while (true)
        {
            if (xaml.Contains("<Image") == false)
                break;

            var start = xaml.IndexOf("<Image", StringComparison.Ordinal);
            var end = xaml.IndexOf("/>", start, StringComparison.Ordinal) + 2;

            var imageXaml = xaml[start..end];
            var moniker = GetProperty(imageXaml, "Tag", true);
            if (moniker == null)
                throw new ArgumentException("Tag property must exist on the image containing the KnownMoniker for CrispImage.");

            var potentialProperties = new List<string>
            {
                GetProperty(imageXaml, "Width"),
                GetProperty(imageXaml, "Height"),
                GetProperty(imageXaml, "Grid.Row"),
                GetProperty(imageXaml, "Grid.Column"),
                GetProperty(imageXaml, "Orientation"),
                GetProperty(imageXaml, "DockPanel.Dock"),
                GetProperty(imageXaml, "VerticalAlignment"),
                GetProperty(imageXaml, "HorizontalAlignment"),
                GetProperty(imageXaml, "Visibility"),
                GetProperty(imageXaml, "Margin")
            };

            var crispImage = new StringBuilder();
            crispImage
                .Append("<imaging:CrispImage")
                .Append($" Moniker=\"{{x:Static catalog:{moniker}}}\"");

            foreach (var property in potentialProperties)
            {
                if (property == null)
                    continue;

                crispImage.Append($" {property}");
            }

            crispImage.Append(" />");
            xaml = xaml.Replace(imageXaml, crispImage.ToString());
        }

        xaml = xaml.Replace("Click=\"TreeItem_Clicked\"", "");
        return xaml;
    }
}