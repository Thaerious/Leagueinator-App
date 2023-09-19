using Leagueinator.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Leagueinator.App.Forms.Report {
    /// <summary>
    /// Display the bowls break down and summary of an event.
    /// Invoke #InitColumns to initialize the form.
    /// Invoke #AddRow to add a data point.
    /// </summary>
    public partial class FormReport : Form {
        private List<object> sources = new List<object>();
        private bool inRefresh = false;

        public FormReport() {
            this.InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e) {
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.CellValueChanged += this.DataGridView_CellValueChanged;
        }

        private void DataGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e) {
            if (this.inRefresh) return;

            var col = this.dataGridView.Columns[e.ColumnIndex];
            var source = this.dataGridView.Rows[e.RowIndex].Cells[0].Value;

            PropertyInfo propInfo = source.GetType().GetProperty(col.Name);
            var value = this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            propInfo.SetValue(source, value, null);

            this.RefreshAll();
        }

        private void RefreshAll() {
            this.inRefresh = true;
            foreach (DataGridViewRow row in this.dataGridView.Rows) {
                (row.Cells[0].Value as HasRefresh).Refresh();
                this.RefreshRow(row);
            }
            this.inRefresh = false;
        }

        private void RefreshRow(DataGridViewRow row) {
            var source = row.Cells[0].Value;

            foreach (DataGridViewColumn col in this.dataGridView.Columns) {
                PropertyInfo propInfo = source.GetType().GetProperty(col.Name);
                if (propInfo == null) continue;
                DataGridViewCell cell = row.Cells[col.Index];
                cell.Value = propInfo.GetValue(source);
            }
        }

        /// Summary:
        //     Adds a new row to the form.
        //     Will use public properties to determine the cell behaviour and values.
        //     If there is not a public setter then the cell is read only.
        //     If there is no property, or public getter, than the cell is disabled.
        //
        // Returns:
        //     The index of the new row.
        ///
        public int AddRow(Object source) {
            int rowIndex = this.dataGridView.Rows.Add();

            DataGridViewRow row = this.dataGridView.Rows[rowIndex];

            row.Cells[0].Value = source;

            foreach (DataGridViewColumn col in this.dataGridView.Columns) {
                PropertyInfo propInfo = source.GetType().GetProperty(col.Name);
                DataGridViewCell cell = this.dataGridView.Rows[rowIndex].Cells[col.Index];

                if (propInfo == null || propInfo.GetGetMethod() == null) {
                    cell.ReadOnly = true;
                    cell.Style.BackColor = System.Drawing.Color.Gray;
                    continue;
                };


                if (propInfo.GetSetMethod() == null) {
                    cell.ReadOnly = true;
                    cell.Style.ForeColor = System.Drawing.Color.Blue;
                }

                cell.Value = propInfo.GetValue(source);
            }

            return rowIndex;
        }

        public void InitColumns(string[] names, string[] labels = null, int[] widths = null) {
            if (labels == null) labels = names;

            this.dataGridView.Columns.Clear();
            this.dataGridView.Rows.Clear();
            this.dataGridView.ColumnCount = names.Length + 1;

            this.dataGridView.Columns[0].HeaderText = ".row";
            this.dataGridView.Columns[0].Name = ".row";
            this.dataGridView.Columns[0].ReadOnly = false;
            this.dataGridView.Columns[0].ValueType = typeof(object);
            this.dataGridView.Columns[0].Visible = false;

            for (int c = 0; c < names.Length; c++) {
                this.dataGridView.Columns[c + 1].HeaderText = labels[c];
                this.dataGridView.Columns[c + 1].Name = names[c];
                this.dataGridView.Columns[c + 1].ReadOnly = false;
                this.dataGridView.Columns[c + 1].ValueType = typeof(int);

                if (widths != null) {
                    this.dataGridView.Columns[c + 1].Width = widths[c];
                }
            }
        }
    }
}
