using System;
using Microsoft.DotNet.Interactive.Formatting;
using SkiaSharp;
using static ILNumerics.Community.Interactive.HtmlContentUtility;

namespace ILNumerics.Community.Interactive.HtmlFormatters;

public class HtmlSKBitmapFormatter : ITypeFormatter
{
    public string MimeType => HtmlFormatter.MimeType;

    public Type Type => typeof(SKBitmap);

    public bool Format(object instance, FormatContext context)
    {
        if (instance is not SKBitmap bitmap)
            return false;

        context.Writer.Write(WritePNG(bitmap));

        return true;
    }
}