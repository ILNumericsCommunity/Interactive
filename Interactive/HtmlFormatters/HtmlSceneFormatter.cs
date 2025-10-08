using System;
using System.IO;
using ILNumerics.Drawing;
using Microsoft.DotNet.Interactive.Formatting;
using Scene = ILNumerics.Drawing.Scene;
using static ILNumerics.Community.Interactive.HtmlContentUtility;

namespace ILNumerics.Community.Interactive.HtmlFormatters;

public class HtmlSceneFormatter : ITypeFormatter
{
    public string MimeType => HtmlFormatter.MimeType;

    public Type Type => typeof(Scene);

    public bool Format(object instance, FormatContext context)
    {
        if (instance is not Scene scene)
            return false;

        scene.Configure();

        switch (InteractiveOptions.GraphMode)
        {
            case GraphMode.Png:
                RenderPng(context, scene);
                break;
            case GraphMode.Svg:
                RenderSvg(context, scene);
                break;
            case GraphMode.WebPlotly:
                RenderWebPlotly(context, scene);
                break;
            default:
                return false;
        }

        return true;
    }

    private void RenderPng(FormatContext context, Scene scene)
    {
        var bitmap = scene.RenderSKBitmap();

        // Embed base64-encoded PNG as HTML content
        context.Writer.Write(WritePNG(bitmap));
    }

    private void RenderSvg(FormatContext context, Scene scene)
    {
        using var memoryStream = new MemoryStream();

        // Render SVG into memory stream
        var graphSize = InteractiveOptions.GraphSize;
        new SVGDriver(memoryStream, graphSize.Width, graphSize.Height, scene).Render();

        var svgBytes = memoryStream.ToArray();
        if (svgBytes.Length <= InteractiveOptions.GraphSvgSizeLimit)
        {
            // Embed SVG as HTML content
            context.Writer.Write(WriteSVG(svgBytes));
        }
        else
        {
            // Fallback to embedded bitmap (Png) if the SVG source size is too large (very slow rendering)
            var note = $"<div><b>Note: SVG output too large (&gt; {InteractiveOptions.GraphSvgSizeLimit / (1000 * 1000)} MBytes). Using bitmap (PNG) instead.</b></div>";
            context.Writer.WriteLine(note);

            RenderPng(context, scene);
        }
    }

    private void RenderWebPlotly(FormatContext context, Scene scene)
    {
        var chart = WebExport.WebExport.GetChart(scene);
        if (chart != null)
        {
            // Render Plotly chart
            var htmlString = chart.RenderPartial();
            context.Writer.Write(htmlString);
        }
        else
        {
            // Fallback to SVG output
            var note = "<div><b>Note: HTML Plotly output not possible. Using SVG instead.</b></div>";
            context.Writer.WriteLine(note);

            RenderSvg(context, scene);
        }
    }
}