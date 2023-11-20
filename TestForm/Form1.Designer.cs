namespace TestForm {
    partial class Form1 {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            printToolStripMenuItem = new ToolStripMenuItem();
            printPreviewDialog = new PrintPreviewDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.customControl1 = new CustomControl();
            menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { printToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1160, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.Size = new Size(64, 29);
            printToolStripMenuItem.Text = "Print";
            printToolStripMenuItem.Click += this.Menu_Print;
            // 
            // printPreviewDialog
            // 
            printPreviewDialog.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog.ClientSize = new Size(400, 300);
            printPreviewDialog.Enabled = true;
            printPreviewDialog.Icon = (Icon)resources.GetObject("printPreviewDialog.Icon");
            printPreviewDialog.Name = "printPreviewDialog";
            printPreviewDialog.Visible = false;
            // 
            // customControl1
            // 
            this.customControl1.Location = new Point(122, 143);
            this.customControl1.Name = "customControl1";
            this.customControl1.Size = new Size(879, 649);
            this.customControl1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1160, 937);
            this.Controls.Add(this.customControl1);
            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem printToolStripMenuItem;
        private PrintPreviewDialog printPreviewDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private CustomControl customControl1;
    }
}
