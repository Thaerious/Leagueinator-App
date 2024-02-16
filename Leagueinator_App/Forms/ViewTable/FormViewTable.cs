﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leagueinator_App.Forms.ViewTable {
    public partial class FormViewTable : Form {
        public FormViewTable() {
            InitializeComponent();
        }

        public void Show(DataTable table) {
            this.dataGridView1.DataSource = table;
            this.Visible = true;
        }
    }
}
