namespace Leagueinator.App.Forms.Main {
    partial class FormMain {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            printToolStripMenuItem = new ToolStripMenuItem();
            scoreCardToolStripMenuItem = new ToolStripMenuItem();
            printScoreCardToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            eventsToolStripMenuItem = new ToolStripMenuItem();
            selectEventToolStripMenuItem = new ToolStripMenuItem();
            addEventToolStripMenuItem = new ToolStripMenuItem();
            playersToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            copyPreviousRoundToolStripMenuItem = new ToolStripMenuItem();
            randomizeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            assignLanesToolStripMenuItem = new ToolStripMenuItem();
            scrambleToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            reportToolStripMenuItem = new ToolStripMenuItem();
            roundReportToolStripMenuItem = new ToolStripMenuItem();
            eventReportToolStripMenuItem = new ToolStripMenuItem();
            devToolStripMenuItem = new ToolStripMenuItem();
            printLeagueToolStripMenuItem = new ToolStripMenuItem();
            printCurrentEventToolStripMenuItem = new ToolStripMenuItem();
            isSavedToolStripMenuItem = new ToolStripMenuItem();
            roundsCollectionToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            this.eventPanel = new Components.EventPanel.EventPanel();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            printPreviewDialog = new PrintPreviewDialog();
            printDialog = new PrintDialog();
            standingsToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, eventsToolStripMenuItem, playersToolStripMenuItem, viewToolStripMenuItem, devToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1278, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, loadToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, printToolStripMenuItem, closeToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(270, 34);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += this.File_New;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(270, 34);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += this.File_Load;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(270, 34);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += this.File_Save;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(270, 34);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += this.File_SaveAs;
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { scoreCardToolStripMenuItem, printScoreCardToolStripMenuItem, standingsToolStripMenuItem });
            printToolStripMenuItem.Enabled = false;
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.Size = new Size(270, 34);
            printToolStripMenuItem.Text = "Print";
            // 
            // scoreCardToolStripMenuItem
            // 
            scoreCardToolStripMenuItem.Name = "scoreCardToolStripMenuItem";
            scoreCardToolStripMenuItem.Size = new Size(270, 34);
            scoreCardToolStripMenuItem.Text = "Preview Score Card";
            scoreCardToolStripMenuItem.Click += this.File_Print_Preview;
            // 
            // printScoreCardToolStripMenuItem
            // 
            printScoreCardToolStripMenuItem.Name = "printScoreCardToolStripMenuItem";
            printScoreCardToolStripMenuItem.Size = new Size(270, 34);
            printScoreCardToolStripMenuItem.Text = "Print Score Card";
            printScoreCardToolStripMenuItem.Click += this.File_Print_Card;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(270, 34);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += this.File_Close;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(270, 34);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += this.File_Exit;
            // 
            // eventsToolStripMenuItem
            // 
            eventsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { selectEventToolStripMenuItem, addEventToolStripMenuItem });
            eventsToolStripMenuItem.Enabled = false;
            eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            eventsToolStripMenuItem.Size = new Size(79, 29);
            eventsToolStripMenuItem.Text = "Events";
            // 
            // selectEventToolStripMenuItem
            // 
            selectEventToolStripMenuItem.Name = "selectEventToolStripMenuItem";
            selectEventToolStripMenuItem.Size = new Size(208, 34);
            selectEventToolStripMenuItem.Text = "Select Event";
            selectEventToolStripMenuItem.Click += this.Events_SelectEvent;
            // 
            // addEventToolStripMenuItem
            // 
            addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            addEventToolStripMenuItem.Size = new Size(208, 34);
            addEventToolStripMenuItem.Text = "AddChild Event";
            addEventToolStripMenuItem.Click += this.Events_AddEvent;
            // 
            // playersToolStripMenuItem
            // 
            playersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem, clearToolStripMenuItem, resetToolStripMenuItem, copyPreviousRoundToolStripMenuItem, randomizeToolStripMenuItem, toolStripSeparator1, assignLanesToolStripMenuItem, scrambleToolStripMenuItem });
            playersToolStripMenuItem.Enabled = false;
            playersToolStripMenuItem.Name = "playersToolStripMenuItem";
            playersToolStripMenuItem.Size = new Size(83, 29);
            playersToolStripMenuItem.Text = "Players";
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(285, 34);
            addToolStripMenuItem.Text = "AddChild";
            addToolStripMenuItem.Click += this.Players_Add;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(285, 34);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += this.Players_Clear;
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(285, 34);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += this.Players_Reset;
            // 
            // copyPreviousRoundToolStripMenuItem
            // 
            copyPreviousRoundToolStripMenuItem.Name = "copyPreviousRoundToolStripMenuItem";
            copyPreviousRoundToolStripMenuItem.Size = new Size(285, 34);
            copyPreviousRoundToolStripMenuItem.Text = "Copy Previous Round";
            copyPreviousRoundToolStripMenuItem.Click += this.Players_Copy;
            // 
            // randomizeToolStripMenuItem
            // 
            randomizeToolStripMenuItem.Name = "randomizeToolStripMenuItem";
            randomizeToolStripMenuItem.Size = new Size(285, 34);
            randomizeToolStripMenuItem.Text = "Randomize";
            randomizeToolStripMenuItem.Click += this.Players_Randomize;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(282, 6);
            // 
            // assignLanesToolStripMenuItem
            // 
            assignLanesToolStripMenuItem.Name = "assignLanesToolStripMenuItem";
            assignLanesToolStripMenuItem.Size = new Size(285, 34);
            assignLanesToolStripMenuItem.Text = "Assign Lanes";
            assignLanesToolStripMenuItem.Click += this.Players_AssignLanes;
            // 
            // scrambleToolStripMenuItem
            // 
            scrambleToolStripMenuItem.Name = "scrambleToolStripMenuItem";
            scrambleToolStripMenuItem.Size = new Size(285, 34);
            scrambleToolStripMenuItem.Text = "Scramble";
            scrambleToolStripMenuItem.Click += this.Players_Scramble;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { reportToolStripMenuItem, roundReportToolStripMenuItem, eventReportToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(65, 29);
            viewToolStripMenuItem.Text = "View";
            // 
            // reportToolStripMenuItem
            // 
            reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            reportToolStripMenuItem.Size = new Size(224, 34);
            reportToolStripMenuItem.Text = "Report";
            reportToolStripMenuItem.Click += this.View_Report;
            // 
            // roundReportToolStripMenuItem
            // 
            roundReportToolStripMenuItem.Name = "roundReportToolStripMenuItem";
            roundReportToolStripMenuItem.Size = new Size(224, 34);
            roundReportToolStripMenuItem.Text = "Round Report";
            roundReportToolStripMenuItem.Click += this.View_RoundSummary;
            // 
            // eventReportToolStripMenuItem
            // 
            eventReportToolStripMenuItem.Name = "eventReportToolStripMenuItem";
            eventReportToolStripMenuItem.Size = new Size(224, 34);
            eventReportToolStripMenuItem.Text = "Event Report";
            eventReportToolStripMenuItem.Click += this.View_EventSummary;
            // 
            // devToolStripMenuItem
            // 
            devToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { printLeagueToolStripMenuItem, printCurrentEventToolStripMenuItem, isSavedToolStripMenuItem, roundsCollectionToolStripMenuItem });
            devToolStripMenuItem.Name = "devToolStripMenuItem";
            devToolStripMenuItem.Size = new Size(59, 29);
            devToolStripMenuItem.Text = "Dev";
            // 
            // printLeagueToolStripMenuItem
            // 
            printLeagueToolStripMenuItem.Name = "printLeagueToolStripMenuItem";
            printLeagueToolStripMenuItem.Size = new Size(261, 34);
            printLeagueToolStripMenuItem.Text = "Print League";
            printLeagueToolStripMenuItem.Click += this.Dev_PrintLeague;
            // 
            // printCurrentEventToolStripMenuItem
            // 
            printCurrentEventToolStripMenuItem.Name = "printCurrentEventToolStripMenuItem";
            printCurrentEventToolStripMenuItem.Size = new Size(261, 34);
            printCurrentEventToolStripMenuItem.Text = "Print Current Event";
            printCurrentEventToolStripMenuItem.Click += this.Dev_PrintCurrentEvent;
            // 
            // isSavedToolStripMenuItem
            // 
            isSavedToolStripMenuItem.Name = "isSavedToolStripMenuItem";
            isSavedToolStripMenuItem.Size = new Size(261, 34);
            isSavedToolStripMenuItem.Text = "Is Saved";
            isSavedToolStripMenuItem.Click += this.Dev_IsSaved;
            // 
            // roundsCollectionToolStripMenuItem
            // 
            roundsCollectionToolStripMenuItem.Name = "roundsCollectionToolStripMenuItem";
            roundsCollectionToolStripMenuItem.Size = new Size(261, 34);
            roundsCollectionToolStripMenuItem.Text = "Rounds Collection";
            roundsCollectionToolStripMenuItem.Click += this.Dev_HashCode;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(65, 29);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(164, 34);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += this.Help_About;
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
            printPreviewDialog.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog.ClientSize = new Size(400, 300);
            printPreviewDialog.Enabled = true;
            printPreviewDialog.Icon = (Icon)resources.GetObject("printPreviewDialog.Icon");
            printPreviewDialog.Name = "printPreviewDialog";
            printPreviewDialog.Visible = false;
            // 
            // printDialog
            // 
            printDialog.UseEXDialog = true;
            // 
            // standingsToolStripMenuItem
            // 
            standingsToolStripMenuItem.Name = "standingsToolStripMenuItem";
            standingsToolStripMenuItem.Size = new Size(270, 34);
            standingsToolStripMenuItem.Text = "Standings";
            standingsToolStripMenuItem.Click += this.File_Print_Standings;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1278, 744);
            this.Controls.Add(this.eventPanel);
            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;
            this.MinimumSize = new Size(1000, 800);
            this.Name = "FormMain";
            this.Text = "Leagueinator";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
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
        private Components.EventPanel.EventPanel eventPanel;
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
    }
}
