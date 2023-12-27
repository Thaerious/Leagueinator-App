namespace Leagueinator_App.Forms {
    partial class MatchDevForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.matchControl1 = new Components.MatchCard.MatchControl();
            this.SuspendLayout();
            // 
            // matchControl1
            // 
            this.matchControl1.BackColor = Color.Green;
            this.matchControl1.Location = new Point(23, 110);
            this.matchControl1.Name = "matchControl1";
            this.matchControl1.Size = new Size(500, 121);
            this.matchControl1.TabIndex = 0;
            // 
            // MatchDevForm
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.matchControl1);
            this.Name = "MatchDevForm";
            this.Text = "MatchDevForm";
            this.ResumeLayout(false);
        }

        #endregion

        private Components.MatchCard.MatchControl matchControl1;
    }
}