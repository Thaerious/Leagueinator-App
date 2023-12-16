using System.Data;
using System.Diagnostics;
using System.Text;

namespace Model.Tables {

    public static class Extensions {

        /// <summary>
        /// Retrieve a list of all values for a specified column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IEnumerable<T?> ColumnValues<T>(this DataTable table, string column) {
            return table.AsEnumerable().Select(row => row.Field<T>(column));
        }

        public static List<string> ColumnNames(this DataTable table) {
            List<string> list = [];
            foreach (DataColumn column in table.Columns) list.Add(column.ColumnName);
            return list;
        }

        public static DataView Clone(this DataView view) {
            // Create a new DataView
            DataView clone = new DataView();

            // Copy the properties from the original DataView
            clone.Table = view.Table;
            clone.RowFilter = view.RowFilter;
            clone.Sort = view.Sort;
            clone.RowStateFilter = view.RowStateFilter;

            // Return the clone
            return clone;
        }

        public static string PrettyPrint(this DataTable Table, string? title = null) {
            DataRowCollection rowCollection = Table.Rows;
            DataRow[] rowArray = new DataRow[rowCollection.Count];
            rowCollection.CopyTo(rowArray, 0);
            return Table.PrettyPrint(rowArray, title);
        }

        public static string PrettyPrint(this DataTable Table, DataRowCollection rowCollection, string? title = null) {
            DataRow[] rowArray = new DataRow[rowCollection.Count];
            rowCollection.CopyTo(rowArray, 0);
            return Table.PrettyPrint(rowArray, title);
        }

        public static string PrettyPrint(this DataTable Table, DataView view, string? title = null) {
            return Extensions.PrettyPrint(view.ToTable(), title);
        }

        public static string PrettyPrint(this DataTable Table, DataRow row, string? title = null) {
            return Table.PrettyPrint(new DataRow[] { row }, title);
        }

        public static string PrettyPrint(this DataTable Table, DataRow[] rows, string? title = null) {
            title ??= Table.TableName;
            var sb = new StringBuilder();

            sb.Append('+');
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', column.ColumnName.Length + 2));
                sb.Append('+');
            }

            int headerSize = -1;
            sb.Append("\n| ");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(column.ColumnName);
                sb.Append(" | ");
                headerSize += column.ColumnName.Length + 3;
            }
            Debug.WriteLine("Header Size " + headerSize);
            title = title.PadLeft((headerSize / 2) + (title.Length / 2));
            title = title.PadRight(headerSize);
            sb.Insert(0, $"|{title}|\n");
            sb.Insert(0, "+" + new string('-', headerSize) + "+\n");

            sb.Append("\n+");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', column.ColumnName.Length + 2));
                sb.Append('+');
            }

            foreach (DataRow row in rows) {
                sb.Append("\n| ");
                foreach (DataColumn column in Table.Columns) {
                    string s = row[column.ColumnName].ToString() ?? "NULL";
                    sb.Append(s.PadLeft(column.ColumnName.Length));
                    sb.Append(" | ");
                }
            }

            sb.Append("\n+");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', column.ColumnName.Length + 2));
                sb.Append('+');
            }

            return sb.ToString();
        }
    }
}
