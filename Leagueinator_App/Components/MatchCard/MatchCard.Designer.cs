namespace Leagueinator.App.Components {
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
            this.txtScore0 = new TextBox();
            this.txtScore1 = new TextBox();
            this.flowTeam0 = new FlowLayoutPanel();
            this.label1 = new Label();
            this.flowTeam1 = new FlowLayoutPanel();
            this.label2 = new Label();
            this.labelLane = new Label();
            this.flowTeam0.SuspendLayout();
            this.flowTeam1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtScore0
            // 
            this.txtScore0.Anchor = AnchorStyles.Left;
            this.txtScore0.BackColor = Color.WhiteSmoke;
            this.txtScore0.Font = new Font("Arial", 14F, FontStyle.Bold);
            this.txtScore0.Location = new Point(3, 3);
            this.txtScore0.Margin = new Padding(3, 4, 3, 4);
            this.txtScore0.Name = "txtScore0";
            this.txtScore0.Size = new Size(64, 40);
            this.txtScore0.TabIndex = 0;
            this.txtScore0.Text = "0";
            this.txtScore0.TextAlign = HorizontalAlignment.Center;
            // 
            // txtScore1
            // 
            this.txtScore1.Anchor = AnchorStyles.Right;
            this.txtScore1.BackColor = Color.WhiteSmoke;
            this.txtScore1.Font = new Font("Arial", 14F, FontStyle.Bold);
            this.txtScore1.Location = new Point(-87, -21);
            this.txtScore1.Margin = new Padding(3, 4, 3, 4);
            this.txtScore1.Name = "txtScore1";
            this.txtScore1.Size = new Size(64, 40);
            this.txtScore1.TabIndex = 1;
            this.txtScore1.Text = "0";
            this.txtScore1.TextAlign = HorizontalAlignment.Center;
            // 
            // flowTeam0
            // 
            this.flowTeam0.Anchor = AnchorStyles.Left;
            this.flowTeam0.AutoSize = true;
            this.flowTeam0.BackColor = Color.Silver;
            this.flowTeam0.Controls.Add(this.label1);
            this.flowTeam0.FlowDirection = FlowDirection.TopDown;
            this.flowTeam0.Location = new Point(234, -50);
            this.flowTeam0.Margin = new Padding(3, 4, 3, 4);
            this.flowTeam0.Name = "flowTeam0";
            this.flowTeam0.Size = new Size(134, 32);
            this.flowTeam0.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(128, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowTeam1
            // 
            this.flowTeam1.Anchor = AnchorStyles.Left;
            this.flowTeam1.AutoSize = true;
            this.flowTeam1.BackColor = Color.Silver;
            this.flowTeam1.Controls.Add(this.label2);
            this.flowTeam1.FlowDirection = FlowDirection.TopDown;
            this.flowTeam1.Location = new Point(623, -50);
            this.flowTeam1.Margin = new Padding(3, 4, 3, 4);
            this.flowTeam1.Name = "flowTeam1";
            this.flowTeam1.Size = new Size(134, 32);
            this.flowTeam1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(128, 32);
            this.label2.TabIndex = 6;
            this.label2.Text = "label2";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelLane
            // 
            this.labelLane.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.labelLane.AutoSize = true;
            this.labelLane.Font = new Font("Arial", 14F, FontStyle.Bold);
            this.labelLane.Location = new Point(-30, 0);
            this.labelLane.Name = "labelLane";
            this.labelLane.Size = new Size(31, 33);
            this.labelLane.TabIndex = 4;
            this.labelLane.Text = "1";
            this.labelLane.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MatchCard
            // 
            this.AutoScaleDimensions = new SizeF(11F, 28F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Green;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.txtScore0);
            this.Controls.Add(this.txtScore1);
            this.Controls.Add(this.flowTeam0);
            this.Controls.Add(this.labelLane);
            this.Controls.Add(this.flowTeam1);
            this.Cursor = Cursors.IBeam;
            this.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "MatchCard";
            this.Padding = new Padding(20, 10, 20, 10);
            this.Size = new Size(0, 0);
            this.flowTeam0.ResumeLayout(false);
            this.flowTeam1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TextBox txtScore0;
        private TextBox txtScore1;
        private FlowLayoutPanel flowTeam0;
        private FlowLayoutPanel flowTeam1;
        private Label labelLane;
        private Label label1;
        private Label label2;
    }
}
