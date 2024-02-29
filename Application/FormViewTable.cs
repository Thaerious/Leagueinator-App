using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leagueinator.Components {
    public partial class FormViewTable : Form {
        public FormViewTable() {
            InitializeComponent();
        }

        public void Show(string title, DataTable table) {
            this.Text = title;
            this.dataGridView1.DataSource = table;
            this.Visible = true;
        }
    }
}
