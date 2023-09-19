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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            printToolStripMenuItem = new ToolStripMenuItem();
            scoreCardToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            eventsToolStripMenuItem = new ToolStripMenuItem();
            selectEventToolStripMenuItem = new ToolStripMenuItem();
            addEventToolStripMenuItem = new ToolStripMenuItem();
            addPlayersToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            reportToolStripMenuItem = new ToolStripMenuItem();
            devToolStripMenuItem = new ToolStripMenuItem();
            printLeagueToolStripMenuItem = new ToolStripMenuItem();
            printCurrentEventToolStripMenuItem = new ToolStripMenuItem();
            isSavedToolStripMenuItem = new ToolStripMenuItem();
            roundsCollectionToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            this.eventPanel = new Components.EventPanel.EventPanel();
            roundReportToolStripMenuItem = new ToolStripMenuItem();
            eventReportToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, eventsToolStripMenuItem, viewToolStripMenuItem, devToolStripMenuItem, helpToolStripMenuItem });
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
            newToolStripMenuItem.Size = new Size(212, 34);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += this.File_New;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(212, 34);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += this.File_Load;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(212, 34);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += this.File_Save;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(212, 34);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += this.File_SaveAs;
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { scoreCardToolStripMenuItem });
            printToolStripMenuItem.Enabled = false;
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.Size = new Size(212, 34);
            printToolStripMenuItem.Text = "Print";
            printToolStripMenuItem.Click += this.File_Print;
            // 
            // scoreCardToolStripMenuItem
            // 
            scoreCardToolStripMenuItem.Name = "scoreCardToolStripMenuItem";
            scoreCardToolStripMenuItem.Size = new Size(200, 34);
            scoreCardToolStripMenuItem.Text = "Score Card";
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(212, 34);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += this.File_Close;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(212, 34);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += this.File_Exit;
            // 
            // eventsToolStripMenuItem
            // 
            eventsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { selectEventToolStripMenuItem, addEventToolStripMenuItem, addPlayersToolStripMenuItem });
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
            addEventToolStripMenuItem.Text = "Add Event";
            addEventToolStripMenuItem.Click += this.Events_AddEvent;
            // 
            // addPlayersToolStripMenuItem
            // 
            addPlayersToolStripMenuItem.Name = "addPlayersToolStripMenuItem";
            addPlayersToolStripMenuItem.Size = new Size(208, 34);
            addPlayersToolStripMenuItem.Text = "Add Players";
            addPlayersToolStripMenuItem.Click += this.Events_AddPlayers;
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
            reportToolStripMenuItem.Size = new Size(270, 34);
            reportToolStripMenuItem.Text = "Report";
            reportToolStripMenuItem.Click += this.View_Summary;
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
            // roundReportToolStripMenuItem
            // 
            roundReportToolStripMenuItem.Name = "roundReportToolStripMenuItem";
            roundReportToolStripMenuItem.Size = new Size(270, 34);
            roundReportToolStripMenuItem.Text = "Round Report";
            roundReportToolStripMenuItem.Click += this.View_RoundSummary;
            // 
            // eventReportToolStripMenuItem
            // 
            eventReportToolStripMenuItem.Name = "eventReportToolStripMenuItem";
            eventReportToolStripMenuItem.Size = new Size(270, 34);
            eventReportToolStripMenuItem.Text = "Event Report";
            eventReportToolStripMenuItem.Click += this.View_EventSummary;
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
        private ToolStripMenuItem addPlayersToolStripMenuItem;
        private ToolStripMenuItem printCurrentEventToolStripMenuItem;
        private ToolStripMenuItem printLeagueToolStripMenuItem;
        private ToolStripMenuItem isSavedToolStripMenuItem;
        private ToolStripMenuItem roundsCollectionToolStripMenuItem;
        private ToolStripMenuItem selectEventToolStripMenuItem;
        private ToolStripMenuItem reportToolStripMenuItem;
        private ToolStripMenuItem roundReportToolStripMenuItem;
        private ToolStripMenuItem eventReportToolStripMenuItem;
    }
}
