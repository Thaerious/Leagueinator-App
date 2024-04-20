namespace Leagueinator.Components {
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
            this.membersGrid = new DataGridView();
            this.flowLayoutPanel = new FlowLayoutPanel();
            this.label1 = new Label();
            this.txtEnds = new TextBox();
            this.teamsGrid = new DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.membersGrid).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.teamsGrid).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.membersGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.teamsGrid, 2, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new Size(726, 279);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // membersGrid
            // 
            this.membersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.membersGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.membersGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.membersGrid.Dock = DockStyle.Fill;
            this.membersGrid.Location = new Point(3, 3);
            this.membersGrid.Name = "membersGrid";
            this.membersGrid.RowHeadersWidth = 62;
            this.membersGrid.Size = new Size(284, 273);
            this.membersGrid.TabIndex = 0;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.label1);
            this.flowLayoutPanel.Controls.Add(this.txtEnds);
            this.flowLayoutPanel.Dock = DockStyle.Fill;
            this.flowLayoutPanel.Location = new Point(293, 3);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new Size(139, 273);
            this.flowLayoutPanel.TabIndex = 2;
            // 
            // LabelPage
            // 
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ends";
            // 
            // txtEnds
            // 
            this.txtEnds.AcceptsReturn = true;
            this.txtEnds.Location = new Point(3, 28);
            this.txtEnds.Name = "txtEnds";
            this.txtEnds.Size = new Size(136, 31);
            this.txtEnds.TabIndex = 1;
            this.txtEnds.TextChanged += this.TxtEndsChangedHnd;
            // 
            // teamsGrid
            // 
            this.teamsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.teamsGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.teamsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.teamsGrid.Dock = DockStyle.Fill;
            this.teamsGrid.Location = new Point(438, 3);
            this.teamsGrid.Name = "teamsGrid";
            this.teamsGrid.RowHeadersWidth = 62;
            this.teamsGrid.Size = new Size(285, 273);
            this.teamsGrid.TabIndex = 1;
            // 
            // MatchCard
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MatchCard";
            this.Size = new Size(726, 279);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.membersGrid).EndInit();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.teamsGrid).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel;
        private Label label1;
        private TextBox txtEnds;
        private DataGridView teamsGrid;
        public DataGridView membersGrid;
    }
}
