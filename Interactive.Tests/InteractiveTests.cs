using System;
using System.Threading.Tasks;
using ILNumerics.Community.Interactive;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.CSharp;
using Xunit;

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

    [Fact]
    public async Task DoubleArrayTable()
    {
        var result = await _kernel.SubmitCodeAsync(@"
using ILNumerics;
using static ILNumerics.ILMath;
using static ILNumerics.Globals;

Array<double> a = randn(2, 3, 4);
return a["":;:;2""];");
    }
}
