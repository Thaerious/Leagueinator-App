namespace Application.Components {
    partial class MatchCard {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.dataGridView1 = new DataGridView();
            this.dataGridView2 = new DataGridView();
            this.flowLayoutPanel = new FlowLayoutPanel();
            this.txtBowls = new TextBox();
            this.txtEnds = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView2).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView2, 2, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new Size(726, 279);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.Size = new Size(284, 273);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = DockStyle.Fill;
            this.dataGridView2.Location = new Point(438, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.Size = new Size(285, 273);
            this.dataGridView2.TabIndex = 1;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.label2);
            this.flowLayoutPanel.Controls.Add(this.txtBowls);
            this.flowLayoutPanel.Controls.Add(this.label1);
            this.flowLayoutPanel.Controls.Add(this.txtEnds);
            this.flowLayoutPanel.Dock = DockStyle.Fill;
            this.flowLayoutPanel.Location = new Point(293, 3);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new Size(139, 273);
            this.flowLayoutPanel.TabIndex = 2;
            // 
            // txtBowls
            // 
            this.txtBowls.Location = new Point(3, 28);
            this.txtBowls.Name = "txtBowls";
            this.txtBowls.Size = new Size(136, 31);
            this.txtBowls.TabIndex = 0;
            // 
            // txtEnds
            // 
            this.txtEnds.AcceptsReturn = true;
            this.txtEnds.Location = new Point(3, 90);
            this.txtEnds.Name = "txtEnds";
            this.txtEnds.Size = new Size(136, 31);
            this.txtEnds.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ends";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(58, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bowls";
            // 
            // MatchCard
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MatchCard";
            this.Size = new Size(726, 279);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView2).EndInit();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private FlowLayoutPanel flowLayoutPanel;
        private Label label2;
        private TextBox txtBowls;
        private Label label1;
        private TextBox txtEnds;
    }
}
