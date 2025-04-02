using System;
using System.Drawing.Imaging;
using System.IO;
using ILNumerics.Drawing;
using Point = System.Drawing.Point;

namespace ILNumerics.Community.Interactive;

/// <summary>
/// Provides utility methods for saving scenes in various formats.
/// </summary>
public static class SceneUtility
{
    #region SaveAs

    /// <summary>
    /// Saves the scene as an SVG file.
    /// </summary>
    /// <param name="scene">The scene to save.</param>
    /// <param name="filePath">The file path where the SVG will be saved.</param>
    /// <param name="graphSize">Optional. The size of the graph. If not provided, the default size will be used.</param>
    /// <exception cref="ArgumentNullException">Thrown when the file path is null or empty.</exception>
    public static void SaveAsSvg(this Scene scene, string filePath, Point? graphSize = null)
    {
        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        filePath = Path.ChangeExtension(filePath, ".svg");
        graphSize ??= InteractiveOptions.GraphSize;

        using var fileStream = new FileStream(filePath, FileMode.Create);
        new SVGDriver(fileStream, graphSize.Value.X, graphSize.Value.Y, scene).Render();

        Console.WriteLine($"Scene saved as SVG at '{filePath}'.");
    }

    /// <summary>
    /// Saves the scene as a TikZ/PGFPlots file.
    /// </summary>
    /// <param name="scene">The scene to save.</param>
    /// <param name="filePath">The file path where the TIKZ will be saved.</param>
    /// <param name="graphSize">Optional. The size of the graph. If not provided, a default size will be used (100px = 10 mm).</param>
    /// <param name="ppmm">Optional. Pixels per millimeter. Default is 10.0 (i.e. 100 px -> 10 mm).</param>
    /// <exception cref="ArgumentNullException">Thrown when the file path is null or empty.</exception>
    public static void SaveAsTikz(this Scene scene, string filePath, Point? graphSize = null, double ppmm = 10.0)
    {
        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        filePath = Path.ChangeExtension(filePath, ".tikz");
        graphSize ??= new Point((int) (InteractiveOptions.GraphSize.X / ppmm), (int) (InteractiveOptions.GraphSize.Y / ppmm));

        using var fileStream = new FileStream(filePath, FileMode.Create);
        using var streamWriter = new StreamWriter(fileStream);
        TikzExport.TikzExport.Export(scene, streamWriter, new System.Drawing.Size(graphSize.Value));
        streamWriter.Flush();

        Console.WriteLine($"Scene saved as TIKZ at '{filePath}'.");
    }

    /// <summary>
    /// Saves the scene as a PNG file.
    /// </summary>
    /// <param name="scene">The scene to save.</param>
    /// <param name="filePath">The file path where the PNG will be saved.</param>
    /// <param name="graphSize">Optional. The size of the graph. If not provided, the default size will be used.</param>
    /// <param name="resolution">Optional. The resolution of the PNG. Default is 300 DPI.</param>
    /// <exception cref="ArgumentNullException">Thrown when the file path is null or empty.</exception>
    public static void SaveAsPng(this Scene scene, string filePath, Point? graphSize = null, int resolution = 300)
    {
        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        filePath = Path.ChangeExtension(filePath, ".png");
        graphSize ??= InteractiveOptions.GraphSize;

        var driver = new GDIDriver(graphSize.Value.X, graphSize.Value.Y, scene);
        driver.Render();
        driver.BackBuffer.Bitmap.SetResolution(resolution, resolution);
        driver.BackBuffer.Bitmap.Save(filePath, ImageFormat.Png);

        Console.WriteLine($"Scene saved as PNG at '{filePath}'.");
    }

    #endregion
}
