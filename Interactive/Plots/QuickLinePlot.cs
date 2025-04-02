using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Creates a line plot with (up to four) y-values against a common x-value array.
    /// </summary>  
    /// <param name="x">The x-values for the plot.</param>  
    /// <param name="y1">The first set of y-values for the plot.</param>  
    /// <param name="y2">The second set of y-values for the plot (optional).</param>  
    /// <param name="y3">The third set of y-values for the plot (optional).</param>  
    /// <param name="y4">The fourth set of y-values for the plot (optional).</param>  
    /// <param name="xAxisScale">The scale mode for the x-axis (default is linear).</param>  
    /// <param name="yAxisScale">The scale mode for the y-axis (default is linear).</param>  
    /// <param name="xAxisLabel">The label for the x-axis (optional).</param>  
    /// <param name="yAxisLabel">The label for the y-axis (optional).</param>  
    /// <param name="labels">The labels for the legend (optional).</param>  
    /// <returns>A Scene object containing the plot.</returns>  
    /// <exception cref="ArgumentNullException">Thrown when x is null.</exception>  
    /// <exception cref="ArgumentException">Thrown when x is not a 1-dim vector or when y-values are not 1-dim vectors or do not match the length of x.</exception>  
    public static Scene Plot(InArray<float> x, InArray<float> y1, InArray<float> y2 = null, InArray<float> y3 = null, InArray<float> y4 = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, string xAxisLabel = null, string yAxisLabel = null,
                             IEnumerable<string> labels = null)
    {
        using (Scope.Enter(x, y1, y2, y3, y4))
        {
            if (isnull(x))
                throw new ArgumentNullException(nameof(x));
            if (!x.IsVector)
                throw new ArgumentException("Argument 'x' must be a 1-dim vector.");

            var yArgs = new[] { y1, y2, y3, y4 }.Where(y => !isnull(y)).ToArray();
            foreach (var y in yArgs)
            {
                if (!y.IsVector)
                    throw new ArgumentException("All arguments 'y' must be a 1-dim vector.");
                if (x.Length != y.Length)
                    throw new ArgumentException("Arguments 'x' and all 'y' must be vectors with same length (number of elements).");
            }

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            foreach (var y in yArgs)
                plotCube.Add(new LinePlot(x, y));

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;

            var legendLabels = labels?.Where(label => label != null).ToArray();
            if (legendLabels != null && legendLabels.Length > 0)
                plotCube.Add(new Legend(legendLabels));

            return scene;
        }
    }

    /// <summary>  
    /// Creates a line plot with (up to four) y-values against a common x-value array.
    /// </summary>  
    /// <param name="x">The x-values for the plot.</param>  
    /// <param name="y1">The first set of y-values for the plot.</param>  
    /// <param name="y2">The second set of y-values for the plot (optional).</param>  
    /// <param name="y3">The third set of y-values for the plot (optional).</param>  
    /// <param name="y4">The fourth set of y-values for the plot (optional).</param>  
    /// <param name="xAxisScale">The scale mode for the x-axis (default is linear).</param>  
    /// <param name="yAxisScale">The scale mode for the y-axis (default is linear).</param>  
    /// <param name="xAxisLabel">The label for the x-axis (optional).</param>  
    /// <param name="yAxisLabel">The label for the y-axis (optional).</param>  
    /// <param name="labels">The labels for the legend (optional).</param>  
    /// <returns>A Scene object containing the plot.</returns>  
    /// <exception cref="ArgumentNullException">Thrown when x is null.</exception>  
    /// <exception cref="ArgumentException">Thrown when x is not a 1-dim vector or when y-values are not 1-dim vectors or do not match the length of x.</exception>  
    public static Scene Plot(InArray<double> x, InArray<double> y1, InArray<double> y2 = null, InArray<double> y3 = null, InArray<double> y4 = null,
                             AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear, string xAxisLabel = null, string yAxisLabel = null,
                             IEnumerable<string> labels = null)
    {
        using (Scope.Enter(x, y1, y2, y3, y4))
        {
            if (isnull(x))
                throw new ArgumentNullException(nameof(x));
            if (!x.IsVector)
                throw new ArgumentException("Argument 'x' must be a 1-dim vector.");

            var yArgs = new[] { y1, y2, y3, y4 }.Where(y => !isnull(y)).ToArray();
            foreach (var y in yArgs)
            {
                if (!y.IsVector)
                    throw new ArgumentException("All arguments 'y' must be a 1-dim vector.");
                if (x.Length != y.Length)
                    throw new ArgumentException("Arguments 'x' and all 'y' must be vectors with same length (number of elements).");
            }

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            foreach (var y in yArgs)
                plotCube.Add(new LinePlot(x, y));

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;

            var legendLabels = labels?.Where(label => label != null).ToArray();
            if (legendLabels != null && legendLabels.Length > 0)
                plotCube.Add(new Legend(legendLabels));

            return scene;
        }
    }

    /// <summary>  
    /// Creates a line plot from an array of positions.
    /// </summary>  
    /// <param name="positions">The positions for the plot.</param>  
    /// <param name="xAxisScale">The scale mode for the x-axis (default is linear).</param>  
    /// <param name="yAxisScale">The scale mode for the y-axis (default is linear).</param>  
    /// <param name="xAxisLabel">The label for the x-axis (optional).</param>  
    /// <param name="yAxisLabel">The label for the y-axis (optional).</param>  
    /// <param name="labels">The labels for the legend (optional).</param>  
    /// <returns>A Scene object containing the plot.</returns>  
    /// <exception cref="ArgumentNullException">Thrown when positions is null.</exception>  
    public static Scene Plot(InArray<float> positions, AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, IEnumerable<string> labels = null)
    {
        using (Scope.Enter(positions))
        {
            if (isnull(positions))
                throw new ArgumentNullException(nameof(positions));

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            plotCube.Add(new LinePlot(positions));

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;

            var legendLabels = labels?.Where(label => label != null).ToArray();
            if (legendLabels != null && legendLabels.Length > 0)
                plotCube.Add(new Legend(legendLabels));

            return scene;
        }
    }

    /// <summary>  
    /// Creates a line plot from an array of positions.
    /// </summary>  
    /// <param name="positions">The positions for the plot.</param>  
    /// <param name="xAxisScale">The scale mode for the x-axis (default is linear).</param>  
    /// <param name="yAxisScale">The scale mode for the y-axis (default is linear).</param>  
    /// <param name="xAxisLabel">The label for the x-axis (optional).</param>  
    /// <param name="yAxisLabel">The label for the y-axis (optional).</param>  
    /// <param name="labels">The labels for the legend (optional).</param>  
    /// <returns>A Scene object containing the plot.</returns>  
    /// <exception cref="ArgumentNullException">Thrown when positions is null.</exception>  
    public static Scene Plot(InArray<double> positions, AxisScale xAxisScale = AxisScale.Linear, AxisScale yAxisScale = AxisScale.Linear,
                             string xAxisLabel = null, string yAxisLabel = null, IEnumerable<string> labels = null)
    {
        using (Scope.Enter(positions))
        {
            if (isnull(positions))
                throw new ArgumentNullException(nameof(positions));

            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            plotCube.Add(new LinePlot(tosingle(positions)));

            plotCube.ScaleModes.XAxisScale = xAxisScale;
            plotCube.ScaleModes.YAxisScale = yAxisScale;

            plotCube.Axes.XAxis.Label.Text = String.IsNullOrEmpty(xAxisLabel) ? "X Axis" : xAxisLabel;
            plotCube.Axes.YAxis.Label.Text = String.IsNullOrEmpty(yAxisLabel) ? "Y Axis" : yAxisLabel;

            var legendLabels = labels?.Where(label => label != null).ToArray();
            if (legendLabels != null && legendLabels.Length > 0)
                plotCube.Add(new Legend(legendLabels));

            return scene;
        }
    }
}
