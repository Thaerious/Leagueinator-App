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
            this.canvas = new Leagueinator.PrinterComponents.PrinterCanvas();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Dock = DockStyle.Fill;
            this.canvas.GridSize = 25;
            this.canvas.Location = new Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new Size(1430, 834);
            this.canvas.SubGridSize = 5;
            this.canvas.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1430, 834);
            this.Controls.Add(this.canvas);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }

        #endregion

        public Leagueinator.PrinterComponents.PrinterCanvas canvas;
        private Leagueinator.PrinterComponents.PrinterCanvas printerCanvas1;
    }
}
