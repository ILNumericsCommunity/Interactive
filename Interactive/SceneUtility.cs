using System;
using System.IO;
using ILNumerics.Drawing;
using SkiaSharp;

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
    public static void SaveAsSvg(this Scene scene, string filePath, System.Drawing.Size? graphSize = null)
    {
        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        filePath = Path.ChangeExtension(filePath, ".svg");
        graphSize ??= InteractiveOptions.GraphSize;

        using var fileStream = new FileStream(filePath, FileMode.Create);
        new SVGDriver(fileStream, graphSize.Value.Width, graphSize.Value.Height, scene).Render();

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
    public static void SaveAsTikz(this Scene scene, string filePath, System.Drawing.Size? graphSize = null, double ppmm = 10.0)
    {
        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        filePath = Path.ChangeExtension(filePath, ".tikz");
        graphSize ??= new System.Drawing.Size((int) (InteractiveOptions.GraphSize.Width / ppmm), (int) (InteractiveOptions.GraphSize.Height / ppmm));

        TikzExport.TikzExport.ExportFile(scene, filePath, graphSize.Value);
        
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
    public static void SaveAsPng(this Scene scene, string filePath, System.Drawing.Size? graphSize = null, int resolution = 300)
    {
        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        filePath = Path.ChangeExtension(filePath, ".png");
        graphSize ??= InteractiveOptions.GraphSize;

        var bitmap = scene.RenderSKBitmap(graphSize);

        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var image = SKImage.FromBitmap(bitmap);
        image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(stream);

        Console.WriteLine($"Scene saved as PNG at '{filePath}'.");
    }

    #endregion

    #region Internal

    internal static SKBitmap? RenderSKBitmap(this Scene scene, System.Drawing.Size? graphSize = null)
    {
        graphSize ??= InteractiveOptions.GraphSize;

        using var memoryStream = new MemoryStream();

        // Render bitmap
        var driver = new GDIDriver(new CommonBackBuffer(), scene);
        driver.BackBuffer.Size = graphSize.Value;
        driver.Render();

        if (driver.BackBuffer is not CommonBackBuffer backBuffer)
            return null;

        Array<int> pixelBuffer = backBuffer.PixelBuffer;

        var bitmap = new SKBitmap(graphSize.Value.Width, graphSize.Value.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        bitmap.InstallPixels(bitmap.Info, pixelBuffer.GetHostPointerForRead(), bitmap.RowBytes);

        return bitmap;
    }

    #endregion
}
