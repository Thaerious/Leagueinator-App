using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Leagueinator.Forms {
    /// <summary>
    /// Interaction logic for TableViewer.xaml
    /// </summary>
    public partial class TableViewer : Window {
        public TableViewer() {
            InitializeComponent();
        }

        public void Show(DataTable dataTable) {
            foreach (DataColumn col in dataTable.Columns) {
                DataGridTextColumn dgCol = new() {
                    Header = col.ColumnName,
                    Binding = new Binding($"[{col.ColumnName}]")
                };
                DataGrid.Columns.Add(dgCol);
            }

            List<dynamic> list = [];
            foreach (DataRow row in dataTable.Rows) {
                Dictionary<string, object> dictionary = [];
                foreach (DataColumn col in dataTable.Columns) {
                    dictionary[col.ColumnName] = row[col];
                }
                list.Add(dictionary);
            }

            this.DataGrid.ItemsSource = list;
            this.Show();
        }
    }
}
