using System.Data;
using System.Text;

namespace Leagueinator.Utility {
    public static class DataTableExtensions {

        public static string DelString(this DataTable dataTable) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(dataTable.TableName);

            foreach (DataRowView rowView in dataTable.DefaultView) {
                sb.AppendLine(rowView.Row.ItemArray.DelString(", "));
            }

            sb.AppendLine($"row count {dataTable.Rows.Count}");
            return sb.ToString();
        }

    }
}
