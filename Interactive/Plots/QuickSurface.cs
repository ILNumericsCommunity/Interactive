using System;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.ILMath;

namespace ILNumerics.Community.Interactive.Plots;

/// <summary>
/// Provides methods to quickly create line plots or surface plots.
/// </summary>
public static partial class QuickPlot
{
    /// <summary>
    /// Creates a surface plot from the given ZXY positions.
    /// </summary>
    /// <param name="ZXYPositions">Matrix of ZXY positions.</param>
    /// <param name="C">Optional color data.</param>
    /// <param name="xAxisScale">Scale mode for the X axis.</param>
    /// <param name="yAxisScale">Scale mode for the Y axis.</param>
    /// <param name="zAxisScale">Scale mode for the Z axis.</param>
    /// <returns>A scene containing the surface plot.</returns>
    /// <exception cref="ArgumentNullException">Thrown when ZXYPositions is null.</exception>
    /// <exception cref="ArgumentException">Thrown when ZXYPositions is not a matrix.</exception>
    public static Scene Surf(InArray<float> ZXYPositions, InArray<float> C = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, AxisScale zAxisScale = AxisScale.Linear)
    {
        using (Scope.Enter(ZXYPositions))
        {
            if (isnull(ZXYPositions))
                throw new ArgumentNullException(nameof(ZXYPositions));
            if (!ZXYPositions.IsMatrix)
                throw new ArgumentException("Argument 'ZXYPositions' must be a matrix of size [m x n x [1|2|3]].");

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            plotCube.Add(new Surface(ZXYPositions, C));
            plotCube.Rotation = InteractiveOptions.GraphSurfRotation;

            // AxisScale
            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;
            plotCube.ScaleModes.ZAxisScale = zAxisScale;

            return scene;
        }
    }

    /// <summary>
    /// Creates a surface plot from the given ZXY positions.
    /// </summary>
    /// <param name="ZXYPositions">Matrix of ZXY positions.</param>
    /// <param name="C">Optional color data.</param>
    /// <param name="xAxisScale">Scale mode for the X axis.</param>
    /// <param name="yAxisScale">Scale mode for the Y axis.</param>
    /// <param name="zAxisScale">Scale mode for the Z axis.</param>
    /// <param name="xAxisLabel">Label for the X axis.</param>
    /// <param name="yAxisLabel">Label for the Y axis.</param>
    /// <param name="zAxisLabel">Label for the Z axis.</param>
    /// <returns>A scene containing the surface plot.</returns>
    /// <exception cref="ArgumentNullException">Thrown when ZXYPositions is null.</exception>
    /// <exception cref="ArgumentException">Thrown when ZXYPositions is not a matrix.</exception>
    public static Scene Surf(InArray<double> ZXYPositions, InArray<float> C = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, AxisScale zAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, string zAxisLabel = null)
    {
        using (Scope.Enter(ZXYPositions, C))
        {
            if (isnull(ZXYPositions))
                throw new ArgumentNullException(nameof(ZXYPositions));
            if (!ZXYPositions.IsMatrix)
                throw new ArgumentException("Argument 'ZXYPositions' must be a matrix of size [m x n x [1|2|3]].");

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            plotCube.Add(new Surface(tosingle(ZXYPositions), C));
            plotCube.Rotation = InteractiveOptions.GraphSurfRotation;

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;
            plotCube.ScaleModes.ZAxisScale = zAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;
            plotCube.Axes.ZAxis.Label.Text = String.IsNullOrEmpty(zAxisLabel) ? "Z Axis" : zAxisLabel;

            return scene;
        }
    }

