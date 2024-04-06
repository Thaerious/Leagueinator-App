namespace Leagueinator.VisualUnitTest {
    partial class Main {
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
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.splitContainer1 = new SplitContainer();
            this.tabControlView = new TabControl();
            this.tabPageActual = new TabPage();
            this.CanvasActual = new Printer.Components.PrinterCanvas();
            this.tabPageExpected = new TabPage();
            this.PanelExpected = new Panel();
            this.tabControlModel = new TabControl();
            this.tabPageXML = new TabPage();
            this.RichTextXML = new CustomRichTextBox();
            this.tabPageStyle = new TabPage();
            this.RichTextStyle = new RichTextBox();
            this.FlowPanelTestCards = new FlowLayoutPanel();
            this.menuStrip = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.loadToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.closeToolStripMenuItem = new ToolStripMenuItem();
            this.autoFormatToolStripMenuItem = new ToolStripMenuItem();
            this.testToolStripMenuItem = new ToolStripMenuItem();
            this.addToolStripMenuItem = new ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new ToolStripMenuItem();
            this.removeToolStripMenuItem = new ToolStripMenuItem();
            this.renameToolStripMenuItem = new ToolStripMenuItem();
            this.runSelectedToolStripMenuItem = new ToolStripMenuItem();
            this.runAllToolStripMenuItem = new ToolStripMenuItem();
            this.acceptActualToolStripMenuItem = new ToolStripMenuItem();
            this.clearResultsToolStripMenuItem = new ToolStripMenuItem();
            this.FolderDialog = new FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlView.SuspendLayout();
            this.tabPageActual.SuspendLayout();
            this.tabPageExpected.SuspendLayout();
            this.tabControlModel.SuspendLayout();
            this.tabPageXML.SuspendLayout();
            this.tabPageStyle.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 450F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FlowPanelTestCards, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new Size(1729, 915);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(453, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlModel);
            this.splitContainer1.Size = new Size(1273, 909);
            this.splitContainer1.SplitterDistance = 626;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControlView
            // 
            this.tabControlView.Controls.Add(this.tabPageActual);
            this.tabControlView.Controls.Add(this.tabPageExpected);
            this.tabControlView.Dock = DockStyle.Fill;
            this.tabControlView.Location = new Point(0, 0);
            this.tabControlView.Name = "tabControlView";
            this.tabControlView.SelectedIndex = 0;
            this.tabControlView.Size = new Size(626, 909);
            this.tabControlView.TabIndex = 0;
            // 
            // tabPageActual
            // 
            this.tabPageActual.Controls.Add(this.CanvasActual);
            this.tabPageActual.Location = new Point(4, 34);
            this.tabPageActual.Name = "tabPageActual";
            this.tabPageActual.Padding = new Padding(3);
            this.tabPageActual.Size = new Size(618, 871);
            this.tabPageActual.TabIndex = 0;
            this.tabPageActual.Text = "Acutal";
            this.tabPageActual.UseVisualStyleBackColor = true;
            // 
            // CanvasActual
            // 
            this.CanvasActual.Dock = DockStyle.Fill;
            this.CanvasActual.GridSize = 100;
            this.CanvasActual.InnerBorder = BorderStyle.None;
            this.CanvasActual.Location = new Point(3, 3);
            this.CanvasActual.Name = "CanvasActual";
            this.CanvasActual.Page = 0;
            this.CanvasActual.RenderNode = null;
            this.CanvasActual.Size = new Size(612, 865);
            this.CanvasActual.SubGridSize = 25;
            this.CanvasActual.TabIndex = 0;
            this.CanvasActual.ToBack = true;
            // 
            // tabPageExpected
            // 
            this.tabPageExpected.Controls.Add(this.PanelExpected);
            this.tabPageExpected.Location = new Point(4, 34);
            this.tabPageExpected.Name = "tabPageExpected";
            this.tabPageExpected.Padding = new Padding(3);
            this.tabPageExpected.Size = new Size(636, 871);
            this.tabPageExpected.TabIndex = 1;
            this.tabPageExpected.Text = "Expected";
            this.tabPageExpected.UseVisualStyleBackColor = true;
            // 
            // PanelExpected
            // 
            this.PanelExpected.Dock = DockStyle.Fill;
            this.PanelExpected.Location = new Point(3, 3);
            this.PanelExpected.Name = "PanelExpected";
            this.PanelExpected.Size = new Size(630, 865);
            this.PanelExpected.TabIndex = 0;
            // 
            // tabControlModel
            // 
            this.tabControlModel.Controls.Add(this.tabPageXML);
            this.tabControlModel.Controls.Add(this.tabPageStyle);
            this.tabControlModel.Dock = DockStyle.Fill;
            this.tabControlModel.Location = new Point(0, 0);
            this.tabControlModel.Name = "tabControlModel";
            this.tabControlModel.SelectedIndex = 0;
            this.tabControlModel.Size = new Size(643, 909);
            this.tabControlModel.TabIndex = 0;
            // 
            // tabPageXML
            // 
            this.tabPageXML.Controls.Add(this.RichTextXML);
            this.tabPageXML.Location = new Point(4, 34);
            this.tabPageXML.Name = "tabPageXML";
            this.tabPageXML.Padding = new Padding(3);
            this.tabPageXML.Size = new Size(635, 871);
            this.tabPageXML.TabIndex = 0;
            this.tabPageXML.Text = "XML";
            this.tabPageXML.UseVisualStyleBackColor = true;
            // 
            // RichTextXML
            // 
            this.RichTextXML.AcceptsTab = true;
            this.RichTextXML.Dock = DockStyle.Fill;
            this.RichTextXML.Font = new Font("Consolas", 12F);
            this.RichTextXML.Location = new Point(3, 3);
            this.RichTextXML.Name = "RichTextXML";
            this.RichTextXML.Size = new Size(629, 865);
            this.RichTextXML.TabIndex = 1;
            this.RichTextXML.Text = "";
            // 
            // tabPageStyle
            // 
            this.tabPageStyle.Controls.Add(this.RichTextStyle);
            this.tabPageStyle.Location = new Point(4, 34);
            this.tabPageStyle.Name = "tabPageStyle";
            this.tabPageStyle.Padding = new Padding(3);
            this.tabPageStyle.Size = new Size(652, 871);
            this.tabPageStyle.TabIndex = 1;
            this.tabPageStyle.Text = "Style";
            this.tabPageStyle.UseVisualStyleBackColor = true;
            // 
            // RichTextStyle
            // 
            this.RichTextStyle.AcceptsTab = true;
            this.RichTextStyle.Dock = DockStyle.Fill;
            this.RichTextStyle.Font = new Font("Consolas", 12F);
            this.RichTextStyle.Location = new Point(3, 3);
            this.RichTextStyle.Name = "RichTextStyle";
            this.RichTextStyle.Size = new Size(646, 865);
            this.RichTextStyle.TabIndex = 0;
            this.RichTextStyle.Text = "";
            // 
            // FlowPanelTestCards
            // 
            this.FlowPanelTestCards.AutoScroll = true;
            this.FlowPanelTestCards.Dock = DockStyle.Fill;
            this.FlowPanelTestCards.Location = new Point(3, 3);
            this.FlowPanelTestCards.Name = "FlowPanelTestCards";
            this.FlowPanelTestCards.Size = new Size(444, 909);
            this.FlowPanelTestCards.TabIndex = 1;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new Size(24, 24);
            this.menuStrip.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.testToolStripMenuItem });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(1729, 33);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.loadToolStripMenuItem, this.saveToolStripMenuItem, this.closeToolStripMenuItem, this.autoFormatToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new Size(275, 34);
            this.loadToolStripMenuItem.Text = "Load Directory";
            this.loadToolStripMenuItem.Click += this.HndMenuLoadClick;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            this.saveToolStripMenuItem.Size = new Size(275, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += this.HndMenuSave;
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new Size(275, 34);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += this.HndMenuCloseClick;
            // 
            // autoFormatToolStripMenuItem
            // 
            this.autoFormatToolStripMenuItem.Name = "autoFormatToolStripMenuItem";
            this.autoFormatToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            this.autoFormatToolStripMenuItem.Size = new Size(275, 34);
            this.autoFormatToolStripMenuItem.Text = "Auto Format";
            this.autoFormatToolStripMenuItem.Click += this.HndMenuAutoFormat;
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.addToolStripMenuItem, this.duplicateToolStripMenuItem, this.removeToolStripMenuItem, this.renameToolStripMenuItem, this.runSelectedToolStripMenuItem, this.runAllToolStripMenuItem, this.acceptActualToolStripMenuItem, this.clearResultsToolStripMenuItem });
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new Size(58, 29);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = Keys.F3;
            this.addToolStripMenuItem.Size = new Size(255, 34);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += this.HndMenuAddTest;
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            this.duplicateToolStripMenuItem.Size = new Size(255, 34);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            this.duplicateToolStripMenuItem.Click += this.HndMenuDuplicate;
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Delete;
            this.removeToolStripMenuItem.Size = new Size(255, 34);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += this.HndMenuDelete;
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.ShortcutKeys = Keys.F2;
            this.renameToolStripMenuItem.Size = new Size(255, 34);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += this.HndMenuRenameTest;
            // 
            // runSelectedToolStripMenuItem
            // 
            this.runSelectedToolStripMenuItem.Name = "runSelectedToolStripMenuItem";
            this.runSelectedToolStripMenuItem.ShortcutKeys = Keys.F5;
            this.runSelectedToolStripMenuItem.Size = new Size(255, 34);
            this.runSelectedToolStripMenuItem.Text = "Run Selected";
            this.runSelectedToolStripMenuItem.Click += this.HndMenuRunSelected;
            // 
            // runAllToolStripMenuItem
            // 
            this.runAllToolStripMenuItem.Name = "runAllToolStripMenuItem";
            this.runAllToolStripMenuItem.ShortcutKeys = Keys.F6;
            this.runAllToolStripMenuItem.Size = new Size(255, 34);
            this.runAllToolStripMenuItem.Text = "Run All";
            this.runAllToolStripMenuItem.Click += this.HndMenuRunAll;
            // 
            // acceptActualToolStripMenuItem
            // 
            this.acceptActualToolStripMenuItem.Name = "acceptActualToolStripMenuItem";
            this.acceptActualToolStripMenuItem.ShortcutKeys = Keys.F3;
            this.acceptActualToolStripMenuItem.Size = new Size(255, 34);
            this.acceptActualToolStripMenuItem.Text = "Accept Actual";
            this.acceptActualToolStripMenuItem.Click += this.HndAcceptActual;
            // 
            // clearResultsToolStripMenuItem
            // 
            this.clearResultsToolStripMenuItem.Name = "clearResultsToolStripMenuItem";
            this.clearResultsToolStripMenuItem.Size = new Size(255, 34);
            this.clearResultsToolStripMenuItem.Text = "Clear Results";
            this.clearResultsToolStripMenuItem.Click += this.HndMenuClearResults;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1729, 948);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.Text = "Visual Unit Tester";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlView.ResumeLayout(false);
            this.tabPageActual.ResumeLayout(false);
            this.tabPageExpected.ResumeLayout(false);
            this.tabControlModel.ResumeLayout(false);
            this.tabPageXML.ResumeLayout(false);
            this.tabPageStyle.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private SplitContainer splitContainer1;
        private TabControl tabControlView;
        private TabPage tabPageActual;
        private TabPage tabPageExpected;
        private TabControl tabControlModel;
        private TabPage tabPageXML;
        private TabPage tabPageStyle;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem runSelectedToolStripMenuItem;
        private ToolStripMenuItem runAllToolStripMenuItem;
        private Printer.Components.PrinterCanvas CanvasActual;
        private RichTextBox RichTextStyle;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem duplicateToolStripMenuItem;
        private ToolStripMenuItem acceptActualToolStripMenuItem;
        private Panel PanelExpected;
        private FlowLayoutPanel FlowPanelTestCards;
        internal FolderBrowserDialog FolderDialog;
        private ToolStripMenuItem clearResultsToolStripMenuItem;
        private CustomRichTextBox RichTextXML;
        private ToolStripMenuItem autoFormatToolStripMenuItem;
    }
}
