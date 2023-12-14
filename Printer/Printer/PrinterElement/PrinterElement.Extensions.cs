using Leagueinator.Utility;
using Leagueinator.Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Printer
{
    public static class PrinterElementExtensions{

        public static int ApplyRow(this PrinterElement element, DataRow row, Action<PrinterElement, string, object> action) {
            int count = 0;

            foreach (DataColumn col in row.Table.Columns) {
                var item = row[col];
                if (item is null) continue;
                action(element, col.ColumnName, item);
                count++;
            }

            return count;
        }

        /// <summary>
        /// For each column in the specified dataset row, find the element with a matching
        /// id attribute and set it's Text to the column value.
        /// 
        /// It calls the action on each non-null element of the row.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="row"></param>
        public static int ApplyRowAsText(this PrinterElement element, DataRow row) {
            int count = 0;

            element.ApplyRow(row, (element, col, value) => {
                var child = element.Children.Query($"#{col}");
                if (child is null) return;
                child.InnerText = value.ToString();
                count++;                
            });

            return count;
        }

        public static XMLStringBuilder LocXML(this PrinterElement ele) {
            XMLStringBuilder xml = new();

            xml.OpenTag(ele.TagName);

            xml.InlineTag("Container");
            xml.Attribute("w", ele.ContainerRect.Width);
            xml.Attribute("h", ele.ContainerRect.Height);
            xml.Attribute("x", ele.ContainerRect.X);
            xml.Attribute("y", ele.ContainerRect.Y);
            xml.CloseTag();

            xml.InlineTag("Content");
            xml.Attribute("w", ele.ContentRect.Width);
            xml.Attribute("h", ele.ContentRect.Height);
            xml.Attribute("x", ele.ContentRect.X);
            xml.Attribute("y", ele.ContentRect.Y);
            xml.CloseTag();

            foreach (PrinterElement child in ele.Children) {
                xml.AppendXML(child.LocXML());
            }

            xml.CloseTag();

            return xml;
        }
    }
}
