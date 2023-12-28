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
            this.components = new System.ComponentModel.Container();
            this.layoutRounds = new TableLayoutPanel();
            this.butAddRound = new Button();
            this.butRemoveRound = new Button();
            this.flowRounds = new FlowLayoutPanel();
            this.playerListBox = new PlayerListBox(this.components);
            this.splitContainer1 = new SplitContainer();
            this.splitContainer2 = new SplitContainer();
            this.matchCardPanel = new MatchCardPanel();
            this.layoutRounds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.layoutRounds.Location = new Point(0, 0);
            this.layoutRounds.Margin = new Padding(3, 4, 3, 4);
            this.layoutRounds.Name = "layoutRounds";
            this.layoutRounds.RowCount = 4;
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            this.layoutRounds.Size = new Size(527, 939);
            this.layoutRounds.TabIndex = 0;
            // 
            // butAddRound
            // 
            this.butAddRound.Dock = DockStyle.Fill;
            this.butAddRound.Location = new Point(4, 791);
            this.butAddRound.Margin = new Padding(3, 4, 3, 4);
            this.butAddRound.Name = "butAddRound";
            this.butAddRound.Size = new Size(519, 54);
            this.butAddRound.TabIndex = 0;
            this.butAddRound.Text = "Add Round";
            this.butAddRound.UseVisualStyleBackColor = true;
            this.butAddRound.Click += this.HndAddRound;
            // 
            // butRemoveRound
            // 
            this.butRemoveRound.Dock = DockStyle.Fill;
            this.butRemoveRound.Location = new Point(4, 854);
            this.butRemoveRound.Margin = new Padding(3, 4, 3, 4);
            this.butRemoveRound.Name = "butRemoveRound";
            this.butRemoveRound.Size = new Size(519, 54);
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
            this.flowRounds.Size = new Size(519, 777);
            this.flowRounds.TabIndex = 2;
            // 
            // playerListBox
            // 
            this.playerListBox.Dock = DockStyle.Fill;
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.ItemHeight = 25;
            this.playerListBox.Location = new Point(0, 0);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new Size(350, 939);
            this.playerListBox.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.layoutRounds);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new Size(1583, 939);
            this.splitContainer1.SplitterDistance = 527;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.playerListBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.matchCardPanel);
            this.splitContainer2.Size = new Size(1052, 939);
            this.splitContainer2.SplitterDistance = 350;
            this.splitContainer2.TabIndex = 0;
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
            // EventPanel
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "EventPanel";
            this.Size = new Size(1583, 939);
            this.layoutRounds.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layoutRounds;
        private Button butAddRound;
        private Button butRemoveRound;
        private FlowLayoutPanel flowRounds;
        private PlayerListBox playerListBox;
        private MatchCard matchCard1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private MatchCardPanel matchCardPanel;
    }
}
