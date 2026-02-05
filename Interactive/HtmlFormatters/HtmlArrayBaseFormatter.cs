using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.DotNet.Interactive.Formatting;

namespace ILNumerics.Community.Interactive.HtmlFormatters;

/// <summary>
/// Base class for HTML formatters that render ILNumerics arrays as tables.
/// </summary>
public abstract class HtmlArrayFormatterBase
{
    private const string JQueryJsCdn = "https://code.jquery.com/jquery-3.7.1.min.js";
    private const string DataTablesJsCdn = "https://cdn.datatables.net/2.3.4/js/dataTables.min.js";
    private const string DataTablesCssCdn = "https://cdn.datatables.net/2.3.4/css/dataTables.dataTables.min.css";

    /// <summary>
    /// Writes an HTML table representation of the array to the specified writer.
    /// </summary>
    /// <typeparam name="T">The array element type.</typeparam>
    /// <param name="array">The array to render.</param>
    /// <param name="writer">The writer to receive the HTML output.</param>
    protected void FormatTable<T>(BaseArray<T> array, TextWriter writer)
    {
        var maxElements = InteractiveOptions.MaxArrayElements;
        var columnCount = (int) Math.Min(maxElements, array.S[1]);
        var rowCount = (int) Math.Min(maxElements, array.S[0]);

        // Build table header
        var headerHtml = new StringBuilder();
        headerHtml.AppendLine("<thead>");
        headerHtml.Append("<tr>");
        headerHtml.Append("<th>index</th>");
        for (var c = 0; c < columnCount; c++)
            headerHtml.Append($"<th>{c}</th>");
        headerHtml.AppendLine("</tr>");
        headerHtml.AppendLine("</thead>");

        // Build table body (rows/cols)
        var bodyHtml = new StringBuilder();
        bodyHtml.AppendLine("<tbody>");
        for (var r = 0; r < rowCount; r++)
        {
            bodyHtml.Append("<tr>");
            bodyHtml.Append($"<th>{r}</th>");
            for (var c = 0; c < columnCount; c++)
            {
                // Format cell content and HTML-encode (to avoid breaking markup)
                var cell = array.GetValue(r, c)?.ToDisplayString();
                bodyHtml.Append($"<td>{WebUtility.HtmlEncode(cell ?? String.Empty)}</td>");
            }
            bodyHtml.AppendLine("</tr>");
        }
        bodyHtml.AppendLine("</tbody>");

        // Unique id for the table to allow multiple tables on one page
        var tableId = HtmlContentUtility.GetId("iln-dt-");

        // Write table markup and data
        writer.WriteLine($"<table id=\"{tableId}\" class=\"display\" style=\"width:100%\">\n{headerHtml}\n{bodyHtml}\n</table>");

        // jQuery-based initialization
        var script = $$"""
                       <script>
                       (function () {
                           var tableId = '{{tableId}}';

                           function initDataTable() {
                               try {
                                   var table = document.getElementById(tableId);
                                   if (!table) return;
                                   if (table.__dtInitialized) return;

                                   var $ = window.jQuery || window.$;
                                   if (!$ || !$.fn || !$.fn.dataTable) {
                                       throw new Error('jQuery DataTables plugin not available');
                                   }

                                   $(table).DataTable({
                                       scrollX: true,
                                       scrollY: 500,
                                       scrollCollapse: true,
                                       pageLength: 25
                                   });
                                   table.__dtInitialized = true;
                               } catch (e) {
                                   if (console && console.error) console.error(e);
                               }
                           }

                           function loadScript(src) {
                               return new Promise(function (res, rej) {
                                   var s = document.createElement('script');
                                   s.src = src;
                                   s.onload = res;
                                   s.onerror = rej;
                                   document.head.appendChild(s);
                               });
                           }

                           function loadCSS(href) {
                               if (!window.__ilnDataTablesCssIncluded) {
                                   var l = document.createElement('link');
                                   l.rel = 'stylesheet';
                                   l.href = href;
                                   document.head.appendChild(l);
                                   window.__ilnDataTablesCssIncluded = true;
                               }
                           }

                           // Ensure CSS is present
                           loadCSS('{{DataTablesCssCdn}}');

                           function ensureJQuery() {
                               if (window.jQuery || window.$) {
                                   return Promise.resolve(window.jQuery || window.$);
                               }
                               if (!window.__ilnJQueryReadyProm) {
                                   window.__ilnJQueryReadyProm = loadScript('{{JQueryJsCdn}}').then(function () {
                                       return window.jQuery || window.$;
                                   });
                               }
                               return window.__ilnJQueryReadyProm;
                           }

                           function ensureDataTables() {
                               var $ = window.jQuery || window.$;
                               if ($ && $.fn && $.fn.dataTable) {
                                   return Promise.resolve();
                               }
                               if (!window.__ilnDataTablesJqReadyProm) {
                                   window.__ilnDataTablesJqReadyProm = loadScript('{{DataTablesJsCdn}}');
                               }
                               return window.__ilnDataTablesJqReadyProm;
                           }

                           ensureJQuery()
                               .then(ensureDataTables)
                               .then(initDataTable)
                               .catch(function (e) {
                                   console && console.error && console.error(e);
                               });
                       })();
                       </script>
                       """;

        writer.WriteLine(script);

        // Write array dimensions and type
        writer.WriteLine($"<b>Dims: {String.Join(" x ", Enumerable.Range(0, (int)array.S.NumberOfDimensions).Select(i => array.S[i]))} [{array.GetElementType().FullName}]</b></br>");

        // Warning: More than 2 dimensions
        if (array.S.NumberOfDimensions > 2)
            writer.WriteLine($"<b>Note: </b>Array with {array.S.NumberOfDimensions} dimensions (first 2 dimensions shown) "
                             + $"&rarr; Use ILNumerics indexers to select elements, e.g. a[\":;:;1\"].</br>");

        // Warning: Table truncated at MaxArrayElements
        if (array.S[0] > maxElements || array.S[1] > maxElements)
            writer.WriteLine($"<b>Note: Table truncated at {maxElements} elements.</b></br>");
    }
}
