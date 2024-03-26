using Leagueinator.Components;

namespace Leagueinator.Forms {
    partial class FormMain : Form {
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
            this.eventPanel = new EventPanel();
            this.menuStrip1 = new MenuStrip();
            this.viewTableToolStripMenuItem = new ToolStripMenuItem();
            this.dataTableToolStripMenuItem = new ToolStripMenuItem();
            this.matchesToolStripMenuItem = new ToolStripMenuItem();
            this.teamsToolStripMenuItem = new ToolStripMenuItem();
            this.membersToolStripMenuItem = new ToolStripMenuItem();
            this.idleToolStripMenuItem = new ToolStripMenuItem();
            this.playersToolStripMenuItem = new ToolStripMenuItem();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.eventsToolStripMenuItem = new ToolStripMenuItem();
            this.roundsToolStripMenuItem = new ToolStripMenuItem();
            this.resultsToolStripMenuItem = new ToolStripMenuItem();
            this.eventToolStripMenuItem = new ToolStripMenuItem();
            this.debugToolStripMenuItem = new ToolStripMenuItem();
            this.matchTableToolStripMenuItem = new ToolStripMenuItem();
            this.roundToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventPanel
            // 
            this.eventPanel.Dock = DockStyle.Fill;
            this.eventPanel.EventRow = null;
            this.eventPanel.Location = new Point(0, 33);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.Size = new Size(1896, 762);
            this.eventPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new Size(24, 24);
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.viewTableToolStripMenuItem, this.debugToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(1896, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewTableToolStripMenuItem
            // 
            this.viewTableToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.dataTableToolStripMenuItem, this.resultsToolStripMenuItem });
            this.viewTableToolStripMenuItem.Name = "viewTableToolStripMenuItem";
            this.viewTableToolStripMenuItem.Size = new Size(65, 29);
            this.viewTableToolStripMenuItem.Text = "View";
            // 
            // dataTableToolStripMenuItem
            // 
            this.dataTableToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.matchesToolStripMenuItem, this.teamsToolStripMenuItem, this.membersToolStripMenuItem, this.idleToolStripMenuItem, this.playersToolStripMenuItem, this.settingsToolStripMenuItem, this.eventsToolStripMenuItem, this.roundsToolStripMenuItem });
            this.dataTableToolStripMenuItem.Name = "dataTableToolStripMenuItem";
            this.dataTableToolStripMenuItem.Size = new Size(270, 34);
            this.dataTableToolStripMenuItem.Text = "Data-Table";
            // 
            // matchesToolStripMenuItem
            // 
            this.matchesToolStripMenuItem.Name = "matchesToolStripMenuItem";
            this.matchesToolStripMenuItem.Size = new Size(270, 34);
            this.matchesToolStripMenuItem.Text = "Matches";
            this.matchesToolStripMenuItem.Click += this.HndMenuViewMatches;
            // 
            // teamsToolStripMenuItem
            // 
            this.teamsToolStripMenuItem.Name = "teamsToolStripMenuItem";
            this.teamsToolStripMenuItem.Size = new Size(270, 34);
            this.teamsToolStripMenuItem.Text = "Teams";
            this.teamsToolStripMenuItem.Click += this.HndMenuViewTeams;
            // 
            // membersToolStripMenuItem
            // 
            this.membersToolStripMenuItem.Name = "membersToolStripMenuItem";
            this.membersToolStripMenuItem.Size = new Size(270, 34);
            this.membersToolStripMenuItem.Text = "Members";
            this.membersToolStripMenuItem.Click += this.HndMenuViewMembers;
            // 
            // idleToolStripMenuItem
            // 
            this.idleToolStripMenuItem.Name = "idleToolStripMenuItem";
            this.idleToolStripMenuItem.Size = new Size(270, 34);
            this.idleToolStripMenuItem.Text = "Idle";
            this.idleToolStripMenuItem.Click += this.HndMenuViewIdle;
            // 
            // playersToolStripMenuItem
            // 
            this.playersToolStripMenuItem.Name = "playersToolStripMenuItem";
            this.playersToolStripMenuItem.Size = new Size(270, 34);
            this.playersToolStripMenuItem.Text = "Players";
            this.playersToolStripMenuItem.Click += this.HndMenuViewPlayers;
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new Size(270, 34);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += this.HndMenuViewSettings;
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new Size(270, 34);
            this.eventsToolStripMenuItem.Text = "Events";
            this.eventsToolStripMenuItem.Click += this.HndMenuViewEvents;
            // 
            // roundsToolStripMenuItem
            // 
            this.roundsToolStripMenuItem.Name = "roundsToolStripMenuItem";
            this.roundsToolStripMenuItem.Size = new Size(270, 34);
            this.roundsToolStripMenuItem.Text = "Rounds";
            this.roundsToolStripMenuItem.Click += this.HndMenuViewRounds;
            // 
            // resultsToolStripMenuItem
            // 
            this.resultsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.roundToolStripMenuItem, this.eventToolStripMenuItem });
            this.resultsToolStripMenuItem.Name = "resultsToolStripMenuItem";
            this.resultsToolStripMenuItem.Size = new Size(270, 34);
            this.resultsToolStripMenuItem.Text = "Results";
            // 
            // eventToolStripMenuItem
            // 
            this.eventToolStripMenuItem.Name = "eventToolStripMenuItem";
            this.eventToolStripMenuItem.Size = new Size(270, 34);
            this.eventToolStripMenuItem.Text = "Event";
            this.eventToolStripMenuItem.Click += this.HndMenuViewEventResults;
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.matchTableToolStripMenuItem });
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new Size(82, 29);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // matchTableToolStripMenuItem
            // 
            this.matchTableToolStripMenuItem.Name = "matchTableToolStripMenuItem";
            this.matchTableToolStripMenuItem.Size = new Size(270, 34);
            this.matchTableToolStripMenuItem.Text = "MatchTable";
            this.matchTableToolStripMenuItem.Click += this.HndMenuViewMatches;
            // 
            // roundToolStripMenuItem
            // 
            this.roundToolStripMenuItem.Name = "roundToolStripMenuItem";
            this.roundToolStripMenuItem.Size = new Size(270, 34);
            this.roundToolStripMenuItem.Text = "Round";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1896, 795);
            this.Controls.Add(this.eventPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Components.EventPanel eventPanel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem viewTableToolStripMenuItem;
        private ToolStripMenuItem teamsToolStripMenuItem;
        private ToolStripMenuItem playersToolStripMenuItem;
        private ToolStripMenuItem membersToolStripMenuItem;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripMenuItem matchTableToolStripMenuItem;
        private ToolStripMenuItem eventsToolStripMenuItem;
        private ToolStripMenuItem roundsToolStripMenuItem;
        private ToolStripMenuItem matchesToolStripMenuItem;
        private ToolStripMenuItem idleToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem dataTableToolStripMenuItem;
        private ToolStripMenuItem resultsToolStripMenuItem;
        private ToolStripMenuItem eventToolStripMenuItem;
        private ToolStripMenuItem roundToolStripMenuItem;
    }
}
