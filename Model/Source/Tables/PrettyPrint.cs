using System.Data;
using System.Text;

namespace Leagueinator.Model.Tables {

    public static class PrettyPrintExtensions {

        public static string PrettyPrint(this object @object, string? title = null) {
            if (@object is DataTable dataTable) {
                return PrettyPrint(dataTable, title);
            }
            else if (@object is DataView dataView) {
                return PrettyPrint(dataView.ToTable(), title);
            }
            else if (@object is CustomRow customRow) {
                return PrettyPrint(customRow, title);
            }
            else if (@object is IEnumerable<CustomRow> collection) {
                return PrettyPrint(collection, title);
            }

            throw new NotSupportedException($"Can not pretty print object of type '{@object.GetType()}'");
        }

        private static string PrettyPrint(CustomRow customRow, string? title = null) {
            title = title ?? $"{customRow.GetType()}";
            DataTable table = customRow.DataRow.Table;
            List<DataRow> rowList = [customRow.DataRow];
            return PrettyPrint(title, table.Columns.Cast<DataColumn>().ToArray(), [.. rowList]);
        }

        private static string PrettyPrint(this IEnumerable<CustomRow> customRows, string? title = null) {
            title = title ?? $"{customRows.GetType()}";
            DataTable table = customRows.First().DataRow.Table;
            List<DataRow> rowList = [];
            foreach (CustomRow row in customRows) {
                if (row.DataRow.Table != table) throw new NotSupportedException("Tables must be the same for all rows");
                rowList.Add(row.DataRow);
            }

            return PrettyPrint(title, table.Columns.Cast<DataColumn>().ToArray(), [.. rowList]);
        }

        private static string PrettyPrint(DataTable table, string? title = null) {
            DataRowCollection rowCollection = table.Rows;
            DataRow[] rowArray = new DataRow[rowCollection.Count];
            rowCollection.CopyTo(rowArray, 0);

            return PrettyPrint(
                $"Table: '{table.TableName}'",
                table.Columns.Cast<DataColumn>().ToArray(),
                table.Rows.Cast<DataRow>().ToArray()
            );
        }

        private static string PrettyPrint(string title, DataColumn[] columns, DataRow[] rows) {
            var sb = new StringBuilder();

            Dictionary<DataColumn, int> colSizes = [];

            foreach (DataColumn column in columns) {
                colSizes[column] = column.ColumnName.Length;
            }

            while (colSizes.Sum(col => col.Value) < title.Length) {
                foreach (DataColumn key in colSizes.Keys) colSizes[key]++;
            }

            foreach (DataRow row in rows) {
                foreach (DataColumn column in columns) {
                    string value = row[column].ToString() ?? "";
                    colSizes[column] = Math.Max(value.Length, colSizes[column]);
                }
            }

            sb.Append('+');
            foreach (DataColumn column in columns) {
                sb.Append(new string('-', colSizes[column] + 2));
                sb.Append('+');
            }

            // Set Headers
            int headerSize = -1;
            sb.Append("\n| ");
            foreach (DataColumn column in columns) {
                string value = column.ColumnName;
                value = value.PadLeft((colSizes[column] + value.Length) / 2);
                value = value.PadRight(colSizes[column]);
                sb.Append(value);
                sb.Append(" | ");
                headerSize += value.Length + 3;
            }

            headerSize = Math.Max(headerSize, 5);
            string[] splitTitle = title.Split('\n');

            foreach (string split in splitTitle.Reverse()) {
                var line = split;
                line = line.PadLeft((headerSize / 2) + (line.Length / 2));
                line = line.PadRight(headerSize);
                sb.Insert(0, $"|{line}|\n");
            }
            sb.Insert(0, "+" + new string('-', headerSize) + "+\n");

            sb.Append("\n+");
            foreach (DataColumn column in columns) {
                sb.Append(new string('-', colSizes[column] + 2));
                sb.Append('+');
            }

            foreach (DataRow row in rows) {
                sb.Append("\n| ");
                foreach (DataColumn column in columns) {
                    string s = row[column.ColumnName].ToString() ?? "NULL";
                    sb.Append(s.PadLeft(colSizes[column]));
                    sb.Append(" | ");
                }
            }

            sb.Append("\n+");
            foreach (DataColumn column in columns) {
                sb.Append(new string('-', colSizes[column] + 2));
                sb.Append('+');
            }

            sb.Append("\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns all records from the left TeamTable, and the matching records from 
        /// the right TeamTable(table2).  Records are merged when leftCol equals rightCol.
        /// </summary>
        public static DataTable LeftJoin<T>(this DataTable left, DataTable right, string leftCol, string rightCol) {
            if (left.Columns[leftCol] == null) throw new KeyNotFoundException(leftCol);
            if (right.Columns[rightCol] == null) throw new KeyNotFoundException(rightCol);

            var newTable = new DataTable().MergeWith(left, right);

            var query = from row1 in left.AsEnumerable()
                        join row2 in right.AsEnumerable()
                        on row1.Field<T>(leftCol) equals row2.Field<T>(rightCol) into enumerable
                        from subRow in enumerable
                        select new {
                            leftRow = row1,
                            rightRow = subRow
                        };

            foreach (var record in query) {
                var row = newTable.NewRow();
                foreach (DataColumn col in left.Columns) {
                    row[$"{left.TableName}.{col.ColumnName}"] = record.leftRow[col.ColumnName];
                }
                foreach (DataColumn col in right.Columns) {
                    row[$"{right.TableName}.{col.ColumnName}"] = record.rightRow[col.ColumnName];
                }

                newTable.Rows.Add(row);
            }

            return newTable;
        }

        public static DataTable MergeWith(this DataTable target, params DataTable[] tables) {
            foreach (var table in tables) {
                foreach (DataColumn column in table.Columns) {
                    DataColumn newCol = new() {
                        DataType = column.DataType,
                        ColumnName = $"{table.TableName}.{column.ColumnName}"
                    };
                    target.Columns.Add(newCol);
                }
            }

            return target;
        }

        public static DataTable MergeWith(this DataTable target, Func<DataColumn, string> nameFunc, params DataTable[] tables) {
            foreach (var table in tables) {
                foreach (DataColumn column in table.Columns) {
                    DataColumn newCol = new() {
                        DataType = column.DataType,
                        ColumnName = nameFunc(column),
                        DefaultValue = column.DefaultValue,
                    };
                    target.Columns.Add(newCol);
                }
            }

            return target;
        }

        public static DataTable As(this DataTable table, params string[] names) {
            var newTable = table.Clone();

            for (int i = 0; i < names.Length; i++) {
                newTable.Columns[i].ColumnName = names[i];
            }

            return newTable;
        }
    }
}
