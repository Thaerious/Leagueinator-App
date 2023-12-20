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
            this.layoutRoot = new TableLayoutPanel();
            this.playerListBox = new PlayerListBox(this.components);
            this.flowMatchCards = new FlowLayoutPanel();
            this.matchCard1 = new MatchCard();
            this.layoutRounds.SuspendLayout();
            this.layoutRoot.SuspendLayout();
            this.flowMatchCards.SuspendLayout();
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
            this.layoutRounds.Dock = DockStyle.Left;
            this.layoutRounds.Location = new Point(3, 4);
            this.layoutRounds.Margin = new Padding(3, 4, 3, 4);
            this.layoutRounds.Name = "layoutRounds";
            this.layoutRounds.RowCount = 4;
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            this.layoutRounds.Size = new Size(244, 931);
            this.layoutRounds.TabIndex = 0;
            // 
            // butAddRound
            // 
            this.butAddRound.Dock = DockStyle.Fill;
            this.butAddRound.Location = new Point(4, 783);
            this.butAddRound.Margin = new Padding(3, 4, 3, 4);
            this.butAddRound.Name = "butAddRound";
            this.butAddRound.Size = new Size(236, 54);
            this.butAddRound.TabIndex = 0;
            this.butAddRound.Text = "Add Round";
            this.butAddRound.UseVisualStyleBackColor = true;
            this.butAddRound.Click += this.HndAddRound;
            // 
            // butRemoveRound
            // 
            this.butRemoveRound.Dock = DockStyle.Fill;
            this.butRemoveRound.Location = new Point(4, 846);
            this.butRemoveRound.Margin = new Padding(3, 4, 3, 4);
            this.butRemoveRound.Name = "butRemoveRound";
            this.butRemoveRound.Size = new Size(236, 54);
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
            this.flowRounds.Size = new Size(236, 769);
            this.flowRounds.TabIndex = 2;
            // 
            // layoutRoot
            // 
            this.layoutRoot.ColumnCount = 3;
            this.layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            this.layoutRoot.ColumnStyles.Add(new ColumnStyle());
            this.layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 640F));
            this.layoutRoot.Controls.Add(this.layoutRounds, 0, 0);
            this.layoutRoot.Controls.Add(this.playerListBox, 1, 0);
            this.layoutRoot.Controls.Add(this.flowMatchCards, 2, 0);
            this.layoutRoot.Dock = DockStyle.Fill;
            this.layoutRoot.Location = new Point(0, 0);
            this.layoutRoot.Margin = new Padding(3, 4, 3, 4);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 1;
            this.layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.layoutRoot.Size = new Size(1583, 939);
            this.layoutRoot.TabIndex = 1;
            // 
            // playerListBox
            // 
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.ItemHeight = 25;
            this.playerListBox.Location = new Point(303, 3);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new Size(294, 929);
            this.playerListBox.TabIndex = 2;
            // 
            // flowMatchCards
            // 
            this.flowMatchCards.Controls.Add(this.matchCard1);
            this.flowMatchCards.Location = new Point(603, 4);
            this.flowMatchCards.Margin = new Padding(3, 4, 3, 4);
            this.flowMatchCards.Name = "flowMatchCards";
            this.flowMatchCards.Size = new Size(694, 931);
            this.flowMatchCards.TabIndex = 1;
            // 
            // matchCard1
            // 
            this.matchCard1.BackColor = Color.Silver;
            this.matchCard1.BorderStyle = BorderStyle.FixedSingle;
            this.matchCard1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            this.matchCard1.Lane = 1;
            this.matchCard1.Location = new Point(3, 4);
            this.matchCard1.Margin = new Padding(3, 4, 3, 4);
            this.matchCard1.Match = null;
            this.matchCard1.MaximumSize = new Size(620, 110);
            this.matchCard1.MinimumSize = new Size(620, 110);
            this.matchCard1.Name = "matchCard1";
            this.matchCard1.Size = new Size(620, 110);
            this.matchCard1.TabIndex = 0;
            // 
            // EventPanel
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.layoutRoot);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "EventPanel";
            this.Size = new Size(1583, 939);
            this.layoutRounds.ResumeLayout(false);
            this.layoutRoot.ResumeLayout(false);
            this.flowMatchCards.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layoutRounds;
        private Button butAddRound;
        private Button butRemoveRound;
        private FlowLayoutPanel flowRounds;
        private TableLayoutPanel layoutRoot;
        private FlowLayoutPanel flowMatchCards;
        private PlayerListBox playerListBox;
        private MatchCard matchCard1;
    }
}