    /// <summary>
    /// Creates a surface plot from the given Z, X, and Y data.
    /// </summary>
    /// <param name="Z">Matrix of Z values.</param>
    /// <param name="X">Matrix or vector of X values.</param>
    /// <param name="Y">Matrix or vector of Y values.</param>
    /// <param name="C">Optional color data.</param>
    /// <param name="xAxisScale">Scale mode for the X axis.</param>
    /// <param name="yAxisScale">Scale mode for the Y axis.</param>
    /// <param name="zAxisScale">Scale mode for the Z axis.</param>
    /// <param name="xAxisLabel">Label for the X axis.</param>
    /// <param name="yAxisLabel">Label for the Y axis.</param>
    /// <param name="zAxisLabel">Label for the Z axis.</param>
    /// <returns>A scene containing the surface plot.</returns>
    /// <exception cref="ArgumentException">Thrown when Z, X, or Y are not valid matrices or vectors.</exception>
    public static Scene Surf(InArray<float> Z, InArray<float> X, InArray<float> Y, InArray<float> C = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, AxisScale zAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, string zAxisLabel = null)
    {
        using (Scope.Enter(Z, X, Y, C))
        {
            if (!isnull(Z) && !Z.IsVector && !Z.IsMatrix)
                throw new ArgumentException("Argument 'X' must be null, a vector of length n or a matrix of size [m by n], with m = Z.S[0], n = Z.S[1].");
            if (!isnull(X) && !X.IsVector && !X.IsMatrix)
                throw new ArgumentException("Argument 'X' must be null, a vector of length n or a matrix of size [m by n], with m = Z.S[0], n = Z.S[1].");
            if (!isnull(Y) && !Y.IsVector && !Y.IsMatrix)
                throw new ArgumentException("Argument 'Y' must be null, a vector of length m or a matrix of size [m by n], with m = Z.S[0], n = Z.S[1].");

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            plotCube.Add(new Surface(Z, X, Y, C));
            plotCube.Rotation = InteractiveOptions.GraphSurfRotation;

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;
            plotCube.ScaleModes.ZAxisScale = zAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;
            plotCube.Axes.ZAxis.Label.Text = String.IsNullOrEmpty(zAxisLabel) ? "Z Axis" : zAxisLabel;

            return scene;
        }
    }

    /// <summary>
    /// Creates a surface plot from the given Z, X, and Y data.
    /// </summary>
    /// <param name="Z">Matrix of Z values.</param>
    /// <param name="X">Matrix or vector of X values.</param>
    /// <param name="Y">Matrix or vector of Y values.</param>
    /// <param name="C">Optional color data.</param>
    /// <param name="xAxisScale">Scale mode for the X axis.</param>
    /// <param name="yAxisScale">Scale mode for the Y axis.</param>
    /// <param name="zAxisScale">Scale mode for the Z axis.</param>
    /// <param name="xAxisLabel">Label for the X axis.</param>
    /// <param name="yAxisLabel">Label for the Y axis.</param>
    /// <param name="zAxisLabel">Label for the Z axis.</param>
    /// <returns>A scene containing the surface plot.</returns>
    /// <exception cref="ArgumentException">Thrown when Z, X, or Y are not valid matrices or vectors.</exception>
    public static Scene Surf(InArray<double> Z, InArray<double> X, InArray<double> Y, InArray<float> C = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, AxisScale zAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, string zAxisLabel = null)
    {
        using (Scope.Enter(Z, X, Y, C))
        {
            if (!isnull(Z) && !Z.IsVector && !Z.IsMatrix)
                throw new ArgumentException("Argument 'X' must be null, a vector of length n or a matrix of size [m by n], with m = Z.S[0], n = Z.S[1].");
            if (!isnull(X) && !X.IsVector && !X.IsMatrix)
                throw new ArgumentException("Argument 'X' must be null, a vector of length n or a matrix of size [m by n], with m = Z.S[0], n = Z.S[1].");
            if (!isnull(Y) && !Y.IsVector && !Y.IsMatrix)
                throw new ArgumentException("Argument 'Y' must be null, a vector of length m or a matrix of size [m by n], with m = Z.S[0], n = Z.S[1].");

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            plotCube.Add(new Surface(tosingle(Z), tosingle(X), tosingle(Y), C));
            plotCube.Rotation = InteractiveOptions.GraphSurfRotation;

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;
            plotCube.ScaleModes.ZAxisScale = zAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;
            plotCube.Axes.ZAxis.Label.Text = String.IsNullOrEmpty(zAxisLabel) ? "Z Axis" : zAxisLabel;

            return scene;
        }
    }

