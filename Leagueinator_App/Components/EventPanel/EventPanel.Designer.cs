namespace Leagueinator.App.Components.EventPanel {
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
            layoutRounds = new TableLayoutPanel();
            butAddRound = new Button();
            butRemoveRound = new Button();
            flowRounds = new FlowLayoutPanel();
            layoutRoot = new TableLayoutPanel();
            this.playerListBox = new PlayerListBox.PlayerListBox(this.components);
            flowMatchCards = new FlowLayoutPanel();
            this.matchCard1 = new MatchCard.MatchCard();
            layoutRounds.SuspendLayout();
            layoutRoot.SuspendLayout();
            flowMatchCards.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutRounds
            // 
            layoutRounds.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            layoutRounds.ColumnCount = 1;
            layoutRounds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layoutRounds.Controls.Add(butAddRound, 0, 1);
            layoutRounds.Controls.Add(butRemoveRound, 0, 2);
            layoutRounds.Controls.Add(flowRounds, 0, 0);
            layoutRounds.Dock = DockStyle.Left;
            layoutRounds.Location = new Point(3, 4);
            layoutRounds.Margin = new Padding(3, 4, 3, 4);
            layoutRounds.Name = "layoutRounds";
            layoutRounds.RowCount = 4;
            layoutRounds.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            layoutRounds.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            layoutRounds.Size = new Size(244, 931);
            layoutRounds.TabIndex = 0;
            // 
            // butAddRound
            // 
            butAddRound.Dock = DockStyle.Fill;
            butAddRound.Location = new Point(4, 783);
            butAddRound.Margin = new Padding(3, 4, 3, 4);
            butAddRound.Name = "butAddRound";
            butAddRound.Size = new Size(236, 54);
            butAddRound.TabIndex = 0;
            butAddRound.Text = "AddChild";
            butAddRound.UseVisualStyleBackColor = true;
            butAddRound.Click += this.HndAddRound;
            // 
            // butRemoveRound
            // 
            butRemoveRound.Dock = DockStyle.Fill;
            butRemoveRound.Location = new Point(4, 846);
            butRemoveRound.Margin = new Padding(3, 4, 3, 4);
            butRemoveRound.Name = "butRemoveRound";
            butRemoveRound.Size = new Size(236, 54);
            butRemoveRound.TabIndex = 1;
            butRemoveRound.Text = "Remove";
            butRemoveRound.UseVisualStyleBackColor = true;
            butRemoveRound.Click += this.HndDeleteRound;
            // 
            // flowRounds
            // 
            flowRounds.Dock = DockStyle.Fill;
            flowRounds.Location = new Point(4, 5);
            flowRounds.Margin = new Padding(3, 4, 3, 4);
            flowRounds.Name = "flowRounds";
            flowRounds.Size = new Size(236, 769);
            flowRounds.TabIndex = 2;
            // 
            // layoutRoot
            // 
            layoutRoot.ColumnCount = 3;
            layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            layoutRoot.ColumnStyles.Add(new ColumnStyle());
            layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 640F));
            layoutRoot.Controls.Add(layoutRounds, 0, 0);
            layoutRoot.Controls.Add(this.playerListBox, 1, 0);
            layoutRoot.Controls.Add(flowMatchCards, 2, 0);
            layoutRoot.Dock = DockStyle.Fill;
            layoutRoot.Location = new Point(0, 0);
            layoutRoot.Margin = new Padding(3, 4, 3, 4);
            layoutRoot.Name = "layoutRoot";
            layoutRoot.RowCount = 1;
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutRoot.Size = new Size(1583, 939);
            layoutRoot.TabIndex = 1;
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
            flowMatchCards.Controls.Add(this.matchCard1);
            flowMatchCards.Location = new Point(603, 4);
            flowMatchCards.Margin = new Padding(3, 4, 3, 4);
            flowMatchCards.Name = "flowMatchCards";
            flowMatchCards.Size = new Size(694, 931);
            flowMatchCards.TabIndex = 1;
            // 
            // matchCard1
            // 
            this.matchCard1.BackColor = Color.Silver;
            this.matchCard1.BorderStyle = BorderStyle.FixedSingle;
            this.matchCard1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.matchCard1.Location = new Point(3, 4);
            this.matchCard1.Margin = new Padding(3, 4, 3, 4);
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
            this.Controls.Add(layoutRoot);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "EventPanel";
            this.Size = new Size(1583, 939);
            layoutRounds.ResumeLayout(false);
            layoutRoot.ResumeLayout(false);
            flowMatchCards.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layoutRounds;
        private Button butAddRound;
        private Button butRemoveRound;
        private FlowLayoutPanel flowRounds;
        private TableLayoutPanel layoutRoot;
        private FlowLayoutPanel flowMatchCards;
        private PlayerListBox.PlayerListBox playerListBox;
        private MatchCard.MatchCard matchCard1;
    }
}
