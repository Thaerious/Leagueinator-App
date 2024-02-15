namespace Printer_Dev_Form
{
    partial class Form1
    {
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
            this.canvas = new Leagueinator.PrinterComponents.PrinterCanvas();
            this.menuStrip1 = new MenuStrip();
            this.printToolStripMenuItem = new ToolStripMenuItem();
            this.previewToolStripMenuItem = new ToolStripMenuItem();
            this.panel = new Panel();
            this.printDialog = new PrintDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog = new PrintPreviewDialog();
            this.menuStrip1.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.AutoScroll = true;
            this.canvas.AutoSize = true;
            this.canvas.Dock = DockStyle.Fill;
            this.canvas.GridSize = 0;
            this.canvas.Location = new Point(0, 0);
            this.canvas.Margin = new Padding(2);
            this.canvas.Name = "canvas";
            this.canvas.RootElement = null;
            this.canvas.Size = new Size(966, 579);
            this.canvas.SubGridSize = 0;
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.ToBack = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.printToolStripMenuItem, this.previewToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(966, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new Size(44, 20);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += this.Menu_Print_Click;
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new Size(60, 20);
            this.previewToolStripMenuItem.Text = "Preview";
            this.previewToolStripMenuItem.Click += this.Menu_Preview_Click;
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Controls.Add(this.canvas);
            this.panel.Dock = DockStyle.Fill;
            this.panel.Location = new Point(0, 24);
            this.panel.Name = "panel";
            this.panel.Size = new Size(966, 579);
            this.panel.TabIndex = 2;
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
            this.printPreviewDialog.ClientSize = new Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = (Icon)resources.GetObject("printPreviewDialog.Icon");
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ClientSize = new Size(966, 603);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new Padding(2);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        public Leagueinator.PrinterComponents.PrinterCanvas canvas;
        private Leagueinator.PrinterComponents.PrinterCanvas printerCanvas1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem printToolStripMenuItem;
        private Panel panel;
        private PrintDialog printDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private ToolStripMenuItem previewToolStripMenuItem;
        private PrintPreviewDialog printPreviewDialog;
    }
}
