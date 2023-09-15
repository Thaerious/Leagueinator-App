namespace Leagueinator.App.Components.MatchCard {
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
            txtScore0 = new TextBox();
            txtScore1 = new TextBox();
            flowTeam0 = new FlowLayoutPanel();
            flowTeam1 = new FlowLayoutPanel();
            labelLane = new Label();
            this.SuspendLayout();
            // 
            // txtScore0
            // 
            txtScore0.Anchor = AnchorStyles.Left;
            txtScore0.BackColor = Color.WhiteSmoke;
            txtScore0.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point);
            txtScore0.Location = new Point(15, 33);
            txtScore0.Margin = new Padding(3, 4, 3, 4);
            txtScore0.Name = "txtScore0";
            txtScore0.Size = new Size(64, 40);
            txtScore0.TabIndex = 0;
            txtScore0.Text = "0";
            txtScore0.TextAlign = HorizontalAlignment.Center;
            // 
            // txtScore1
            // 
            txtScore1.Anchor = AnchorStyles.Right;
            txtScore1.BackColor = Color.WhiteSmoke;
            txtScore1.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point);
            txtScore1.Location = new Point(539, 33);
            txtScore1.Margin = new Padding(3, 4, 3, 4);
            txtScore1.Name = "txtScore1";
            txtScore1.Size = new Size(64, 40);
            txtScore1.TabIndex = 1;
            txtScore1.Text = "0";
            txtScore1.TextAlign = HorizontalAlignment.Center;
            // 
            // flowTeam0
            // 
            flowTeam0.Anchor = AnchorStyles.Left;
            flowTeam0.AutoSize = true;
            flowTeam0.BackColor = Color.Silver;
            flowTeam0.FlowDirection = FlowDirection.TopDown;
            flowTeam0.Location = new Point(94, 46);
            flowTeam0.Margin = new Padding(3, 4, 3, 4);
            flowTeam0.Name = "flowTeam0";
            flowTeam0.Size = new Size(180, 13);
            flowTeam0.TabIndex = 2;
            // 
            // flowTeam1
            // 
            flowTeam1.Anchor = AnchorStyles.Left;
            flowTeam1.AutoSize = true;
            flowTeam1.BackColor = Color.Silver;
            flowTeam1.FlowDirection = FlowDirection.TopDown;
            flowTeam1.Location = new Point(346, 46);
            flowTeam1.Margin = new Padding(3, 4, 3, 4);
            flowTeam1.Name = "flowTeam1";
            flowTeam1.Size = new Size(180, 13);
            flowTeam1.TabIndex = 3;
            // 
            // labelLane
            // 
            labelLane.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            labelLane.AutoSize = true;
            labelLane.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point);
            labelLane.Location = new Point(296, 36);
            labelLane.Name = "labelLane";
            labelLane.Size = new Size(31, 33);
            labelLane.TabIndex = 4;
            labelLane.Text = "1";
            labelLane.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MatchCard
            // 
            this.AutoScaleDimensions = new SizeF(11F, 28F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Silver;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(labelLane);
            this.Controls.Add(flowTeam0);
            this.Controls.Add(flowTeam1);
            this.Controls.Add(txtScore0);
            this.Controls.Add(txtScore1);
            this.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.Margin = new Padding(3, 4, 3, 4);
            this.MaximumSize = new Size(620, 110);
            this.MinimumSize = new Size(620, 110);
            this.Name = "MatchCard";
            this.Size = new Size(618, 108);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TextBox txtScore0;
        private TextBox txtScore1;
        private FlowLayoutPanel flowTeam0;
        private FlowLayoutPanel flowTeam1;
        private Label labelLane;
    }
}
