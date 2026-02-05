using System;
using Microsoft.DotNet.Interactive.Formatting;
using SkiaSharp;
using static ILNumerics.Community.Interactive.HtmlContentUtility;

namespace ILNumerics.Community.Interactive.HtmlFormatters;

/// <summary>
/// Formats SkiaSharp bitmaps as HTML images.
/// </summary>
public class HtmlSKBitmapFormatter : ITypeFormatter
{
    /// <summary>
    /// Gets the supported MIME type.
    /// </summary>
    public string MimeType => HtmlFormatter.MimeType;

    /// <summary>
    /// Gets the supported type.
    /// </summary>
    public Type Type => typeof(SKBitmap);

    /// <summary>
    /// Formats a bitmap instance as HTML.
    /// </summary>
    /// <param name="instance">The instance to format.</param>
    /// <param name="context">The formatting context.</param>
    /// <returns><see langword="true"/> when formatted; otherwise <see langword="false"/>.</returns>
    public bool Format(object instance, FormatContext context)
    {
        if (instance is not SKBitmap bitmap)
            return false;

        context.Writer.Write(WritePNG(bitmap));

        return true;
    }
}