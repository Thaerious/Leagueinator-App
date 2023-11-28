namespace PrinterTestForm {
    partial class MainForm {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer = new SplitContainer();
            this.printerCanvas = new PrinterCanvas();
            this.toolStrip1 = new ToolStrip();
            this.toolRefresh = new ToolStripButton();
            this.tabSource = new TabControl();
            this.tabXML = new TabPage();
            this.txtXML = new TextBox();
            this.tabStyle = new TabPage();
            this.txtStyle = new TextBox();
            this.toolPrintXML = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabSource.SuspendLayout();
            this.tabXML.SuspendLayout();
            this.tabStyle.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Location = new Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.printerCanvas);
            this.splitContainer.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabSource);
            this.splitContainer.Size = new Size(1733, 897);
            this.splitContainer.SplitterDistance = 843;
            this.splitContainer.TabIndex = 0;
            // 
            // printerCanvas
            // 
            this.printerCanvas.Dock = DockStyle.Fill;
            this.printerCanvas.Location = new Point(0, 34);
            this.printerCanvas.Margin = new Padding(0);
            this.printerCanvas.Name = "printerCanvas";
            this.printerCanvas.Size = new Size(843, 863);
            this.printerCanvas.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new Size(24, 24);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolRefresh, this.toolPrintXML });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(843, 34);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolRefresh
            // 
            this.toolRefresh.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolRefresh.Image = (Image)resources.GetObject("toolRefresh.Image");
            this.toolRefresh.ImageTransparentColor = Color.Magenta;
            this.toolRefresh.Name = "toolRefresh";
            this.toolRefresh.Size = new Size(74, 29);
            this.toolRefresh.Text = "Refresh";
            this.toolRefresh.Click += this.ToolRefresh_Click;
            // 
            // tabSource
            // 
            this.tabSource.Controls.Add(this.tabXML);
            this.tabSource.Controls.Add(this.tabStyle);
            this.tabSource.Dock = DockStyle.Fill;
            this.tabSource.Location = new Point(0, 0);
            this.tabSource.Name = "tabSource";
            this.tabSource.SelectedIndex = 0;
            this.tabSource.Size = new Size(886, 897);
            this.tabSource.TabIndex = 0;
            // 
            // tabXML
            // 
            this.tabXML.Controls.Add(this.txtXML);
            this.tabXML.Location = new Point(4, 34);
            this.tabXML.Name = "tabXML";
            this.tabXML.Padding = new Padding(3);
            this.tabXML.Size = new Size(878, 859);
            this.tabXML.TabIndex = 0;
            this.tabXML.Text = "xml";
            this.tabXML.UseVisualStyleBackColor = true;
            // 
            // txtXML
            // 
            this.txtXML.Dock = DockStyle.Fill;
            this.txtXML.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtXML.Location = new Point(3, 3);
            this.txtXML.Multiline = true;
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new Size(872, 853);
            this.txtXML.TabIndex = 0;
            this.txtXML.Text = "<document></document>";
            // 
            // tabStyle
            // 
            this.tabStyle.Controls.Add(this.txtStyle);
            this.tabStyle.Location = new Point(4, 34);
            this.tabStyle.Name = "tabStyle";
            this.tabStyle.Padding = new Padding(3);
            this.tabStyle.Size = new Size(878, 859);
            this.tabStyle.TabIndex = 1;
            this.tabStyle.Text = "style";
            this.tabStyle.UseVisualStyleBackColor = true;
            // 
            // txtStyle
            // 
            this.txtStyle.Dock = DockStyle.Fill;
            this.txtStyle.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtStyle.Location = new Point(3, 3);
            this.txtStyle.Multiline = true;
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new Size(872, 853);
            this.txtStyle.TabIndex = 1;
            // 
            // toolPrintXML
            // 
            this.toolPrintXML.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolPrintXML.Image = (Image)resources.GetObject("toolPrintXML.Image");
            this.toolPrintXML.ImageTransparentColor = Color.Magenta;
            this.toolPrintXML.Name = "toolPrintXML";
            this.toolPrintXML.Size = new Size(92, 29);
            this.toolPrintXML.Text = "Print XML";
            this.toolPrintXML.Click += this.ToolPrintXML_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1733, 897);
            this.Controls.Add(this.splitContainer);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabSource.ResumeLayout(false);
            this.tabXML.ResumeLayout(false);
            this.tabXML.PerformLayout();
            this.tabStyle.ResumeLayout(false);
            this.tabStyle.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer;
        private TabControl tabSource;
        private TabPage tabXML;
        private TabPage tabStyle;
        private ToolStrip toolStrip1;
        private ToolStripButton toolRefresh;
        private PrinterCanvas printerCanvas;
        private TextBox txtXML;
        private TextBox txtStyle;
        public ToolStripButton toolPrintXML;
    }
}
