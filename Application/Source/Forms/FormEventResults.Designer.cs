using Leagueinator.Printer.Components;

namespace Leagueinator.Forms {
    partial class FormEventResults {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEventResults));
            this.toolStrip1 = new ToolStrip();
            this.butPrint = new ToolStripButton();
            this.Canvas = new PrinterCanvas();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new Size(24, 24);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.butPrint });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(870, 34);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // butPrint
            // 
            this.butPrint.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.butPrint.Image = (Image)resources.GetObject("butPrint.Image");
            this.butPrint.ImageTransparentColor = Color.Magenta;
            this.butPrint.Name = "butPrint";
            this.butPrint.Size = new Size(52, 29);
            this.butPrint.Text = "Print";
            this.butPrint.Click += this.HndPrintClick;
            // 
            // Canvas
            // 
            this.Canvas.Dock = DockStyle.Fill;
            this.Canvas.GridSize = 0;
            this.Canvas.InnerBorder = BorderStyle.None;
            this.Canvas.Location = new Point(0, 34);
            this.Canvas.Name = "canvas";
            this.Canvas.Page = 0;
            this.Canvas.RenderNode = null;
            this.Canvas.Size = new Size(870, 944);
            this.Canvas.SubGridSize = 0;
            this.Canvas.TabIndex = 1;
            this.Canvas.ToBack = false;
            // 
            // FormEventResults
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(870, 978);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormEventResults";
            this.Text = "FormEventResults";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton butPrint;
        private PrinterCanvas Canvas;
    }
}
