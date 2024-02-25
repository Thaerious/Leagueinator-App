using Model.Tables;
using System.Data;

namespace Leagueinator.Components {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();           
        }

        private void HndMenuViewTeams(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.TeamTable);
        }

        private void HndMenuViewPlayers(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.PlayersTable);
        }

        private void HndMenuViewMembers(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.MembersTable);
        }

        private void matchTableToolStripMenuItem_Click(object sender, EventArgs e) {
            DataView dataSource = (DataView)this.eventPanel1.matchCard1.membersGrid.DataSource;
            DataTable sourceTable = dataSource.Table;
            MembersTable membersTable = modelController.League.MembersTable;

            Console.WriteLine($"Members Grid Data Source = {dataSource}:{dataSource.GetHashCode()}");
            Console.WriteLine($"Data Source Parent Table = {sourceTable}:{sourceTable.GetHashCode()}");
            Console.WriteLine($"League Members Table = {membersTable}:{membersTable.GetHashCode()}");

            Console.WriteLine($"Source Data Set = {sourceTable.DataSet}:{sourceTable.DataSet.GetHashCode()}");
            Console.WriteLine($"League = {modelController.League}:{modelController.League.GetHashCode()}");
        }

        private void HndMenuViewEvents(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.EventTable);
        }

        private void HndMenuViewRounds(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.RoundsTable);
        }

        private void HndMenuViewMatches(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.MatchTable);
        }

        private void HndMenuViewIdle(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.IdleTable);
        }

        private void HndMenuViewSettings(object sender, EventArgs e) {
            new FormViewTable().Show(this.modelController.League.SettingsTable);
        }
    }
}
