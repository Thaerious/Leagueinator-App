using Model.Tables;
using System.Data;

namespace Leagueinator.Components {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
            this.eventPanel1.Controller = this.modelController;
        }

        private void HndMenuViewTeams(object sender, EventArgs e) {
            new FormViewTable().Show("Teams", this.modelController.League.TeamTable);
        }

        private void HndMenuViewPlayers(object sender, EventArgs e) {
            new FormViewTable().Show("Players", this.modelController.League.PlayerTable);
        }

        private void HndMenuViewMembers(object sender, EventArgs e) {
            new FormViewTable().Show("Members", this.modelController.League.MemberTable);
        }

        private void matchTableToolStripMenuItem_Click(object sender, EventArgs e) {
            DataView dataSource = (DataView)this.eventPanel1.matchCard1.membersGrid.DataSource;
            DataTable sourceTable = dataSource.Table;
            MemberTable membersTable = modelController.League.MemberTable;
        }

        private void HndMenuViewEvents(object sender, EventArgs e) {
            new FormViewTable().Show("Events", this.modelController.League.EventTable);
        }

        private void HndMenuViewRounds(object sender, EventArgs e) {
            new FormViewTable().Show("Rounds", this.modelController.League.RoundTable);
        }

        private void HndMenuViewMatches(object sender, EventArgs e) {
            new FormViewTable().Show("Matches", this.modelController.League.MatchTable);
        }

        private void HndMenuViewIdle(object sender, EventArgs e) {
            new FormViewTable().Show("Idle Players", this.modelController.League.IdleTable);
        }

        private void HndMenuViewSettings(object sender, EventArgs e) {
            new FormViewTable().Show("Settings", this.modelController.League.SettingsTable);
        }
    }
}
