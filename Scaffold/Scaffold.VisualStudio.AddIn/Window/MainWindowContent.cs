using System.Text;
using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.AddIn.Xaml;
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

    private static string TempImageBlock(XamlImageInfo info)
        => $"<TempImage StartIndex='{info.StartIndex}' EndIndex='{info.EndIndex}'>";

    public override async Task<string> GetXamlAsync(CancellationToken cancellationToken)
    {
        var xaml = await base.GetXamlAsync(cancellationToken);

        const string imagingNamespace = "xmlns:imaging=\"clr-namespace:Microsoft.VisualStudio.Imaging;assembly=Microsoft.VisualStudio.Imaging\"";
        const string catalogNamespace = "xmlns:catalog=\"clr-namespace:Microsoft.VisualStudio.Imaging;assembly=Microsoft.VisualStudio.ImageCatalog\"";

        xaml = AddNamespace(xaml, imagingNamespace);
        xaml = AddNamespace(xaml, catalogNamespace);

        var imageInfoList = new List<XamlImageInfo>();

        while (true)
        {
            if (xaml.Contains("<Image") == false)
                break;

            var info = new XamlImageInfo
            {
                StartIndex = xaml.IndexOf("<Image", StringComparison.Ordinal)
            };

            info.EndIndex = xaml.IndexOf("/>", info.StartIndex, StringComparison.Ordinal) + 2;

            imageInfoList.Add(info);
            info.ImageContent = xaml[info.StartIndex..info.EndIndex];

            var moniker = GetProperty(info.ImageContent, "Tag", true);
            if (moniker != null)
            {
                var potentialProperties = new List<string>
                {
                    GetProperty(info.ImageContent, "Width"),
                    GetProperty(info.ImageContent, "Height"),
                    GetProperty(info.ImageContent, "Grid.Row"),
                    GetProperty(info.ImageContent, "Grid.Column"),
                    GetProperty(info.ImageContent, "Orientation"),
                    GetProperty(info.ImageContent, "DockPanel.Dock"),
                    GetProperty(info.ImageContent, "VerticalAlignment"),
                    GetProperty(info.ImageContent, "HorizontalAlignment"),
                    GetProperty(info.ImageContent, "Visibility"),
                    GetProperty(info.ImageContent, "Margin")
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
                info.CrispImageContent = crispImage.ToString();
            }

            xaml = xaml.Replace(info.ImageContent, TempImageBlock(info));
        }

        foreach (var image in imageInfoList)
        {
            var replacement = image.CrispImageContent ?? image.ImageContent;
            xaml = xaml.Replace(TempImageBlock(image), replacement);
        }

        return xaml;
    }
}