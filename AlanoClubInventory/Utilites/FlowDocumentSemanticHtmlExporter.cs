using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace AlanoClubInventory.Utilites
{

    public static class FlowDocumentSemanticHtmlExporter
    {
        public static string ToHtml(FlowDocument doc)
        {
            if (doc == null) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset=\"UTF-8\"></head><body>");

            foreach (Block block in doc.Blocks)
            {
                sb.AppendLine(ConvertBlock(block));
            }

            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        private static string ConvertBlock(Block block)
        {
            if (block is Paragraph p)
            {
                return $"<p>{ConvertInlines(p.Inlines)}</p>";
            }
            else if (block is Section s)
            {
                var inner = new StringBuilder();
                foreach (Block innerBlock in s.Blocks)
                    inner.AppendLine(ConvertBlock(innerBlock));
                return $"<section>{inner}</section>";
            }
            else if (block is List list)
            {
                string tag = list.MarkerStyle == TextMarkerStyle.Disc ? "ul" : "ol";
                var inner = new StringBuilder();
                foreach (ListItem item in list.ListItems)
                {
                    inner.AppendLine("<li>");
                    foreach (Block itemBlock in item.Blocks)
                        inner.AppendLine(ConvertBlock(itemBlock));
                    inner.AppendLine("</li>");
                }
                return $"<{tag}>{inner}</{tag}>";
            }
            else if (block is Table table)
            {
                var inner = new StringBuilder();
                foreach (TableRowGroup group in table.RowGroups)
                {
                    inner.AppendLine("<tbody>");
                    foreach (TableRow row in group.Rows)
                    {
                        inner.AppendLine("<tr>");
                        foreach (TableCell cell in row.Cells)
                        {
                            inner.AppendLine("<td>");
                            foreach (Block cellBlock in cell.Blocks)
                                inner.AppendLine(ConvertBlock(cellBlock));
                            inner.AppendLine("</td>");
                        }
                        inner.AppendLine("</tr>");
                    }
                    inner.AppendLine("</tbody>");
                }
                return $"<table border=\"1\">{inner}</table>";
            }

            return string.Empty;
        }

        private static string ConvertInlines(InlineCollection inlines)
        {
            var sb = new StringBuilder();
            foreach (Inline inline in inlines)
            {
                if (inline is Run run)
                {
                    sb.Append(System.Net.WebUtility.HtmlEncode(run.Text));
                }
                else if (inline is Bold bold)
                {
                    sb.Append($"<b>{ConvertInlines(bold.Inlines)}</b>");
                }
                else if (inline is Italic italic)
                {
                    sb.Append($"<i>{ConvertInlines(italic.Inlines)}</i>");
                }
                else if (inline is Underline underline)
                {
                    sb.Append($"<u>{ConvertInlines(underline.Inlines)}</u>");
                }
                else if (inline is Hyperlink link)
                {
                    string url = link.NavigateUri?.ToString() ?? "#";
                    sb.Append($"<a href=\"{url}\">{ConvertInlines(link.Inlines)}</a>");
                }
                else if (inline is Span span)
                {
                    sb.Append(ConvertInlines(span.Inlines));
                }
            }
            return sb.ToString();
        }
    }

}
