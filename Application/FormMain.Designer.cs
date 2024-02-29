namespace Leagueinator.Components {
    partial class FormMain : Form{
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
            this.eventPanel1 = new EventPanel();
            this.menuStrip1 = new MenuStrip();
            this.viewTableToolStripMenuItem = new ToolStripMenuItem();
            this.eventsToolStripMenuItem = new ToolStripMenuItem();
            this.roundsToolStripMenuItem = new ToolStripMenuItem();
            this.matchesToolStripMenuItem = new ToolStripMenuItem();
            this.teamsToolStripMenuItem = new ToolStripMenuItem();
            this.membersToolStripMenuItem = new ToolStripMenuItem();
            this.idleToolStripMenuItem = new ToolStripMenuItem();
            this.playersToolStripMenuItem = new ToolStripMenuItem();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.debugToolStripMenuItem = new ToolStripMenuItem();
            this.matchTableToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventPanel1
            // 
            this.eventPanel1.Dock = DockStyle.Fill;
            this.eventPanel1.Location = new Point(0, 33);
            this.eventPanel1.Name = "eventPanel1";
            this.eventPanel1.Size = new Size(1199, 762);
            this.eventPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new Size(24, 24);
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.viewTableToolStripMenuItem, this.debugToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(1199, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewTableToolStripMenuItem
            // 
            this.viewTableToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.eventsToolStripMenuItem, this.roundsToolStripMenuItem, this.matchesToolStripMenuItem, this.teamsToolStripMenuItem, this.membersToolStripMenuItem, this.idleToolStripMenuItem, this.playersToolStripMenuItem, this.settingsToolStripMenuItem });
            this.viewTableToolStripMenuItem.Name = "viewTableToolStripMenuItem";
            this.viewTableToolStripMenuItem.Size = new Size(110, 29);
            this.viewTableToolStripMenuItem.Text = "View Table";
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new Size(189, 34);
            this.eventsToolStripMenuItem.Text = "Events";
            this.eventsToolStripMenuItem.Click += this.HndMenuViewEvents;
            // 
            // roundsToolStripMenuItem
            // 
            this.roundsToolStripMenuItem.Name = "roundsToolStripMenuItem";
            this.roundsToolStripMenuItem.Size = new Size(189, 34);
            this.roundsToolStripMenuItem.Text = "Rounds";
            this.roundsToolStripMenuItem.Click += this.HndMenuViewRounds;
            // 
            // matchesToolStripMenuItem
            // 
            this.matchesToolStripMenuItem.Name = "matchesToolStripMenuItem";
            this.matchesToolStripMenuItem.Size = new Size(189, 34);
            this.matchesToolStripMenuItem.Text = "Matches";
            this.matchesToolStripMenuItem.Click += this.HndMenuViewMatches;
            // 
            // teamsToolStripMenuItem
            // 
            this.teamsToolStripMenuItem.Name = "teamsToolStripMenuItem";
            this.teamsToolStripMenuItem.Size = new Size(189, 34);
            this.teamsToolStripMenuItem.Text = "Teams";
            this.teamsToolStripMenuItem.Click += this.HndMenuViewTeams;
            // 
            // membersToolStripMenuItem
            // 
            this.membersToolStripMenuItem.Name = "membersToolStripMenuItem";
            this.membersToolStripMenuItem.Size = new Size(189, 34);
            this.membersToolStripMenuItem.Text = "Members";
            this.membersToolStripMenuItem.Click += this.HndMenuViewMembers;
            // 
            // idleToolStripMenuItem
            // 
            this.idleToolStripMenuItem.Name = "idleToolStripMenuItem";
            this.idleToolStripMenuItem.Size = new Size(189, 34);
            this.idleToolStripMenuItem.Text = "Idle";
            this.idleToolStripMenuItem.Click += this.HndMenuViewIdle;
            // 
            // playersToolStripMenuItem
            // 
            this.playersToolStripMenuItem.Name = "playersToolStripMenuItem";
            this.playersToolStripMenuItem.Size = new Size(189, 34);
            this.playersToolStripMenuItem.Text = "Players";
            this.playersToolStripMenuItem.Click += this.HndMenuViewPlayers;
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new Size(189, 34);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += this.HndMenuViewSettings;
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
            this.matchTableToolStripMenuItem.Size = new Size(203, 34);
            this.matchTableToolStripMenuItem.Text = "MatchTable";
            this.matchTableToolStripMenuItem.Click += this.HndMenuViewMatches;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1199, 795);
            this.Controls.Add(this.eventPanel1);
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

        private Components.EventPanel eventPanel1;
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
    }
}
