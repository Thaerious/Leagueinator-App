﻿namespace Printer_Dev_Form
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
            this.canvas.GridSize = 0;
            this.canvas.Location = new Point(0, 0);
            this.canvas.Margin = new Padding(2);
            this.canvas.Name = "canvas";
            this.canvas.Root = null;
            this.canvas.Size = new Size(784, 579);
            this.canvas.SubGridSize = 0;
            this.canvas.TabIndex = 0;
            this.canvas.ToBack = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ClientSize = new Size(784, 579);
            this.Controls.Add(this.canvas);
            this.DoubleBuffered = true;
            this.Margin = new Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }

        #endregion

        public Leagueinator.PrinterComponents.PrinterCanvas canvas;
        private Leagueinator.PrinterComponents.PrinterCanvas printerCanvas1;
    }
}
