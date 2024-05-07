namespace Leagueinator.VisualUnitTest {
    partial class InputNameDialog {
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
            this.TextName = new TextBox();
            this.ButtonOk = new Button();
            this.SuspendLayout();
            // 
            // TextName
            // 
            this.TextName.Location = new Point(51, 80);
            this.TextName.Name = "TextName";
            this.TextName.Size = new Size(309, 31);
            this.TextName.TabIndex = 0;
            this.TextName.KeyDown += this.HndKeyDown;
            // 
            // ButtonOk
            // 
            this.ButtonOk.DialogResult = DialogResult.OK;
            this.ButtonOk.Location = new Point(51, 160);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new Size(309, 34);
            this.ButtonOk.TabIndex = 1;
            this.ButtonOk.Text = "OK";
            this.ButtonOk.UseVisualStyleBackColor = true;
            // 
            // InputNameDialog
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(416, 273);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.TextName);
            this.Name = "InputNameDialog";
            this.Text = "Test Name";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TextBox TextName;
        private Button ButtonOk;
    }
}
