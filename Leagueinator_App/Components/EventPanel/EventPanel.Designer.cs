namespace Leagueinator.App.Components {
    partial class EventPanel {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param TagName="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.layoutRounds = new TableLayoutPanel();
            this.butAddRound = new Button();
            this.butRemoveRound = new Button();
            this.flowRounds = new FlowLayoutPanel();
            this.flowIdlePlayers = new FlowLayoutPanel();
            this.matchCardPanel = new MatchCardPanel();
            this.tablePanel = new TableLayoutPanel();
            this.layoutRounds.SuspendLayout();
            this.tablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutRounds
            // 
            this.layoutRounds.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.layoutRounds.ColumnCount = 1;
            this.layoutRounds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.layoutRounds.Controls.Add(this.butAddRound, 0, 1);
            this.layoutRounds.Controls.Add(this.butRemoveRound, 0, 2);
            this.layoutRounds.Controls.Add(this.flowRounds, 0, 0);
            this.layoutRounds.Dock = DockStyle.Fill;
            this.layoutRounds.Location = new Point(3, 4);
            this.layoutRounds.Margin = new Padding(3, 4, 3, 4);
            this.layoutRounds.Name = "layoutRounds";
            this.layoutRounds.RowCount = 4;
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            this.layoutRounds.Size = new Size(294, 792);
            this.layoutRounds.TabIndex = 0;
            // 
            // butAddRound
            // 
            this.butAddRound.Dock = DockStyle.Fill;
            this.butAddRound.Location = new Point(4, 644);
            this.butAddRound.Margin = new Padding(3, 4, 3, 4);
            this.butAddRound.Name = "butAddRound";
            this.butAddRound.Size = new Size(286, 54);
            this.butAddRound.TabIndex = 0;
            this.butAddRound.Text = "Add Round";
            this.butAddRound.UseVisualStyleBackColor = true;
            this.butAddRound.Click += this.HndAddRound;
            // 
            // butRemoveRound
            // 
            this.butRemoveRound.Dock = DockStyle.Fill;
            this.butRemoveRound.Location = new Point(4, 707);
            this.butRemoveRound.Margin = new Padding(3, 4, 3, 4);
            this.butRemoveRound.Name = "butRemoveRound";
            this.butRemoveRound.Size = new Size(286, 54);
            this.butRemoveRound.TabIndex = 1;
            this.butRemoveRound.Text = "Remove";
            this.butRemoveRound.UseVisualStyleBackColor = true;
            this.butRemoveRound.Click += this.HndDeleteRound;
            // 
            // flowRounds
            // 
            this.flowRounds.Dock = DockStyle.Fill;
            this.flowRounds.Location = new Point(4, 5);
            this.flowRounds.Margin = new Padding(3, 4, 3, 4);
            this.flowRounds.Name = "flowRounds";
            this.flowRounds.Size = new Size(286, 630);
            this.flowRounds.TabIndex = 2;
            // 
            // flowIdlePlayers
            // 
            this.flowIdlePlayers.AutoScroll = true;
            this.flowIdlePlayers.Dock = DockStyle.Fill;
            this.flowIdlePlayers.FlowDirection = FlowDirection.TopDown;
            this.flowIdlePlayers.Location = new Point(303, 3);
            this.flowIdlePlayers.Name = "flowIdlePlayers";
            this.flowIdlePlayers.Size = new Size(294, 794);
            this.flowIdlePlayers.TabIndex = 1;
            this.flowIdlePlayers.WrapContents = false;
            // 
            // matchCardPanel
            // 
            this.matchCardPanel.AutoScroll = true;
            this.matchCardPanel.Dock = DockStyle.Fill;
            this.matchCardPanel.FlowDirection = FlowDirection.TopDown;
            this.matchCardPanel.Location = new Point(0, 0);
            this.matchCardPanel.Name = "matchCardPanel";
            this.matchCardPanel.Round = null;
            this.matchCardPanel.Size = new Size(698, 939);
            this.matchCardPanel.TabIndex = 0;
            this.matchCardPanel.WrapContents = false;
            // 
            // tablePanel
            // 
            this.tablePanel.ColumnCount = 3;
            this.tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            this.tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            this.tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500F));
            this.tablePanel.Controls.Add(this.layoutRounds, 0, 0);
            this.tablePanel.Controls.Add(this.flowIdlePlayers, 1, 0);
            this.tablePanel.Controls.Add(this.matchCardPanel, 2, 0);
            this.tablePanel.Dock = DockStyle.Fill;
            this.tablePanel.Location = new Point(0, 0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.RowCount = 1;
            this.tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tablePanel.Size = new Size(1100, 800);
            this.tablePanel.TabIndex = 1;
            // 
            // EventPanel
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tablePanel);
            this.DoubleBuffered = true;
            this.Margin = new Padding(3, 4, 3, 4);
            this.MaximumSize = new Size(1200, 800);
            this.MinimumSize = new Size(1100, 800);
            this.Name = "EventPanel";
            this.Size = new Size(1200, 800);
            this.layoutRounds.ResumeLayout(false);
            this.tablePanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layoutRounds;
        private Button butAddRound;
        private Button butRemoveRound;
        private FlowLayoutPanel flowRounds;
        private MatchCard matchCard1;
        private MatchCardPanel matchCardPanel;
        private FlowLayoutPanel flowIdlePlayers;
        private TableLayoutPanel tablePanel;
    }
}
