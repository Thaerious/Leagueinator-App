using System.Data;

namespace Leagueinator.Printer {
    public static class PrinterElementExtensions {

        public static int ApplyRow(this Element element, DataRow row, Action<Element, string, object> action) {
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
        public static int ApplyRowAsText(this Element element, DataRow row) {
            int count = 0;

            element.ApplyRow(row, (element, col, value) => {
                var child = element.Children.Query($"#{col}");
                if (child is null) return;
                child.InnerText = value.ToString();
                count++;
            });

            return count;
        }
    }
}
