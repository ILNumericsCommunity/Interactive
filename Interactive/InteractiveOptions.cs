﻿using ILNumerics.Drawing;
using Point = System.Drawing.Point;

namespace ILNumerics.Community.Interactive;

/// <summary>
/// Mode for displaying the graph.
/// </summary>
public enum GraphMode
{
    /// <summary>PNG image</summary>
    Png,

    /// <summary>SVG image</summary>
    Svg,

    /// <summary>Web Interactive via Plotly</summary>
    WebPlotly
}

public static class InteractiveOptions
{
    /// <summary>Gets or sets the graph mode, i.e. how the Scene is displayed.</summary>
    public static GraphMode GraphMode { get; set; } = GraphMode.WebPlotly;

    /// <summary>Gets or sets the graph SVG size limit. Above the limit, we use bitmap (PNG) as fallback for faster rendering.</summary>
    public static int GraphSvgSizeLimit { get; set; } = 4 * 1000 * 1000; // 4M bytes

    /// <summary>Gets or sets the pixel size of the output graph.</summary>
    public static Point GraphSize { get; set; } = new Point(800, 600);

    /// <summary>Gets or sets the default rotation of the plotCube for Surface plots (via Surf).</summary>
    public static Matrix4 GraphSurfRotation { get; set; } = Matrix4.Rotation(new Vector3(1f, 0.23f, 1), 0.7f);

    /// <summary>Gets or sets the maximum array elements to display (before the array output gets truncated).</summary>
    public static int MaxArrayElements { get; set; } = 50;
}