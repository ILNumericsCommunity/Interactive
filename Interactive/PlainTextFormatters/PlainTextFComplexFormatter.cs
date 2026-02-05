using System;
using Microsoft.DotNet.Interactive.Formatting;

namespace ILNumerics.Community.Interactive.PlainTextFormatters;

/// <summary>
/// Formats single-precision complex numbers as plain text.
/// </summary>
public class PlainTextFComplexFormatter : ITypeFormatter
{
    #region ITypeFormatter Members

    /// <summary>
    /// Gets the supported MIME type.
    /// </summary>
    public string MimeType => PlainTextFormatter.MimeType;

    /// <summary>
    /// Gets the supported type.
    /// </summary>
    public Type Type => typeof(fcomplex);

    /// <summary>
    /// Formats a complex instance as plain text.
    /// </summary>
    /// <param name="instance">The instance to format.</param>
    /// <param name="context">The formatting context.</param>
    /// <returns><see langword="true"/> when formatted; otherwise <see langword="false"/>.</returns>
    public bool Format(object instance, FormatContext context)
    {
        if (instance is not fcomplex complexValue)
            return false;

        context.Writer.Write(complexValue.ToString());
        return true;
    }

    #endregion
}