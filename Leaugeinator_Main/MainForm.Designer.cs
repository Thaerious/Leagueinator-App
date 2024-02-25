namespace LeaugeMain {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.eventPanel1 = new Components.EventPanel();
            this.SuspendLayout();
            // 
            // eventPanel1
            // 
            this.eventPanel1.Dock = DockStyle.Fill;
            this.eventPanel1.Location = new Point(0, 0);
            this.eventPanel1.Name = "eventPanel1";
            this.eventPanel1.Size = new Size(1199, 795);
            this.eventPanel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1199, 795);
            this.Controls.Add(this.eventPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }

        #endregion

        private Components.EventPanel eventPanel1;
    }
}