    /// <summary>
    /// Creates a surface plot from the given function.
    /// </summary>
    /// <param name="ZFunc">Function to generate Z values.</param>
    /// <param name="xmin">Minimum X value.</param>
    /// <param name="xmax">Maximum X value.</param>
    /// <param name="xlen">Number of X values.</param>
    /// <param name="ymin">Minimum Y value.</param>
    /// <param name="ymax">Maximum Y value.</param>
    /// <param name="ylen">Number of Y values.</param>
    /// <param name="CFunc">Optional function to generate color data.</param>
    /// <param name="xAxisScale">Scale mode for the X axis.</param>
    /// <param name="yAxisScale">Scale mode for the Y axis.</param>
    /// <param name="zAxisScale">Scale mode for the Z axis.</param>
    /// <param name="xAxisLabel">Label for the X axis.</param>
    /// <param name="yAxisLabel">Label for the Y axis.</param>
    /// <param name="zAxisLabel">Label for the Z axis.</param>
    /// <returns>A scene containing the surface plot.</returns>
    public static Scene Surf(Func<float, float, float> ZFunc, float xmin = -10f, float xmax = 10f, int xlen = 50, float ymin = -10f, float ymax = 10f, int ylen = 50,
                             Func<float, float, float> CFunc = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, AxisScale zAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, string zAxisLabel = null)
    {
        var scene = new Scene();
        var plotCube = scene.Add(new PlotCube());
        plotCube.Add(new Surface(ZFunc, xmin, xmax, xlen, ymin, ymax, ylen, CFunc));
        plotCube.Rotation = InteractiveOptions.GraphSurfRotation;

        plotCube.ScaleModes.XAxisScale = xAxisScale;
        plotCube.ScaleModes.YAxisScale = yAxisScale;
        plotCube.ScaleModes.ZAxisScale = zAxisScale;

        plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
        plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;
        plotCube.Axes.ZAxis.Label.Text = String.IsNullOrEmpty(zAxisLabel) ? "Z Axis" : zAxisLabel;

        return scene;
    }

    /// <summary>
    /// Creates a surface plot from the given function.
    /// </summary>
    /// <param name="ZFunc">Function to generate Z values.</param>
    /// <param name="xmin">Minimum X value.</param>
    /// <param name="xmax">Maximum X value.</param>
    /// <param name="xlen">Number of X values.</param>
    /// <param name="ymin">Minimum Y value.</param>
    /// <param name="ymax">Maximum Y value.</param>
    /// <param name="ylen">Number of Y values.</param>
    /// <param name="CFunc">Optional function to generate color data.</param>
    /// <param name="xAxisScale">Scale mode for the X axis.</param>
    /// <param name="yAxisScale">Scale mode for the Y axis.</param>
    /// <param name="zAxisScale">Scale mode for the Z axis.</param>
    /// <param name="xAxisLabel">Label for the X axis.</param>
    /// <param name="yAxisLabel">Label for the Y axis.</param>
    /// <param name="zAxisLabel">Label for the Z axis.</param>
    /// <returns>A scene containing the surface plot.</returns>
    public static Scene Surf(Func<double, double, double> ZFunc, double xmin = -10d, double xmax = 10d, int xlen = 50, double ymin = -10d, double ymax = 10d, int ylen = 50,
                             Func<float, float, float> CFunc = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, AxisScale zAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, string zAxisLabel = null)
    {
        var scene = new Scene();
        var plotCube = scene.Add(new PlotCube());

        float ZFuncFloat(float x, float y) => (float)ZFunc(x, y);
        plotCube.Add(new Surface(ZFuncFloat, (float)xmin, (float)xmax, xlen, (float)ymin, (float)ymax, ylen, CFunc));
        plotCube.Rotation = InteractiveOptions.GraphSurfRotation;

        plotCube.ScaleModes.XAxisScale = xAxisScale;
        plotCube.ScaleModes.YAxisScale = yAxisScale;
        plotCube.ScaleModes.ZAxisScale = zAxisScale;

        plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
        plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;
        plotCube.Axes.ZAxis.Label.Text = String.IsNullOrEmpty(zAxisLabel) ? "Z Axis" : zAxisLabel;

        return scene;
    }
}
