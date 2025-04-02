using System;
using Microsoft.DotNet.Interactive.Formatting;

namespace ILNumerics.Community.Interactive.PlainTextFormatters;

public class PlainTextFComplexFormatter : ITypeFormatter
{
    #region ITypeFormatter Members

    public string MimeType => PlainTextFormatter.MimeType;

    public Type Type => typeof(fcomplex);

    public bool Format(object instance, FormatContext context)
    {
        if (instance is not fcomplex complexValue)
            return false;

        context.Writer.Write(complexValue.ToString());
        return true;
    }

    #endregion
}