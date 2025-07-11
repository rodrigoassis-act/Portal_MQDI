using System.Collections.Generic;
using System.Text;

namespace ONS.PortalMQDI.Shared.Utils
{
    public static class HTMLUtil
    {
        public static string GenerateTableHeaderRow(string[] data)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr class=\"header-row\">");

            foreach (var column in data)
            {
                sb.Append($"<th class=\"text-left fonte-campo-relatorio\">{column}</th>");
            }

            sb.Append("</tr>");

            return sb.ToString();
        }

        public static string GenerateTableBodyRows(List<(string Value, bool ApplySpecialClass)> cellDataAndClass)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");

            foreach (var cell in cellDataAndClass)
            {
                var row = cell.Value;
                var shouldApplySpecialClass = cell.ApplySpecialClass;

                string cssClass = shouldApplySpecialClass == true ? "red" : "";

                sb.Append($"<td class=\"text-left fonte-campo-relatorio {cssClass}\">{row}</td>");
            }

            sb.Append("</tr>");

            return sb.ToString();
        }




        public static string GenerateTable(string html)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"table table-hover table-striped rdx-table\">");

            sb.Append("<tbody>");
            sb.Append(html);
            sb.Append("</tbody>");

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
