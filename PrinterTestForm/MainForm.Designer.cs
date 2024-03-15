using Leagueinator.PrinterComponents;

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
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.canvasPanel = new Panel();
            this.printerCanvas = new PrinterCanvas();
            this.pagePanel = new Panel();
            this.lblPage = new Label();
            this.butNext = new Button();
            this.butPrev = new Button();
            this.toolStrip1 = new ToolStrip();
            this.toolRefresh = new ToolStripButton();
            this.toolPrintXML = new ToolStripButton();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripButton2 = new ToolStripButton();
            this.tabSource = new TabControl();
            this.tabXML = new TabPage();
            this.txtXML = new TextBox();
            this.tabStyle = new TabPage();
            this.txtStyle = new TextBox();
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.viewToolStripMenuItem = new ToolStripMenuItem();
            this.landscapeToolStripMenuItem = new ToolStripMenuItem();
            this.portaitToolStripMenuItem = new ToolStripMenuItem();
            this.freeFormToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.canvasPanel.SuspendLayout();
            this.pagePanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabSource.SuspendLayout();
            this.tabXML.SuspendLayout();
            this.tabStyle.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Location = new Point(0, 33);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabSource);
            this.splitContainer.Size = new Size(1733, 864);
            this.splitContainer.SplitterDistance = 843;
            this.splitContainer.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.canvasPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pagePanel, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 34);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new Size(843, 830);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // canvasPanel
            // 
            this.canvasPanel.Controls.Add(this.printerCanvas);
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.Location = new Point(3, 3);
            this.canvasPanel.Name = "canvasPanel";
            this.canvasPanel.Size = new Size(837, 744);
            this.canvasPanel.TabIndex = 1;
            // 
            // printerCanvas
            // 
            this.printerCanvas.Dock = DockStyle.Fill;
            this.printerCanvas.GridSize = 100;
            this.printerCanvas.Location = new Point(0, 0);
            this.printerCanvas.Margin = new Padding(0);
            this.printerCanvas.Name = "printerCanvas";
            this.printerCanvas.Page = 0;
            this.printerCanvas.RootElement = null;
            this.printerCanvas.Size = new Size(837, 744);
            this.printerCanvas.SubGridSize = 25;
            this.printerCanvas.TabIndex = 1;
            this.printerCanvas.ToBack = false;
            // 
            // pagePanel
            // 
            this.pagePanel.Controls.Add(this.lblPage);
            this.pagePanel.Controls.Add(this.butNext);
            this.pagePanel.Controls.Add(this.butPrev);
            this.pagePanel.Dock = DockStyle.Fill;
            this.pagePanel.Location = new Point(3, 753);
            this.pagePanel.Name = "pagePanel";
            this.pagePanel.Size = new Size(837, 54);
            this.pagePanel.TabIndex = 1;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new Point(390, 16);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new Size(22, 25);
            this.lblPage.TabIndex = 2;
            this.lblPage.Text = "0";
            // 
            // butNext
            // 
            this.butNext.Location = new Point(456, 11);
            this.butNext.Name = "butNext";
            this.butNext.Size = new Size(112, 34);
            this.butNext.TabIndex = 1;
            this.butNext.Text = "Next Page";
            this.butNext.UseVisualStyleBackColor = true;
            this.butNext.Click += this.butNextClick;
            // 
            // butPrev
            // 
            this.butPrev.Location = new Point(227, 11);
            this.butPrev.Name = "butPrev";
            this.butPrev.Size = new Size(112, 34);
            this.butPrev.TabIndex = 0;
            this.butPrev.Text = "Prev Page";
            this.butPrev.UseVisualStyleBackColor = true;
            this.butPrev.Click += this.butPrevClick;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new Size(24, 24);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolRefresh, this.toolPrintXML, this.toolStripButton1, this.toolStripButton2 });
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
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(88, 29);
            this.toolStripButton1.Text = "Print CSS";
            this.toolStripButton1.Click += this.ToolPrintCSS_Click;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            this.toolStripButton2.ImageTransparentColor = Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new Size(83, 29);
            this.toolStripButton2.Text = "Loc XML";
            this.toolStripButton2.Click += this.ToolPrintLocXML;
            // 
            // tabSource
            // 
            this.tabSource.Controls.Add(this.tabXML);
            this.tabSource.Controls.Add(this.tabStyle);
            this.tabSource.Dock = DockStyle.Fill;
            this.tabSource.Location = new Point(0, 0);
            this.tabSource.Name = "tabSource";
            this.tabSource.SelectedIndex = 0;
            this.tabSource.Size = new Size(886, 864);
            this.tabSource.TabIndex = 0;
            // 
            // tabXML
            // 
            this.tabXML.Controls.Add(this.txtXML);
            this.tabXML.Location = new Point(4, 34);
            this.tabXML.Name = "tabXML";
            this.tabXML.Padding = new Padding(3);
            this.tabXML.Size = new Size(878, 826);
            this.tabXML.TabIndex = 0;
            this.tabXML.Text = "xml";
            this.tabXML.UseVisualStyleBackColor = true;
            // 
            // txtXML
            // 
            this.txtXML.AcceptsTab = true;
            this.txtXML.Dock = DockStyle.Fill;
            this.txtXML.Font = new Font("Consolas", 12F);
            this.txtXML.Location = new Point(3, 3);
            this.txtXML.Multiline = true;
            this.txtXML.Name = "txtXML";
            this.txtXML.ScrollBars = ScrollBars.Both;
            this.txtXML.Size = new Size(872, 820);
            this.txtXML.TabIndex = 0;
            this.txtXML.Text = "<document></document>";
            this.txtXML.KeyPress += this.TXT_KeyPress;
            // 
            // tabStyle
            // 
            this.tabStyle.Controls.Add(this.txtStyle);
            this.tabStyle.Location = new Point(4, 34);
            this.tabStyle.Name = "tabStyle";
            this.tabStyle.Padding = new Padding(3);
            this.tabStyle.Size = new Size(878, 826);
            this.tabStyle.TabIndex = 1;
            this.tabStyle.Text = "style";
            this.tabStyle.UseVisualStyleBackColor = true;
            // 
            // txtStyle
            // 
            this.txtStyle.AcceptsTab = true;
            this.txtStyle.Dock = DockStyle.Fill;
            this.txtStyle.Font = new Font("Consolas", 12F);
            this.txtStyle.Location = new Point(3, 3);
            this.txtStyle.Multiline = true;
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.ScrollBars = ScrollBars.Both;
            this.txtStyle.Size = new Size(872, 820);
            this.txtStyle.TabIndex = 1;
            this.txtStyle.KeyPress += this.TXT_KeyPress;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new Size(24, 24);
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.viewToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(1733, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.landscapeToolStripMenuItem, this.portaitToolStripMenuItem, this.freeFormToolStripMenuItem });
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new Size(65, 29);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // landscapeToolStripMenuItem
            // 
            this.landscapeToolStripMenuItem.Name = "landscapeToolStripMenuItem";
            this.landscapeToolStripMenuItem.Size = new Size(197, 34);
            this.landscapeToolStripMenuItem.Text = "Landscape";
            this.landscapeToolStripMenuItem.Click += this.menuLandscapeClick;
            // 
            // portaitToolStripMenuItem
            // 
            this.portaitToolStripMenuItem.Name = "portaitToolStripMenuItem";
            this.portaitToolStripMenuItem.Size = new Size(197, 34);
            this.portaitToolStripMenuItem.Text = "Portait";
            this.portaitToolStripMenuItem.Click += this.menuPortaitClick;
            // 
            // freeFormToolStripMenuItem
            // 
            this.freeFormToolStripMenuItem.Name = "freeFormToolStripMenuItem";
            this.freeFormToolStripMenuItem.Size = new Size(197, 34);
            this.freeFormToolStripMenuItem.Text = "FreeForm";
            this.freeFormToolStripMenuItem.Click += this.menuFreeFormClick;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1733, 897);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.canvasPanel.ResumeLayout(false);
            this.pagePanel.ResumeLayout(false);
            this.pagePanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabSource.ResumeLayout(false);
            this.tabXML.ResumeLayout(false);
            this.tabXML.PerformLayout();
            this.tabStyle.ResumeLayout(false);
            this.tabStyle.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private TabControl tabSource;
        private TabPage tabXML;
        private TabPage tabStyle;
        private ToolStrip toolStrip1;
        private PrinterCanvas printerCanvas;
        private TextBox txtXML;
        private TextBox txtStyle;
        public ToolStripButton toolPrintXML;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel pagePanel;
        private Label lblPage;
        private Button butNext;
        private Button butPrev;
        private ToolStripButton toolRefresh;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem landscapeToolStripMenuItem;
        private ToolStripMenuItem portaitToolStripMenuItem;
        private ToolStripMenuItem freeFormToolStripMenuItem;
        private Panel canvasPanel;
    }
}
