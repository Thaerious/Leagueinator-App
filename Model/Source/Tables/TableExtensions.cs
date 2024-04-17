﻿using System.Data;
using System.Text;

namespace Leagueinator.Model.Tables {

    public static class TableExtensions {

        public static string BuildRowFilter(DataColumn[] fkCol, object[] fkVal) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fkCol.Length; i++) {
                if (fkVal.GetType() == typeof(string)) {
                    sb.Append($"{fkCol[i].ColumnName} = '{fkVal[i]}' ");
                }
                else {
                    sb.Append($"{fkCol[i].ColumnName} = {fkVal[i]} ");
                }
                if (i < fkCol.Length - 1) sb.Append(" AND ");
            }
            return sb.ToString();
        }

        public static string BuildRowFilter(string[] fkCol, object[] fkVal) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fkCol.Length; i++) {
                if (fkVal.GetType() == typeof(string)) {
                    sb.Append($"{fkCol[i]} = '{fkVal[i]}' ");
                }
                else {
                    sb.Append($"{fkCol[i]} = {fkVal[i]} ");
                }
                if (i < fkCol.Length - 1) sb.Append(" AND ");
            }
            return sb.ToString();
        }

        public static bool Has<TYPE>(this DataTable table, string column, TYPE value) {
            return table.AsEnumerable()
                .Where(row => row[column].Equals(value))
                .Any();
        }

        public static bool Has(this DataTable table, string[] column, object[] value) {
            DataView dataView = new DataView(table) {
                RowFilter = TableExtensions.BuildRowFilter(column, value)
            };
            return dataView.Count > 0;
        }

        public static bool Has<TYPE>(this DataView view, string column, TYPE value) {
            foreach (DataRowView row in view) {
                if (row[column].Equals(value)) return true;
            }
            return false;
        }

        public static DataRow? Get<TYPE>(this DataView view, string column, TYPE value) {
            foreach (DataRowView row in view) {
                if (row[column].Equals(value)) return row.Row;
            }
            return null;
        }

        public static void CopyTo(this CustomRow source, DataRow target) {
            source.DataRow.CopyTo(target);
        }

        public static void CopyTo(this DataRow source, DataRow target) {
            foreach (DataColumn col in source.Table.Columns) {
                if (target.Table.Columns[col.ColumnName] == null) continue;
                target[col.ColumnName] = source[col.ColumnName];
            }
        }

        public static DataRow Clone(this DataRow source) {
            var dest = source.Table.NewRow();

            for (int i = 0; i < source.ItemArray.Length; i++) {
                dest[i] = source[i];
            }

            return dest;
        }

        /// <summary>
        /// Extract a specific column as a list of values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static List<T> ColValues<T>(this DataTable table, string column) {
            if (!table.Columns.Contains(column)) throw new KeyNotFoundException(column);

            var list = new List<T>();

            foreach (DataRow row in table.Rows) {
                list.Add(row.Field<T>(column)!);
            }

            return list;
        }

        /// <summary>
        /// Extract a specific column as a list of values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static List<T> ColValues<T>(this DataView view, string column) {
            if (view.Table == null) throw new NullReferenceException();
            if (!view.Table.Columns.Contains(column)) throw new KeyNotFoundException(column);

            var list = new List<T>();

            foreach (DataRowView row in view) {
                list.Add(row.Row.Field<T>(column)!);
            }

            return list;
        }

        /// <summary>
        /// Retrieve a enumerable of all values for a specified column.
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
            DataView clone = new DataView {
                // Copy the properties from the original DataView
                Table = view.Table,
                RowFilter = view.RowFilter,
                Sort = view.Sort,
                RowStateFilter = view.RowStateFilter
            };

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

        public static string PrettyPrint(this DataView view, string? title = null) {
            title ??= $"View of Table '{view.Table!.TableName}'\n" +
                      $"{view.RowFilter}";

            return TableExtensions.PrettyPrint(view.ToTable(), title);
        }

        public static string PrettyPrint(this DataTable Table, DataRow row, string? title = null) {
            return Table.PrettyPrint(new DataRow[] { row }, title);
        }

        public static string PrettyPrint(this DataRow row, string? title = null) {
            return row.Table.PrettyPrint(new DataRow[] { row }, title);
        }

        public static string PrettyPrint(this DataTable Table, DataRow[] rows, string? title = null) {
            title ??= $"Table: '{Table.TableName}'";
            var sb = new StringBuilder();

            Dictionary<DataColumn, int> colSizes = [];

            foreach (DataColumn column in Table.Columns) {
                colSizes[column] = column.ColumnName.Length;
            }

            foreach (DataRow row in rows) {
                foreach (DataColumn column in Table.Columns) {
                    string value = row[column].ToString() ?? "";
                    colSizes[column] = Math.Max(value.Length, colSizes[column]);
                }
            }

            sb.Append('+');
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', colSizes[column] + 2));
                sb.Append('+');
            }

            int headerSize = -1;
            sb.Append("\n| ");
            foreach (DataColumn column in Table.Columns) {
                string value = column.ColumnName.PadLeft(colSizes[column]);
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
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', colSizes[column] + 2));
                sb.Append('+');
            }

            foreach (DataRow row in rows) {
                sb.Append("\n| ");
                foreach (DataColumn column in Table.Columns) {
                    string s = row[column.ColumnName].ToString() ?? "NULL";
                    sb.Append(s.PadLeft(colSizes[column]));
                    sb.Append(" | ");
                }
            }

            sb.Append("\n+");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', colSizes[column] + 2));
                sb.Append('+');
            }

            sb.Append("\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns all records from the left ChildTable, and the matching records from 
        /// the right ChildTable(table2).  Records are merged when leftCol equals rightCol.
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