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
            this.TableLayout = new TableLayoutPanel();
            this.panel1 = new Panel();
            this.toolStrip1.SuspendLayout();
            this.TableLayout.SuspendLayout();
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
            this.Canvas.Location = new Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Page = 0;
            this.Canvas.RenderNode = null;
            this.Canvas.Size = new Size(864, 944);
            this.Canvas.SubGridSize = 0;
            this.Canvas.TabIndex = 1;
            this.Canvas.ToBack = false;
            // 
            // TableLayout
            // 
            this.TableLayout.ColumnCount = 1;
            this.TableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.TableLayout.Controls.Add(this.panel1, 0, 1);
            this.TableLayout.Controls.Add(this.Canvas, 0, 0);
            this.TableLayout.Dock = DockStyle.Fill;
            this.TableLayout.Location = new Point(0, 34);
            this.TableLayout.Name = "TableLayout";
            this.TableLayout.RowCount = 2;
            this.TableLayout.RowStyles.Add(new RowStyle());
            this.TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 400F));
            this.TableLayout.Size = new Size(870, 944);
            this.TableLayout.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = Color.FromArgb(255, 192, 192);
            this.panel1.Location = new Point(3, 953);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(300, 300);
            this.panel1.TabIndex = 1;
            // 
            // FormEventResults
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(870, 978);
            this.Controls.Add(this.TableLayout);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormEventResults";
            this.Text = "FormEventResults";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.TableLayout.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton butPrint;
        private PrinterCanvas Canvas;
        private TableLayoutPanel TableLayout;
        private Panel panel1;
    }
}
