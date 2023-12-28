namespace Leagueinator.App.Forms.Main {
    partial class FormMain {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param TagName="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.newToolStripMenuItem = new ToolStripMenuItem();
            this.loadToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new ToolStripMenuItem();
            this.printToolStripMenuItem = new ToolStripMenuItem();
            this.scoreCardToolStripMenuItem = new ToolStripMenuItem();
            this.printScoreCardToolStripMenuItem = new ToolStripMenuItem();
            this.standingsToolStripMenuItem = new ToolStripMenuItem();
            this.closeToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.eventsToolStripMenuItem = new ToolStripMenuItem();
            this.selectEventToolStripMenuItem = new ToolStripMenuItem();
            this.addEventToolStripMenuItem = new ToolStripMenuItem();
            this.playersToolStripMenuItem = new ToolStripMenuItem();
            this.addToolStripMenuItem = new ToolStripMenuItem();
            this.clearToolStripMenuItem = new ToolStripMenuItem();
            this.resetToolStripMenuItem = new ToolStripMenuItem();
            this.copyPreviousRoundToolStripMenuItem = new ToolStripMenuItem();
            this.randomizeToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.assignLanesToolStripMenuItem = new ToolStripMenuItem();
            this.scrambleToolStripMenuItem = new ToolStripMenuItem();
            this.viewToolStripMenuItem = new ToolStripMenuItem();
            this.reportToolStripMenuItem = new ToolStripMenuItem();
            this.roundReportToolStripMenuItem = new ToolStripMenuItem();
            this.eventReportToolStripMenuItem = new ToolStripMenuItem();
            this.devToolStripMenuItem = new ToolStripMenuItem();
            this.printLeagueToolStripMenuItem = new ToolStripMenuItem();
            this.printCurrentEventToolStripMenuItem = new ToolStripMenuItem();
            this.isSavedToolStripMenuItem = new ToolStripMenuItem();
            this.roundsCollectionToolStripMenuItem = new ToolStripMenuItem();
            this.currentRoundToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.eventPanel = new Components.EventPanel();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog = new PrintPreviewDialog();
            this.printDialog = new PrintDialog();
            this.addRowToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new Size(24, 24);
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.eventsToolStripMenuItem, this.playersToolStripMenuItem, this.viewToolStripMenuItem, this.devToolStripMenuItem, this.helpToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(1278, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.newToolStripMenuItem, this.loadToolStripMenuItem, this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.printToolStripMenuItem, this.closeToolStripMenuItem, this.exitToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new Size(212, 34);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += this.File_New;
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new Size(212, 34);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += this.File_Load;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            this.saveToolStripMenuItem.Size = new Size(212, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += this.File_Save;
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new Size(212, 34);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += this.File_SaveAs;
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.scoreCardToolStripMenuItem, this.printScoreCardToolStripMenuItem, this.standingsToolStripMenuItem });
            this.printToolStripMenuItem.Enabled = false;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new Size(212, 34);
            this.printToolStripMenuItem.Text = "Print";
            // 
            // scoreCardToolStripMenuItem
            // 
            this.scoreCardToolStripMenuItem.Name = "scoreCardToolStripMenuItem";
            this.scoreCardToolStripMenuItem.Size = new Size(265, 34);
            this.scoreCardToolStripMenuItem.Text = "Preview Score Card";
            this.scoreCardToolStripMenuItem.Click += this.File_Print_Preview;
            // 
            // printScoreCardToolStripMenuItem
            // 
            this.printScoreCardToolStripMenuItem.Name = "printScoreCardToolStripMenuItem";
            this.printScoreCardToolStripMenuItem.Size = new Size(265, 34);
            this.printScoreCardToolStripMenuItem.Text = "Print Score Card";
            this.printScoreCardToolStripMenuItem.Click += this.File_Print_Card;
            // 
            // standingsToolStripMenuItem
            // 
            this.standingsToolStripMenuItem.Name = "standingsToolStripMenuItem";
            this.standingsToolStripMenuItem.Size = new Size(265, 34);
            this.standingsToolStripMenuItem.Text = "Standings";
            this.standingsToolStripMenuItem.Click += this.File_Print_Standings;
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new Size(212, 34);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += this.File_Close;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(212, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += this.File_Exit;
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.selectEventToolStripMenuItem, this.addEventToolStripMenuItem });
            this.eventsToolStripMenuItem.Enabled = false;
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new Size(79, 29);
            this.eventsToolStripMenuItem.Text = "Events";
            // 
            // selectEventToolStripMenuItem
            // 
            this.selectEventToolStripMenuItem.Name = "selectEventToolStripMenuItem";
            this.selectEventToolStripMenuItem.Size = new Size(208, 34);
            this.selectEventToolStripMenuItem.Text = "Select Event";
            this.selectEventToolStripMenuItem.Click += this.Events_SelectEvent;
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new Size(208, 34);
            this.addEventToolStripMenuItem.Text = "Add Event";
            this.addEventToolStripMenuItem.Click += this.Events_AddEvent;
            // 
            // playersToolStripMenuItem
            // 
            this.playersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.addToolStripMenuItem, this.clearToolStripMenuItem, this.resetToolStripMenuItem, this.copyPreviousRoundToolStripMenuItem, this.randomizeToolStripMenuItem, this.toolStripSeparator1, this.assignLanesToolStripMenuItem, this.scrambleToolStripMenuItem });
            this.playersToolStripMenuItem.Enabled = false;
            this.playersToolStripMenuItem.Name = "playersToolStripMenuItem";
            this.playersToolStripMenuItem.Size = new Size(83, 29);
            this.playersToolStripMenuItem.Text = "Players";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new Size(285, 34);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += this.Players_Add;
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new Size(285, 34);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += this.Players_Clear;
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new Size(285, 34);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += this.Players_Reset;
            // 
            // copyPreviousRoundToolStripMenuItem
            // 
            this.copyPreviousRoundToolStripMenuItem.Name = "copyPreviousRoundToolStripMenuItem";
            this.copyPreviousRoundToolStripMenuItem.Size = new Size(285, 34);
            this.copyPreviousRoundToolStripMenuItem.Text = "Copy Previous Round";
            this.copyPreviousRoundToolStripMenuItem.Click += this.Players_Copy;
            // 
            // randomizeToolStripMenuItem
            // 
            this.randomizeToolStripMenuItem.Name = "randomizeToolStripMenuItem";
            this.randomizeToolStripMenuItem.Size = new Size(285, 34);
            this.randomizeToolStripMenuItem.Text = "Randomize";
            this.randomizeToolStripMenuItem.Click += this.Players_Randomize;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(282, 6);
            // 
            // assignLanesToolStripMenuItem
            // 
            this.assignLanesToolStripMenuItem.Name = "assignLanesToolStripMenuItem";
            this.assignLanesToolStripMenuItem.Size = new Size(285, 34);
            this.assignLanesToolStripMenuItem.Text = "Assign Lanes";
            this.assignLanesToolStripMenuItem.Click += this.Players_AssignLanes;
            // 
            // scrambleToolStripMenuItem
            // 
            this.scrambleToolStripMenuItem.Name = "scrambleToolStripMenuItem";
            this.scrambleToolStripMenuItem.Size = new Size(285, 34);
            this.scrambleToolStripMenuItem.Text = "Scramble";
            this.scrambleToolStripMenuItem.Click += this.Players_Scramble;
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.reportToolStripMenuItem, this.roundReportToolStripMenuItem, this.eventReportToolStripMenuItem });
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new Size(65, 29);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new Size(270, 34);
            this.reportToolStripMenuItem.Text = "Report";
            this.reportToolStripMenuItem.Click += this.View_Report;
            // 
            // roundReportToolStripMenuItem
            // 
            this.roundReportToolStripMenuItem.Name = "roundReportToolStripMenuItem";
            this.roundReportToolStripMenuItem.Size = new Size(270, 34);
            this.roundReportToolStripMenuItem.Text = "Round Report";
            this.roundReportToolStripMenuItem.Click += this.View_RoundSummary;
            // 
            // eventReportToolStripMenuItem
            // 
            this.eventReportToolStripMenuItem.Name = "eventReportToolStripMenuItem";
            this.eventReportToolStripMenuItem.Size = new Size(270, 34);
            this.eventReportToolStripMenuItem.Text = "Event Report";
            this.eventReportToolStripMenuItem.Click += this.View_EventSummary;
            // 
            // devToolStripMenuItem
            // 
            this.devToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.printLeagueToolStripMenuItem, this.printCurrentEventToolStripMenuItem, this.isSavedToolStripMenuItem, this.roundsCollectionToolStripMenuItem, this.currentRoundToolStripMenuItem, this.addRowToolStripMenuItem });
            this.devToolStripMenuItem.Name = "devToolStripMenuItem";
            this.devToolStripMenuItem.Size = new Size(59, 29);
            this.devToolStripMenuItem.Text = "Dev";
            // 
            // printLeagueToolStripMenuItem
            // 
            this.printLeagueToolStripMenuItem.Name = "printLeagueToolStripMenuItem";
            this.printLeagueToolStripMenuItem.Size = new Size(270, 34);
            this.printLeagueToolStripMenuItem.Text = "Print League";
            this.printLeagueToolStripMenuItem.Click += this.Dev_PrintLeague;
            // 
            // printCurrentEventToolStripMenuItem
            // 
            this.printCurrentEventToolStripMenuItem.Name = "printCurrentEventToolStripMenuItem";
            this.printCurrentEventToolStripMenuItem.Size = new Size(270, 34);
            this.printCurrentEventToolStripMenuItem.Text = "Print Current Event";
            this.printCurrentEventToolStripMenuItem.Click += this.Dev_PrintCurrentEvent;
            // 
            // isSavedToolStripMenuItem
            // 
            this.isSavedToolStripMenuItem.Name = "isSavedToolStripMenuItem";
            this.isSavedToolStripMenuItem.Size = new Size(270, 34);
            this.isSavedToolStripMenuItem.Text = "Is Saved";
            this.isSavedToolStripMenuItem.Click += this.Dev_IsSaved;
            // 
            // roundsCollectionToolStripMenuItem
            // 
            this.roundsCollectionToolStripMenuItem.Name = "roundsCollectionToolStripMenuItem";
            this.roundsCollectionToolStripMenuItem.Size = new Size(270, 34);
            this.roundsCollectionToolStripMenuItem.Text = "Rounds Collection";
            this.roundsCollectionToolStripMenuItem.Click += this.Dev_HashCode;
            // 
            // currentRoundToolStripMenuItem
            // 
            this.currentRoundToolStripMenuItem.Name = "currentRoundToolStripMenuItem";
            this.currentRoundToolStripMenuItem.Size = new Size(270, 34);
            this.currentRoundToolStripMenuItem.Text = "Current Round";
            this.currentRoundToolStripMenuItem.Click += this.currentRoundToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(65, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(164, 34);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += this.Help_About;
            // 
            // eventPanel
            // 
            this.eventPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.eventPanel.AutoSize = true;
            this.eventPanel.LeagueEvent = null;
            this.eventPanel.Location = new Point(0, 37);
            this.eventPanel.Margin = new Padding(3, 4, 3, 4);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.Size = new Size(1278, 708);
            this.eventPanel.TabIndex = 1;
            this.eventPanel.Visible = false;
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
            this.printPreviewDialog.ClientSize = new Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = (Icon)resources.GetObject("printPreviewDialog.Icon");
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new Size(270, 34);
            this.addRowToolStripMenuItem.Text = "Add Row";
            this.addRowToolStripMenuItem.Click += this.addRowToolStripMenuItem_Click;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1278, 744);
            this.Controls.Add(this.eventPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new Size(1000, 800);
            this.Name = "FormMain";
            this.Text = "Leagueinator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem eventsToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem devToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem scoreCardToolStripMenuItem;
        private Components.EventPanel eventPanel;
        private ToolStripMenuItem addEventToolStripMenuItem;
        private ToolStripMenuItem printCurrentEventToolStripMenuItem;
        private ToolStripMenuItem printLeagueToolStripMenuItem;
        private ToolStripMenuItem isSavedToolStripMenuItem;
        private ToolStripMenuItem roundsCollectionToolStripMenuItem;
        private ToolStripMenuItem selectEventToolStripMenuItem;
        private ToolStripMenuItem reportToolStripMenuItem;
        private ToolStripMenuItem roundReportToolStripMenuItem;
        private ToolStripMenuItem eventReportToolStripMenuItem;
        private ToolStripMenuItem playersToolStripMenuItem;
        private ToolStripMenuItem copyPreviousRoundToolStripMenuItem;
        private ToolStripMenuItem randomizeToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private System.Drawing.Printing.PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;
        private PrintDialog printDialog;
        private ToolStripMenuItem printScoreCardToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem scrambleToolStripMenuItem;
        private ToolStripMenuItem assignLanesToolStripMenuItem;
        private ToolStripMenuItem standingsToolStripMenuItem;
        private ToolStripMenuItem currentRoundToolStripMenuItem;
        private ToolStripMenuItem addRowToolStripMenuItem;
    }
}
