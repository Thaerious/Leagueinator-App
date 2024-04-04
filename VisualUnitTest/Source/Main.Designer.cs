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
            this.tabControl1 = new TabControl();
            this.tabPageActual = new TabPage();
            this.tabPageExpected = new TabPage();
            this.tabControl2 = new TabControl();
            this.tabPageXML = new TabPage();
            this.tabPageStyle = new TabPage();
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.testToolStripMenuItem = new ToolStripMenuItem();
            this.loadToolStripMenuItem = new ToolStripMenuItem();
            this.closeToolStripMenuItem = new ToolStripMenuItem();
            this.addToolStripMenuItem = new ToolStripMenuItem();
            this.removeToolStripMenuItem = new ToolStripMenuItem();
            this.renameToolStripMenuItem = new ToolStripMenuItem();
            this.runSelectedToolStripMenuItem = new ToolStripMenuItem();
            this.runAllToolStripMenuItem = new ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 1, 0);
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
            this.splitContainer1.Location = new Point(403, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new Size(1323, 909);
            this.splitContainer1.SplitterDistance = 653;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageActual);
            this.tabControl1.Controls.Add(this.tabPageExpected);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(653, 909);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageActual
            // 
            this.tabPageActual.Location = new Point(4, 34);
            this.tabPageActual.Name = "tabPageActual";
            this.tabPageActual.Padding = new Padding(3);
            this.tabPageActual.Size = new Size(645, 871);
            this.tabPageActual.TabIndex = 0;
            this.tabPageActual.Text = "Acutal";
            this.tabPageActual.UseVisualStyleBackColor = true;
            // 
            // tabPageExpected
            // 
            this.tabPageExpected.Location = new Point(4, 34);
            this.tabPageExpected.Name = "tabPageExpected";
            this.tabPageExpected.Padding = new Padding(3);
            this.tabPageExpected.Size = new Size(645, 871);
            this.tabPageExpected.TabIndex = 1;
            this.tabPageExpected.Text = "Expected";
            this.tabPageExpected.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageXML);
            this.tabControl2.Controls.Add(this.tabPageStyle);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.Location = new Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(666, 909);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPageXML
            // 
            this.tabPageXML.Location = new Point(4, 34);
            this.tabPageXML.Name = "tabPageXML";
            this.tabPageXML.Padding = new Padding(3);
            this.tabPageXML.Size = new Size(658, 904);
            this.tabPageXML.TabIndex = 0;
            this.tabPageXML.Text = "XML";
            this.tabPageXML.UseVisualStyleBackColor = true;
            // 
            // tabPageStyle
            // 
            this.tabPageStyle.Location = new Point(4, 34);
            this.tabPageStyle.Name = "tabPageStyle";
            this.tabPageStyle.Padding = new Padding(3);
            this.tabPageStyle.Size = new Size(658, 871);
            this.tabPageStyle.TabIndex = 1;
            this.tabPageStyle.Text = "Style";
            this.tabPageStyle.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new Size(24, 24);
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.testToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(1729, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.loadToolStripMenuItem, this.closeToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.addToolStripMenuItem, this.removeToolStripMenuItem, this.renameToolStripMenuItem, this.runSelectedToolStripMenuItem, this.runAllToolStripMenuItem });
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new Size(58, 29);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new Size(230, 34);
            this.loadToolStripMenuItem.Text = "Load Directory";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new Size(230, 34);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = Keys.F2;
            this.addToolStripMenuItem.Size = new Size(270, 34);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new Size(270, 34);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new Size(270, 34);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // runSelectedToolStripMenuItem
            // 
            this.runSelectedToolStripMenuItem.Name = "runSelectedToolStripMenuItem";
            this.runSelectedToolStripMenuItem.ShortcutKeys = Keys.F5;
            this.runSelectedToolStripMenuItem.Size = new Size(270, 34);
            this.runSelectedToolStripMenuItem.Text = "Run Selected";
            // 
            // runAllToolStripMenuItem
            // 
            this.runAllToolStripMenuItem.Name = "runAllToolStripMenuItem";
            this.runAllToolStripMenuItem.ShortcutKeys = Keys.F6;
            this.runAllToolStripMenuItem.Size = new Size(270, 34);
            this.runAllToolStripMenuItem.Text = "Run All";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1729, 948);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPageActual;
        private TabPage tabPageExpected;
        private TabControl tabControl2;
        private TabPage tabPageXML;
        private TabPage tabPageStyle;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem runSelectedToolStripMenuItem;
        private ToolStripMenuItem runAllToolStripMenuItem;
    }
}
