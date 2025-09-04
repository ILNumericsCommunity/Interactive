using System;
using System.Threading.Tasks;
using ILNumerics.Community.Interactive.HtmlFormatters;
using ILNumerics.Community.Interactive.PlainTextFormatters;
using ILNumerics.Drawing;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive.Formatting;
using SkiaSharp;

namespace ILNumerics.Community.Interactive;

public class ILNumericsKernelExtension : IKernelExtension
{
    public static void Load(Kernel kernel)
    {
        // Formatter: (f)complex
        Formatter.SetPreferredMimeTypesFor(typeof(fcomplex), PlainTextFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(complex), PlainTextFormatter.MimeType);
        Formatter.Register(new PlainTextFComplexFormatter());
        Formatter.Register(new PlainTextComplexFormatter());

        // Formatters: Array
        Formatter.SetPreferredMimeTypesFor(typeof(Array<sbyte>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<short>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<int>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<long>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<float>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<double>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<fcomplex>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(Array<complex>), HtmlFormatter.MimeType);
        Formatter.Register(new HtmlArrayFormatter());

        // Formatters: RetArray
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<sbyte>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<short>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<int>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<long>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<float>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<double>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<fcomplex>), HtmlFormatter.MimeType);
        Formatter.SetPreferredMimeTypesFor(typeof(RetArray<complex>), HtmlFormatter.MimeType);
        Formatter.Register(new HtmlRetArrayFormatter());

        // Formatters: SceneGraph
        Formatter.SetPreferredMimeTypesFor(typeof(SKBitmap), HtmlFormatter.MimeType);
        Formatter.Register(new HtmlSKBitmapFormatter());
        Formatter.SetPreferredMimeTypesFor(typeof(Scene), HtmlFormatter.MimeType);
        Formatter.Register(new HtmlSceneFormatter());

        // Load Extension
        var extension = new ILNumericsKernelExtension();
        extension.OnLoadAsync(kernel).GetAwaiter().GetResult();
    }

    #region Implementation of IKernelExtension

    /// <inheritdoc />
    public async Task OnLoadAsync(Kernel kernel)
    {
        // Using Statements
        var kernelCSharp = (CSharpKernel) kernel.FindKernelByName("csharp");
        await kernelCSharp.SubmitCodeAsync("using ILNumerics;");
        await kernelCSharp.SubmitCodeAsync("using ILNumerics.Drawing;");
        await kernelCSharp.SubmitCodeAsync("using ILNumerics.Drawing.Plotting;");
        await kernelCSharp.SubmitCodeAsync("using ILNumerics.Community.Interactive;");
        await kernelCSharp.SubmitCodeAsync("using static ILNumerics.ILMath;");
        await kernelCSharp.SubmitCodeAsync("using static ILNumerics.Globals;");
        await kernelCSharp.SubmitCodeAsync("using static ILNumerics.Community.Interactive.Plots.QuickPlot;");

        if (KernelInvocationContext.Current is { } context)
        {
            var message = new HtmlString("<hr>"
                                         + "<u><b>Welcome to ILNumerics.Community.Interactive!</b></u>"
                                         + "<lu>"
                                         + "<li><b>ILNumerics.Community.Interactive</b> integrates <b>ILNumerics Ultimate VS</b> (<a href=\"https://ilnumerics.net/\">https://ilnumerics.net/</a>), which comes with its own license conditions.</li>"
                                         + "<li>Development for <b>ILNumerics.Community.Interactive</b> requires a valid license from <b>ILNumerics</b>.</li>"
                                         + "<li>In the context of <b>ILNumerics.Community.Interactive</b> and within <u>.NET Interactive</u> all features of <b>ILNumerics</b> can be used <i>free of charge</i> (incl. commercial use).</li>"
                                         + "</lu>"
                                         + "<hr>");
            context.Display(message, HtmlFormatter.MimeType);
        }
    }

    #endregion
}