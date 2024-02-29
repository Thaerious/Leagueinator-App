using Model.Tables;
using System.ComponentModel;

namespace Leagueinator.Components {
    public partial class MatchCard : UserControl {

        public MatchRow MatchRow { get; private set; }

        public MatchCard() {
            InitializeComponent();
            this.membersGrid.RowValidating += this.MembersGrid_RowValidating;
            this.teamsGrid.RowValidating += this.TeamsGrid_RowValidating;
        }

        private void MembersGrid_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            Console.WriteLine($"Row Validating {this.Lane} {e.RowIndex}");
            var row = this.membersGrid.Rows[e.RowIndex];
            var index = row.Cells[MemberTable.COL.INDEX].Value;
            var player = row.Cells[MemberTable.COL.PLAYER].Value;

            if (index is DBNull || player is DBNull) return;

            // insert the UID into the row
            row.Cells[MemberTable.COL.MATCH].Value = this.MatchRow.UID;

            // make sure player is in player table
            PlayerTable playerTable = this.MatchRow.League.PlayerTable;
            if (!playerTable.Has(PlayerTable.COL.NAME, player)) {
                playerTable.AddRow((string)player);
            }

            // make sure {match, index} is in team table
            TeamTable teamTable = this.MatchRow.League.TeamTable;
            if (!teamTable.Has([TeamTable.COL.MATCH, TeamTable.COL.INDEX], [this.MatchRow.UID, (int)index])) {
                TeamRow teamRow = MatchRow.Teams.Add();
                row.Cells[MemberTable.COL.INDEX].Value = teamRow.Index;
            }
        }

        private void TeamsGrid_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            var row = this.teamsGrid.Rows[e.RowIndex];
            var index = row.Cells[TeamTable.COL.INDEX].Value;

            if (index is DBNull) return;

            // insert the UID into the row
            row.Cells[TeamTable.COL.MATCH].Value = this.MatchRow.UID;
        }

        [Category("Custom Properties")]
        [Description("Defines the lane index for this control.")]
        public int Lane { get; set; } = -1;

        public Controller? Controller {
            get => _controller;
            set {
                if (_controller == value) return;
                _controller = value;
                if (value == null) return;

                value.OnUpdateTeam += this.OnUpdateTeam;
                value.OnUpdateMatch += this.OnUpdateMatch;
            }
        }

        private void OnUpdateMatch(MatchRow matchRow) {
            if (matchRow.Lane != this.Lane) return;

            this.MatchRow = matchRow;            

            this.membersGrid.DataSource = matchRow.Members;
            this.teamsGrid.DataSource = matchRow.Teams;

            this.membersGrid.Columns[MemberTable.COL.MATCH].Visible = false;
            this.membersGrid.Columns[MemberTable.COL.INDEX].HeaderText = "team";

            this.teamsGrid.Columns[TeamTable.COL.MATCH].Visible = false;
            this.teamsGrid.Columns[TeamTable.COL.INDEX].HeaderText = "team";

            this.teamsGrid.Columns[TeamTable.COL.INDEX].DisplayIndex = 0;
            this.teamsGrid.Columns[TeamTable.COL.BOWLS].DisplayIndex = 1;
            this.teamsGrid.Columns[TeamTable.COL.TIE].DisplayIndex = 2;
        }

        private void OnUpdateTeam(TeamRow teamRow) {
            if (teamRow.Match.Lane != Lane) return;
            this.txtEnds.Text = teamRow.Match.Ends.ToString();
        }

        private Controller? _controller;
    }
}
