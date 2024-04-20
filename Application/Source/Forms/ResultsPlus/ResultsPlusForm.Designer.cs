using Leagueinator.Printer.Components;

namespace Leagueinator.Forms.ResultsPlus {
    partial class ResultsPlusForm {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsPlusForm));
            this.toolStrip1 = new ToolStrip();
            this.butPrint = new ToolStripButton();
            this.TableLayout = new TableLayoutPanel();
            this.Canvas = new PrinterCanvas();
            this.ToolPanel = new Panel();
            this.toolContainer = new Panel();
            this.ButLast = new Button();
            this.ButNext = new Button();
            this.LabelPage = new Label();
            this.ButPrev = new Button();
            this.ButFirst = new Button();
            this.toolStrip1.SuspendLayout();
            this.TableLayout.SuspendLayout();
            this.ToolPanel.SuspendLayout();
            this.toolContainer.SuspendLayout();
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
            // TableLayout
            // 
            this.TableLayout.ColumnCount = 1;
            this.TableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.TableLayout.Controls.Add(this.Canvas, 0, 0);
            this.TableLayout.Controls.Add(this.ToolPanel, 0, 1);
            this.TableLayout.Dock = DockStyle.Fill;
            this.TableLayout.Location = new Point(0, 34);
            this.TableLayout.Name = "TableLayout";
            this.TableLayout.RowCount = 2;
            this.TableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.TableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            this.TableLayout.Size = new Size(870, 944);
            this.TableLayout.TabIndex = 1;
            // 
            // Canvas
            // 
            this.Canvas.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.Canvas.GridSize = 0;
            this.Canvas.InnerBorder = BorderStyle.None;
            this.Canvas.Location = new Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Page = 0;
            this.Canvas.RenderNode = null;
            this.Canvas.Size = new Size(864, 838);
            this.Canvas.SubGridSize = 0;
            this.Canvas.TabIndex = 1;
            this.Canvas.ToBack = false;
            // 
            // ToolPanel
            // 
            this.ToolPanel.Controls.Add(this.toolContainer);
            this.ToolPanel.Dock = DockStyle.Fill;
            this.ToolPanel.Location = new Point(3, 847);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.Size = new Size(864, 94);
            this.ToolPanel.TabIndex = 2;
            // 
            // toolContainer
            // 
            this.toolContainer.Anchor = AnchorStyles.None;
            this.toolContainer.BackColor = Color.Transparent;
            this.toolContainer.Controls.Add(this.ButLast);
            this.toolContainer.Controls.Add(this.ButNext);
            this.toolContainer.Controls.Add(this.LabelPage);
            this.toolContainer.Controls.Add(this.ButPrev);
            this.toolContainer.Controls.Add(this.ButFirst);
            this.toolContainer.Location = new Point(236, 5);
            this.toolContainer.Name = "toolContainer";
            this.toolContainer.Size = new Size(436, 80);
            this.toolContainer.TabIndex = 0;
            // 
            // ButLast
            // 
            this.ButLast.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ButLast.Location = new Point(353, 18);
            this.ButLast.Name = "ButLast";
            this.ButLast.Size = new Size(80, 46);
            this.ButLast.TabIndex = 4;
            this.ButLast.Text = ">>>";
            this.ButLast.UseVisualStyleBackColor = true;
            this.ButLast.Click += this.ButLast_Click;
            // 
            // ButNext
            // 
            this.ButNext.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ButNext.Location = new Point(267, 18);
            this.ButNext.Name = "ButNext";
            this.ButNext.Size = new Size(80, 46);
            this.ButNext.TabIndex = 3;
            this.ButNext.Text = ">";
            this.ButNext.UseVisualStyleBackColor = true;
            this.ButNext.Click += this.ButNext_Click;
            // 
            // LabelPage
            // 
            this.LabelPage.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.LabelPage.Location = new Point(173, 18);
            this.LabelPage.Name = "LabelPage";
            this.LabelPage.Size = new Size(88, 38);
            this.LabelPage.TabIndex = 2;
            this.LabelPage.Text = "1/1";
            this.LabelPage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ButPrev
            // 
            this.ButPrev.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ButPrev.Location = new Point(89, 15);
            this.ButPrev.Name = "ButPrev";
            this.ButPrev.Size = new Size(80, 46);
            this.ButPrev.TabIndex = 1;
            this.ButPrev.Text = "<";
            this.ButPrev.UseVisualStyleBackColor = true;
            this.ButPrev.Click += this.ButPrev_Click;
            // 
            // ButFirst
            // 
            this.ButFirst.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ButFirst.Location = new Point(3, 15);
            this.ButFirst.Name = "ButFirst";
            this.ButFirst.Size = new Size(80, 46);
            this.ButFirst.TabIndex = 0;
            this.ButFirst.Text = "<<<";
            this.ButFirst.UseVisualStyleBackColor = true;
            this.ButFirst.Click += this.ButFirst_Click;
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
            this.ToolPanel.ResumeLayout(false);
            this.toolContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton butPrint;
        private TableLayoutPanel TableLayout;
        private Panel ToolPanel;
        private PrinterCanvas Canvas;
        private Panel toolContainer;
        private Button ButFirst;
        private Button ButLast;
        private Button ButNext;
        private Label LabelPage;
        private Button ButPrev;
    }
}
