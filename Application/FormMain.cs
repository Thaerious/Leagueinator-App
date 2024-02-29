using Model;
using Model.Tables;
using System.Data;

namespace Leagueinator.Components {
    public partial class FormMain : Form {

        private League model;

        public FormMain() {
            InitializeComponent();
            this.model = new MockModel();
            this.eventPanel1.EventRow = model.EventTable.GetRow(0);
        }

        private void HndMenuViewTeams(object sender, EventArgs e) {
            new FormViewTable().Show("Teams", this.model.TeamTable);
        }

        private void HndMenuViewPlayers(object sender, EventArgs e) {
            new FormViewTable().Show("Players", this.model.PlayerTable);
        }

        private void HndMenuViewMembers(object sender, EventArgs e) {
            new FormViewTable().Show("Members", this.model.MemberTable);
        }

        private void HndMenuViewEvents(object sender, EventArgs e) {
            new FormViewTable().Show("Events", this.model.EventTable);
        }

        private void HndMenuViewRounds(object sender, EventArgs e) {
            new FormViewTable().Show("Rounds", this.model.RoundTable);
        }

        private void HndMenuViewMatches(object sender, EventArgs e) {
            new FormViewTable().Show("Matches", this.model.MatchTable);
        }

        private void HndMenuViewIdle(object sender, EventArgs e) {
            new FormViewTable().Show("Idle Players", this.model.IdleTable);
        }

        private void HndMenuViewSettings(object sender, EventArgs e) {
            new FormViewTable().Show("Settings", this.model.SettingsTable);
        }
    }
}
