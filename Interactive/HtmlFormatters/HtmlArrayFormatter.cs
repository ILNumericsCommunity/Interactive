using System;
using Microsoft.DotNet.Interactive.Formatting;
using static ILNumerics.ILMath;

namespace ILNumerics.Community.Interactive.HtmlFormatters;

public class HtmlArrayFormatter : HtmlArrayFormatterBase, ITypeFormatter
{
    #region ITypeFormatter Members

    public string MimeType => HtmlFormatter.MimeType;

    public Type Type => typeof(Array<>);

    public bool Format(object instance, FormatContext context)
    {
        switch (instance)
        {
            case Array<sbyte> sbyteArray:
                FormatTable((Array<sbyte>) squeeze(sbyteArray), context.Writer);
                return true;
            case Array<short> shortArray:
                FormatTable((Array<short>) squeeze(shortArray), context.Writer);
                return true;
            case Array<int> intArray:
                FormatTable((Array<int>) squeeze(intArray), context.Writer);
                return true;
            case Array<long> longArray:
                FormatTable((Array<long>) squeeze(longArray), context.Writer);
                return true;
            case Array<float> floatArray:
                FormatTable((Array<float>) squeeze(floatArray), context.Writer);
                return true;
            case Array<double> doubleArray:
                FormatTable((Array<double>) squeeze(doubleArray), context.Writer);
                return true;
            case Array<fcomplex> fcomplexArray:
                FormatTable((Array<fcomplex>) squeeze(fcomplexArray), context.Writer);
                return true;
            case Array<complex> complexArray:
                FormatTable((Array<complex>) squeeze(complexArray), context.Writer);
                return true;
            default:
                return false;
        }
    }

    #endregion
}