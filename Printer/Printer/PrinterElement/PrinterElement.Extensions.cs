using System.Data;
using System.Diagnostics;

namespace Leagueinator.Printer
{
    public static class PrinterElementExtensions{

        public static void ApplyRow(this PrinterElement element, DataRow row, Action<PrinterElement, string, object> action) {
            foreach (DataColumn col in row.Table.Columns) {
                var item = row[col];
                if (item is null) continue;
                action(element, col.ColumnName, item);
            }
        }

        /// <summary>
        /// For each column in the specified row, find the element with a matching
        /// id attribute and set it's Text to the column value;
        /// </summary>
        /// <param name="element"></param>
        /// <param name="row"></param>
        public static void ApplyRowAsText(this PrinterElement element, DataRow row) {
            element.ApplyRow(row, (element, col, value) => {
                var child = element.Children.QuerySelector($"#{col}");                
                if (child is null) return;
                child.InnerText = value.ToString();
            });
        }
    }
}
