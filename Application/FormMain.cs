using Model;

namespace Leagueinator.Components {
    public partial class FormMain : Form {
        private League Model;

        public FormMain() {
            this.InitializeComponent();
            this.Model = new MockModel();
            this.eventPanel.EventRow = this.Model.EventTable.GetRow(0);
        }

        private void HndMenuViewTeams(object sender, EventArgs e) {
            new FormViewTable().Show("Teams", this.Model.TeamTable);
        }

        private void HndMenuViewPlayers(object sender, EventArgs e) {
            new FormViewTable().Show("Players", this.Model.PlayerTable);
        }

        private void HndMenuViewMembers(object sender, EventArgs e) {
            new FormViewTable().Show("Members", this.Model.MemberTable);
        }

        private void HndMenuViewEvents(object sender, EventArgs e) {
            new FormViewTable().Show("Events", this.Model.EventTable);
        }

        private void HndMenuViewRounds(object sender, EventArgs e) {
            new FormViewTable().Show("Rounds", this.Model.RoundTable);
        }

        private void HndMenuViewMatches(object sender, EventArgs e) {
            new FormViewTable().Show("Matches", this.Model.MatchTable);
        }

        private void HndMenuViewIdle(object sender, EventArgs e) {
            new FormViewTable().Show("Idle Players", this.Model.IdleTable);
        }

        private void HndMenuViewSettings(object sender, EventArgs e) {
            new FormViewTable().Show("Settings", this.Model.SettingsTable);
        }
    }
}
