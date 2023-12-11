using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator_Model.Model.Tables {


    public static class Extensions {

        public static string ToString(this DataTable Table, string? title = null) {
            DataRowCollection rowCollection = Table.Rows;
            DataRow[] rowArray = new DataRow[rowCollection.Count];
            rowCollection.CopyTo(rowArray, 0);
            return Table.ToString(rowArray, title);
        }

        public static string ToString(this DataTable Table, DataRowCollection rowCollection, string? title = null) {            
            DataRow[] rowArray = new DataRow[rowCollection.Count];
            rowCollection.CopyTo(rowArray, 0);
            return Table.ToString(rowArray, title);
        }

        public static string ToString(this DataTable Table, DataRow[] rows, string? title = null) {
            title ??= Table.TableName;
            var sb = new StringBuilder();


            sb.Append("+");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', column.ColumnName.Length + 2));
                sb.Append("+");
            }

            int headerSize = -1;
            sb.Append("\n| ");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(column.ColumnName);
                sb.Append(" | ");
                headerSize += column.ColumnName.Length + 3;
            }

            title = title.PadLeft((headerSize / 2) + (title.Length / 2));
            title = title.PadRight(headerSize);
            sb.Insert(0, $"|{title}|\n");
            sb.Insert(0, "+" + new string('-', headerSize) + "+\n");

            sb.Append("\n+");
            foreach (DataColumn column in Table.Columns) {
                sb.Append(new string('-', column.ColumnName.Length + 2));
                sb.Append("+");
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
                sb.Append("+");
            }

            return sb.ToString();
        }
    }

    public abstract class ATable {
        public DataTable Table { get; init; } = new();

        public override string ToString() {
            return Table.ToString(this.Table.Rows);
        }
    }
}
