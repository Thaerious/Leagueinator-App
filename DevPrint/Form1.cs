using System.Data;
using System.Diagnostics;
using System.Text;

namespace DevPrint {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            var mockEvent = new MockEvent();
            var dataSet = mockEvent.ToDataSet();

            foreach (DataTable table in dataSet.Tables) Debug.WriteLine(table.TableName);

            foreach (DataRow row in dataSet.Tables["team"]!.Rows) {
                StringBuilder sb = new StringBuilder();
                foreach (DataColumn column in row.Table.Columns) {
                    sb.Append(column.ColumnName + ": " + row[column] + "; ");
                }
                Debug.WriteLine(sb.ToString());
            }
            Debug.WriteLine(dataSet.Tables["team"]!.Rows.Count + " rows");
        }
    }
}
