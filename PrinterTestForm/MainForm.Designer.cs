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
            this.printerCanvas = new PrinterCanvas();
            this.pagePanel = new Panel();
            this.lblTimer = new Label();
            this.lblPage = new Label();
            this.butNext = new Button();
            this.butPrev = new Button();
            this.toolStrip1 = new ToolStrip();
            this.toolRefresh = new ToolStripButton();
            this.toolStripButton3 = new ToolStripButton();
            this.tabSource = new TabControl();
            this.tabXML = new TabPage();
            this.txtXML = new TextBox();
            this.tabStyle = new TabPage();
            this.txtStyle = new TextBox();
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.loadXMLToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.printToolStripMenuItem = new ToolStripMenuItem();
            this.viewToolStripMenuItem = new ToolStripMenuItem();
            this.landscapeToolStripMenuItem = new ToolStripMenuItem();
            this.portaitToolStripMenuItem = new ToolStripMenuItem();
            this.freeFormToolStripMenuItem = new ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.printerCanvas, 0, 0);
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
            // printerCanvas
            // 
            this.printerCanvas.Dock = DockStyle.Fill;
            this.printerCanvas.GridSize = 100;
            this.printerCanvas.InnerBorder = BorderStyle.FixedSingle;
            this.printerCanvas.Location = new Point(0, 0);
            this.printerCanvas.Margin = new Padding(0);
            this.printerCanvas.Name = "printerCanvas";
            this.printerCanvas.Page = 0;
            this.printerCanvas.RootElement = null;
            this.printerCanvas.Size = new Size(843, 750);
            this.printerCanvas.SubGridSize = 25;
            this.printerCanvas.TabIndex = 1;
            this.printerCanvas.ToBack = false;
            // 
            // pagePanel
            // 
            this.pagePanel.Controls.Add(this.lblTimer);
            this.pagePanel.Controls.Add(this.lblPage);
            this.pagePanel.Controls.Add(this.butNext);
            this.pagePanel.Controls.Add(this.butPrev);
            this.pagePanel.Dock = DockStyle.Fill;
            this.pagePanel.Location = new Point(3, 753);
            this.pagePanel.Name = "pagePanel";
            this.pagePanel.Size = new Size(837, 54);
            this.pagePanel.TabIndex = 1;
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new Point(699, 16);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new Size(51, 25);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "0 ms";
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
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolRefresh, this.toolStripButton3 });
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
            this.toolRefresh.Size = new Size(98, 29);
            this.toolRefresh.Text = "Do Layout";
            this.toolRefresh.Click += this.ToolLayoutClick;
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = (Image)resources.GetObject("toolStripButton3.Image");
            this.toolStripButton3.ImageTransparentColor = Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new Size(75, 29);
            this.toolStripButton3.Text = "Repaint";
            this.toolStripButton3.Click += this.toolStripButton3_Click;
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
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.loadXMLToolStripMenuItem, this.saveToolStripMenuItem, this.printToolStripMenuItem, this.printPreviewToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadXMLToolStripMenuItem
            // 
            this.loadXMLToolStripMenuItem.Name = "loadXMLToolStripMenuItem";
            this.loadXMLToolStripMenuItem.Size = new Size(270, 34);
            this.loadXMLToolStripMenuItem.Text = "Load";
            this.loadXMLToolStripMenuItem.Click += this.menuLoadClick;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new Size(270, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += this.menuSaveClick;
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new Size(270, 34);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += this.menuPrintClick;
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
            this.freeFormToolStripMenuItem.Text = "Square";
            this.freeFormToolStripMenuItem.Click += this.menuFreeFormClick;
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new Size(270, 34);
            this.printPreviewToolStripMenuItem.Text = "Print Preview";
            this.printPreviewToolStripMenuItem.Click += this.menuPreviewClick;
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
        private ToolStripButton toolStripButton3;
        private Label lblTimer;
        private ToolStripMenuItem loadXMLToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
    }
}
