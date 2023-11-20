namespace PrinterTestForm {
    partial class Form1 {
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
            this.canvas = new CustomCanvas();
            menuStrip1 = new MenuStrip();
            flexDirectionToolStripMenuItem = new ToolStripMenuItem();
            rowToolStripMenuItem = new ToolStripMenuItem();
            rowReverseToolStripMenuItem = new ToolStripMenuItem();
            columnToolStripMenuItem = new ToolStripMenuItem();
            columnReverseToolStripMenuItem = new ToolStripMenuItem();
            flexAlignToolStripMenuItem = new ToolStripMenuItem();
            startToolStripMenuItem = new ToolStripMenuItem();
            endToolStripMenuItem = new ToolStripMenuItem();
            centerToolStripMenuItem = new ToolStripMenuItem();
            spaceEvenlyToolStripMenuItem = new ToolStripMenuItem();
            spaceAroundToolStripMenuItem = new ToolStripMenuItem();
            spaceBetweenToolStripMenuItem = new ToolStripMenuItem();
            alignItemsToolStripMenuItem = new ToolStripMenuItem();
            flexStartToolStripMenuItem = new ToolStripMenuItem();
            flexEndToolStripMenuItem = new ToolStripMenuItem();
            centerToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // printerRoot
            // 
            this.canvas.Location = new Point(64, 117);
            this.canvas.Name = "printerRoot";
            this.canvas.Size = new Size(1200, 800);
            this.canvas.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { flexDirectionToolStripMenuItem, flexAlignToolStripMenuItem, alignItemsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1307, 33);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // flexDirectionToolStripMenuItem
            // 
            flexDirectionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { rowToolStripMenuItem, rowReverseToolStripMenuItem, columnToolStripMenuItem, columnReverseToolStripMenuItem });
            flexDirectionToolStripMenuItem.Name = "flexDirectionToolStripMenuItem";
            flexDirectionToolStripMenuItem.Size = new Size(134, 29);
            flexDirectionToolStripMenuItem.Text = "Flex Direction";
            // 
            // rowToolStripMenuItem
            // 
            rowToolStripMenuItem.Name = "rowToolStripMenuItem";
            rowToolStripMenuItem.Size = new Size(241, 34);
            rowToolStripMenuItem.Text = "Row";
            rowToolStripMenuItem.Click += this.Menu_Direction_Row;
            // 
            // rowReverseToolStripMenuItem
            // 
            rowReverseToolStripMenuItem.Name = "rowReverseToolStripMenuItem";
            rowReverseToolStripMenuItem.Size = new Size(241, 34);
            rowReverseToolStripMenuItem.Text = "Row Reverse";
            rowReverseToolStripMenuItem.Click += this.Menu_Directon_RowReverse;
            // 
            // columnToolStripMenuItem
            // 
            columnToolStripMenuItem.Name = "columnToolStripMenuItem";
            columnToolStripMenuItem.Size = new Size(241, 34);
            columnToolStripMenuItem.Text = "Column";
            columnToolStripMenuItem.Click += this.Menu_Directon_Column;
            // 
            // columnReverseToolStripMenuItem
            // 
            columnReverseToolStripMenuItem.Name = "columnReverseToolStripMenuItem";
            columnReverseToolStripMenuItem.Size = new Size(241, 34);
            columnReverseToolStripMenuItem.Text = "Column Reverse";
            columnReverseToolStripMenuItem.Click += this.Menu_Directon_ColumnReverse;
            // 
            // flexAlignToolStripMenuItem
            // 
            flexAlignToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startToolStripMenuItem, endToolStripMenuItem, centerToolStripMenuItem, spaceEvenlyToolStripMenuItem, spaceAroundToolStripMenuItem, spaceBetweenToolStripMenuItem });
            flexAlignToolStripMenuItem.Name = "flexAlignToolStripMenuItem";
            flexAlignToolStripMenuItem.Size = new Size(145, 29);
            flexAlignToolStripMenuItem.Text = "Justify Content";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(232, 34);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += this.Menu_Justify_FlexStart;
            // 
            // endToolStripMenuItem
            // 
            endToolStripMenuItem.Name = "endToolStripMenuItem";
            endToolStripMenuItem.Size = new Size(232, 34);
            endToolStripMenuItem.Text = "End";
            endToolStripMenuItem.Click += this.Menu_Justify_FlexEnd;
            // 
            // centerToolStripMenuItem
            // 
            centerToolStripMenuItem.Name = "centerToolStripMenuItem";
            centerToolStripMenuItem.Size = new Size(232, 34);
            centerToolStripMenuItem.Text = "Center";
            centerToolStripMenuItem.Click += this.Menu_Justify_Center;
            // 
            // spaceEvenlyToolStripMenuItem
            // 
            spaceEvenlyToolStripMenuItem.Name = "spaceEvenlyToolStripMenuItem";
            spaceEvenlyToolStripMenuItem.Size = new Size(232, 34);
            spaceEvenlyToolStripMenuItem.Text = "Space Evenly";
            spaceEvenlyToolStripMenuItem.Click += this.Menu_Justify_SpaceEvenly;
            // 
            // spaceAroundToolStripMenuItem
            // 
            spaceAroundToolStripMenuItem.Name = "spaceAroundToolStripMenuItem";
            spaceAroundToolStripMenuItem.Size = new Size(232, 34);
            spaceAroundToolStripMenuItem.Text = "Space Around";
            spaceAroundToolStripMenuItem.Click += this.Menu_Justify_SpaceAround;
            // 
            // spaceBetweenToolStripMenuItem
            // 
            spaceBetweenToolStripMenuItem.Name = "spaceBetweenToolStripMenuItem";
            spaceBetweenToolStripMenuItem.Size = new Size(232, 34);
            spaceBetweenToolStripMenuItem.Text = "Space Between";
            spaceBetweenToolStripMenuItem.Click += this.Menu_Justify_SpaceBetween;
            // 
            // alignItemsToolStripMenuItem
            // 
            alignItemsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { flexStartToolStripMenuItem, flexEndToolStripMenuItem, centerToolStripMenuItem1 });
            alignItemsToolStripMenuItem.Name = "alignItemsToolStripMenuItem";
            alignItemsToolStripMenuItem.Size = new Size(118, 29);
            alignItemsToolStripMenuItem.Text = "Align Items";
            // 
            // flexStartToolStripMenuItem
            // 
            flexStartToolStripMenuItem.Name = "flexStartToolStripMenuItem";
            flexStartToolStripMenuItem.Size = new Size(185, 34);
            flexStartToolStripMenuItem.Text = "Flex Start";
            flexStartToolStripMenuItem.Click += this.Menu_Align_Start;
            // 
            // flexEndToolStripMenuItem
            // 
            flexEndToolStripMenuItem.Name = "flexEndToolStripMenuItem";
            flexEndToolStripMenuItem.Size = new Size(185, 34);
            flexEndToolStripMenuItem.Text = "Flex End";
            flexEndToolStripMenuItem.Click += this.Menu_Align_End;
            // 
            // centerToolStripMenuItem1
            // 
            centerToolStripMenuItem1.Name = "centerToolStripMenuItem1";
            centerToolStripMenuItem1.Size = new Size(185, 34);
            centerToolStripMenuItem1.Text = "Center";
            centerToolStripMenuItem1.Click += this.Menu_Align_Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1307, 942);
            this.Controls.Add(this.canvas);
            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private CustomCanvas canvas;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem flexDirectionToolStripMenuItem;
        private ToolStripMenuItem rowToolStripMenuItem;
        private ToolStripMenuItem rowReverseToolStripMenuItem;
        private ToolStripMenuItem columnToolStripMenuItem;
        private ToolStripMenuItem columnReverseToolStripMenuItem;
        private ToolStripMenuItem flexAlignToolStripMenuItem;
        private ToolStripMenuItem endToolStripMenuItem;
        private ToolStripMenuItem centerToolStripMenuItem;
        private ToolStripMenuItem spaceEvenlyToolStripMenuItem;
        private ToolStripMenuItem spaceAroundToolStripMenuItem;
        private ToolStripMenuItem spaceBetweenToolStripMenuItem;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem alignItemsToolStripMenuItem;
        private ToolStripMenuItem flexStartToolStripMenuItem;
        private ToolStripMenuItem flexEndToolStripMenuItem;
        private ToolStripMenuItem centerToolStripMenuItem1;
    }
}