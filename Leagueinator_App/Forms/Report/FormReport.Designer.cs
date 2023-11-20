using System.Drawing;
using System.Windows.Forms;

namespace Leagueinator.App.Forms.Report {
    partial class FormReport {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            dataGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(0, 0);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 62;
            dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView.Size = new Size(973, 802);
            dataGridView.TabIndex = 0;
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(973, 802);
            this.Controls.Add(dataGridView);
            this.Name = "FormReport";
            this.Text = "Form1";
            this.Load += this.OnLoad;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView;
    }
}
