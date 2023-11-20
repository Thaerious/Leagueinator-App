using Leagueinator.Model;
using Leagueinator_Utility.Utility;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.App.Forms.Report {

    /// <summary>
    /// Display the bowls break down and summary of an event.
    /// Invoke #InitColumns to initialize the form.
    /// Invoke #AddRow to add a data point.
    /// </summary>
    public partial class FormReport : Form {
        private MirrorMap<object, DataGridViewRow> mirrorMap = new();
        public delegate List<object> RowGenerator();
        private RowGenerator RowGeneratorCB;
        private bool inRefresh = false;

        public FormReport(RowGenerator rowGenerator) {
            this.InitializeComponent();
            this.RowGeneratorCB = rowGenerator;
        }

        private void OnLoad(object sender, EventArgs e) {
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.CellValueChanged += this.DataGridView_CellValueChanged;
        }

        private void DataGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e) {
            if (this.inRefresh) return;

            var row = this.dataGridView.Rows[e.RowIndex];
            var col = this.dataGridView.Columns[e.ColumnIndex];
            var source = this.mirrorMap.LookupKey(row);

            PropertyInfo? propInfo = source.GetType().GetProperty(col.Name);
            var value = this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (propInfo == null) return;
            propInfo.SetValue(source, value, null);

            this.RefreshAll();
        }

        public void RefreshAll() {
            this.inRefresh = true;
            this.dataGridView.Rows.Clear();

            foreach (object? o in this.RowGeneratorCB()) {
                if (o is null) continue;
                this.AddRow(o);
            }

            this.inRefresh = false;
        }

        /// Summary:
        //     Adds a new row to the form.
        //     Will use public properties to determine the cell behaviour and values.
        //     If there is not a public setter then the cell is read only.
        //     If there is no property, or public getter with a matching name then the cell is disabled.
        //
        // Returns:
        //     The index of the new row.
        ///
        public int AddRow(Object source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (this.mirrorMap.ContainsKey(source)) return -1;

            Type type = source.GetType();
            int rowIndex = this.dataGridView.Rows.Add();
            DataGridViewRow row = this.dataGridView.Rows[rowIndex];
            this.mirrorMap[source] = row;

            foreach (DataGridViewColumn col in this.dataGridView.Columns) {
                PropertyInfo? propInfo = type.GetProperty(col.Name);
                DataGridViewCell cell = row.Cells[col.Index];

                // Disable cell when property isn't found.
                if (propInfo == null || propInfo.GetGetMethod() == null) {
                    cell.ReadOnly = true;
                    cell.Style.BackColor = Color.FromArgb(230, 230, 230);
                    continue;
                };

                // Readonly when Editable(false);
                EditableAttribute[] attrs = (EditableAttribute[])propInfo.GetCustomAttributes(typeof(EditableAttribute), true);
                bool isEditable = attrs.Length <= 0 ? true : attrs[0].Value;

                // Read only when Setter isn't found.
                if (propInfo.GetSetMethod() == null || !isEditable) {
                    cell.ReadOnly = true;
                    cell.Style.ForeColor = Color.Black;
                } else {
                    cell.Style.ForeColor = Color.Blue;
                    cell.Style.BackColor = Color.FromArgb(223, 242, 249);
                    cell.Style.Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
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

            for (int c = 0; c < names.Length; c++) {
                this.dataGridView.Columns[c].HeaderText = labels[c];
                this.dataGridView.Columns[c].Name = names[c];
                this.dataGridView.Columns[c].ReadOnly = false;
                this.dataGridView.Columns[c].ValueType = typeof(int);
                this.dataGridView.Columns[c].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (widths != null) {
                    this.dataGridView.Columns[c].Width = widths[c];
                }
            }
        }
    }
}
