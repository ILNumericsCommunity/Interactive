using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Html;
using SkiaSharp;
using System.Net;

namespace ILNumerics.Community.Interactive;

/// <summary>
/// Utility class for generating HTML content with embedded images.
/// </summary>
public static class HtmlContentUtility
{
    /// <summary>
    /// Generates HTML content with a base64-encoded embedded PNG image.
    /// </summary>
    /// <param name="bitmap">The bitmap to encode as PNG image.</param>
    /// <returns>An <see cref="IHtmlContent"/> containing the embedded PNG image.</returns>
    public static IHtmlContent WritePNG(SKBitmap bitmap)
    {
        // Write bitmap into memory stream
        using var memoryStream = new MemoryStream();
        using var image = SKImage.FromBitmap(bitmap);
        image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(memoryStream);

        return WritePNG(memoryStream.ToArray(), new System.Drawing.Size(bitmap.Width, bitmap.Height));
    }

    /// <summary>
    /// Generates HTML content with a base64-encoded embedded PNG image.
    /// </summary>
    /// <param name="pngBytes">The byte array of the PNG image.</param>
    /// <param name="graphSize">The size of the image.</param>
    /// <returns>An <see cref="IHtmlContent"/> containing the embedded PNG image.</returns>
    public static IHtmlContent WritePNG(byte[] pngBytes, System.Drawing.Size graphSize)
    {
        var imageSource = $"data:image/png;base64,{Convert.ToBase64String(pngBytes)}";

        // Build a simple <img> tag string similar to PocketView output
        var imgHtml = $"<img src=\"{imageSource}\" width=\"{graphSize.Width}\" height=\"{graphSize.Height}\" />";

        return new HtmlString(imgHtml);
    }

    /// <summary>
    /// Generates HTML content with an embedded SVG image.
    /// </summary>
    /// <param name="svgBytes">The byte array of the SVG image.</param>
    /// <returns>An <see cref="IHtmlContent"/> containing the embedded SVG image.</returns>
    public static IHtmlContent WriteSVG(byte[] svgBytes)
    {
        return WriteSVG(Encoding.UTF8.GetString(svgBytes, 0, svgBytes.Length));
    }

    /// <summary>
    /// Generates HTML content with an embedded SVG image.
    /// </summary>
    /// <param name="svgContent">The SVG content as a string.</param>
    /// <returns>An <see cref="IHtmlContent"/> containing the embedded SVG image.</returns>
    public static IHtmlContent WriteSVG(string svgContent)
    {
        svgContent = svgContent.Replace("<?xml version='1.0' encoding='UTF-8'?>", ""); // Strip XML header
        svgContent = svgContent.Replace("<!DOCTYPE svg PUBLIC '-//W3C//DTD SVG 1.1//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd'>", ""); // Strip DocType header
        svgContent = svgContent.Replace("\r\n", ""); // Strip all line breaks

        var id = GetId("svg-");
        var divHtml = $"<div id=\"{WebUtility.HtmlEncode(id)}\">{svgContent}</div>";

        return new HtmlString(divHtml);
    }

    #region Private

    internal static string GetId(string type) => type + Guid.NewGuid().ToString("N");

    #endregion
}
