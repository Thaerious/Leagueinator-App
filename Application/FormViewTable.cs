using System.Data;

namespace Leagueinator.Components {
    public partial class FormViewTable : Form {
        public FormViewTable() {
            this.InitializeComponent();
        }

        public void Show(string title, DataTable table) {
            this.Text = title;
            this.dataGridView1.DataSource = table;
            this.Visible = true;
        }
    }
}
