﻿using System.Reflection;
using Scaffold.Core.Images.Interfaces;
using Scaffold.Core.Interfaces;
using SkiaSharp;

namespace Scaffold.Core.Images.Models;

public class ImageFromEmbeddedResource<T> : ICalcImage
    where T : ICalculation
{
    private string EmbeddedResourceName { get; }

    /// <summary>
    /// Gets an image that is marked as an embedded resource in the project with calculations.
    /// </summary>
    /// <param name="embeddedResourceName">The name of the embedded resource (and its file extension).</param>
    public ImageFromEmbeddedResource(string embeddedResourceName)
        => EmbeddedResourceName = embeddedResourceName;

    public SKBitmap GetImage()
    {
        var embeddedResourceAssembly = Assembly.GetAssembly(typeof(T));
        if (embeddedResourceAssembly == null)
            throw new Exception("Assembly not found.");

        var manifestResources = embeddedResourceAssembly.GetManifestResourceNames();
        var existing = manifestResources.FirstOrDefault(x => x.EndsWith(EmbeddedResourceName));

        if (existing == null)
            throw new Exception($"Embedded resource '{EmbeddedResourceName}' not found.");

        using var stream = embeddedResourceAssembly.GetManifestResourceStream(existing);
        return SKBitmap.Decode(stream);
    }
}