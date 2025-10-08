using ILNumerics;
using ILNumerics.Community.Interactive;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive.Formatting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static ILNumerics.ILMath;

namespace Interactive.Tests;

public class InteractiveTests : IDisposable
{
    private readonly Kernel _kernel;

    public InteractiveTests()
    {
        _kernel = new CompositeKernel { new CSharpKernel() };

        ILNumericsKernelExtension.Load(_kernel);
    }

    #region Implementation of IDisposable

    public void Dispose()
    {
        _kernel.Dispose();
    }

    #endregion

    #region Direct

    [Fact]
    public async Task HtmlArrayFormatter()
    {
        Array<double> a = randn(20, 150, 2);

        var displayString = a.ToDisplayString(HtmlFormatter.MimeType);
    }

    [Fact]
    public async Task HtmlSceneFormatter()
    {
        var scene = new Scene { new PlotCube { new LinePlot(tosingle(randn(1, 100))) } };

        var displayedValue = scene.Display(HtmlFormatter.MimeType);
    }

    #endregion

    #region Kernel

    [Fact]
    public async Task DoubleArrayTable()
    {
        var result = await _kernel.SubmitCodeAsync(@"
using ILNumerics;
using static ILNumerics.ILMath;
using static ILNumerics.Globals;

Array<double> a = randn(2, 3, 4);
return a["":;:;2""];");

        var message = result.Events.Last().ToString();
    }

    #endregion
}
