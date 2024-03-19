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
    }
}
