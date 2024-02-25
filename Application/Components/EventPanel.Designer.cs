namespace Leagueinator.Components {
    partial class EventPanel : UserControl {
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
            this.rootLayout = new TableLayoutPanel();
            this.flowIdlePlayers = new FlowLayoutPanel();
            this.layoutRounds = new TableLayoutPanel();
            this.butAddRound = new Button();
            this.butRemoveRound = new Button();
            this.flowRounds = new FlowLayoutPanel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.matchCard1 = new MatchCard();
            this.rootLayout.SuspendLayout();
            this.layoutRounds.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.ColumnCount = 3;
            this.rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.rootLayout.Controls.Add(this.flowIdlePlayers, 0, 0);
            this.rootLayout.Controls.Add(this.layoutRounds, 0, 0);
            this.rootLayout.Controls.Add(this.flowLayoutPanel1, 2, 0);
            this.rootLayout.Dock = DockStyle.Fill;
            this.rootLayout.Location = new Point(0, 0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.RowCount = 1;
            this.rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.rootLayout.Size = new Size(1564, 896);
            this.rootLayout.TabIndex = 0;
            // 
            // flowIdlePlayers
            // 
            this.flowIdlePlayers.AutoScroll = true;
            this.flowIdlePlayers.Dock = DockStyle.Fill;
            this.flowIdlePlayers.FlowDirection = FlowDirection.TopDown;
            this.flowIdlePlayers.Location = new Point(315, 3);
            this.flowIdlePlayers.Name = "flowIdlePlayers";
            this.flowIdlePlayers.Size = new Size(306, 890);
            this.flowIdlePlayers.TabIndex = 2;
            this.flowIdlePlayers.WrapContents = false;
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
            this.layoutRounds.Size = new Size(306, 888);
            this.layoutRounds.TabIndex = 1;
            // 
            // butAddRound
            // 
            this.butAddRound.Dock = DockStyle.Fill;
            this.butAddRound.Location = new Point(4, 740);
            this.butAddRound.Margin = new Padding(3, 4, 3, 4);
            this.butAddRound.Name = "butAddRound";
            this.butAddRound.Size = new Size(298, 54);
            this.butAddRound.TabIndex = 0;
            this.butAddRound.Text = "Add Round";
            this.butAddRound.UseVisualStyleBackColor = true;
            // 
            // butRemoveRound
            // 
            this.butRemoveRound.Dock = DockStyle.Fill;
            this.butRemoveRound.Location = new Point(4, 803);
            this.butRemoveRound.Margin = new Padding(3, 4, 3, 4);
            this.butRemoveRound.Name = "butRemoveRound";
            this.butRemoveRound.Size = new Size(298, 54);
            this.butRemoveRound.TabIndex = 1;
            this.butRemoveRound.Text = "Remove";
            this.butRemoveRound.UseVisualStyleBackColor = true;
            // 
            // flowRounds
            // 
            this.flowRounds.Dock = DockStyle.Fill;
            this.flowRounds.Location = new Point(4, 5);
            this.flowRounds.Margin = new Padding(3, 4, 3, 4);
            this.flowRounds.Name = "flowRounds";
            this.flowRounds.Size = new Size(298, 726);
            this.flowRounds.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = Color.FromArgb(255, 192, 192);
            this.flowLayoutPanel1.Controls.Add(this.matchCard1);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new Point(627, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(934, 890);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.Layout += this.LayoutEventHander;
            // 
            // matchCard1
            // 
            this.matchCard1.Lane = 0;
            this.matchCard1.Location = new Point(3, 3);
            this.matchCard1.Name = "matchCard1";
            this.matchCard1.Size = new Size(931, 171);
            this.matchCard1.TabIndex = 0;
            // 
            // EventPanel
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.rootLayout);
            this.Name = "EventPanel";
            this.Size = new Size(1564, 896);
            this.rootLayout.ResumeLayout(false);
            this.layoutRounds.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private TableLayoutPanel layoutRounds;
        private Button butAddRound;
        private Button butRemoveRound;
        private FlowLayoutPanel flowRounds;
        private FlowLayoutPanel flowIdlePlayers;
        private FlowLayoutPanel flowLayoutPanel1;
        public MatchCard matchCard1;
    }
}
