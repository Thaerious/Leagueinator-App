namespace Leagueinator_App.Components.MatchCard {
    partial class MatchControl {
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
            this.label1 = new Label();
            this.txtName0 = new TextBox();
            this.txtScore0 = new TextBox();
            this.txtScore1 = new TextBox();
            this.panel0 = new PlayerPanel();
            this.panel1 = new PlayerPanel();
            this.textBox1 = new TextBox();
            this.textName1 = new TextBox();
            this.panel0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(242, 59);
            this.label1.Margin = new Padding(10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(59, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // txtName0
            // 
            this.txtName0.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtName0.Location = new Point(10, 320);
            this.txtName0.Margin = new Padding(10);
            this.txtName0.Name = "txtName0";
            this.txtName0.Size = new Size(2400, 31);
            this.txtName0.TabIndex = 1;
            // 
            // txtScore0
            // 
            this.txtScore0.Location = new Point(13, 59);
            this.txtScore0.Margin = new Padding(10);
            this.txtScore0.Name = "txtScore0";
            this.txtScore0.Size = new Size(31, 31);
            this.txtScore0.TabIndex = 3;
            // 
            // txtScore1
            // 
            this.txtScore1.Location = new Point(485, 59);
            this.txtScore1.Margin = new Padding(10);
            this.txtScore1.Name = "txtScore1";
            this.txtScore1.Size = new Size(31, 31);
            this.txtScore1.TabIndex = 4;
            // 
            // panel0
            // 
            this.panel0.AutoSize = true;
            this.panel0.BackColor = Color.FromArgb(128, 255, 128);
            this.panel0.Controls.Add(this.txtName0);
            this.panel0.Location = new Point(57, 23);
            this.panel0.Name = "panel0";
            this.panel0.Size = new Size(2550, 700);
            this.panel0.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.textName1);
            this.panel1.Location = new Point(314, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(158, 1268);
            this.panel1.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = AnchorStyles.None;
            this.textBox1.Location = new Point(10, 599);
            this.textBox1.Margin = new Padding(10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(158, 31);
            this.textBox1.TabIndex = 2;
            // 
            // textName1
            // 
            this.textName1.Anchor = AnchorStyles.Left;
            this.textName1.Location = new Point(10, 635);
            this.textName1.Margin = new Padding(10);
            this.textName1.Name = "textName1";
            this.textName1.Size = new Size(158, 31);
            this.textName1.TabIndex = 1;
            // 
            // MatchControl
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Green;
            this.Controls.Add(this.txtScore0);
            this.Controls.Add(this.panel0);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtScore1);
            this.Name = "MatchControl";
            this.Padding = new Padding(10);
            this.Size = new Size(545, 128);
            this.panel0.ResumeLayout(false);
            this.panel0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtName0;
        private TextBox txtName1;
        private TextBox txtScore0;
        private TextBox txtScore1;
        private PlayerPanel panel0;
        private PlayerPanel panel1;
        private TextBox textName1;
        private TextBox textBox1;
    }
}
