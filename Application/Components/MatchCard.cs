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
            var row = this.membersGrid.Rows[e.RowIndex];
            var index = row.Cells[MembersTable.COL.INDEX].Value;
            var player = row.Cells[MembersTable.COL.PLAYER].Value;

            if (index is DBNull || player is DBNull) return;

            row.Cells[MembersTable.COL.MATCH].Value = this.MatchRow.UID;
        }

        private void TeamsGrid_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            var row = this.teamsGrid.Rows[e.RowIndex];
            var index = row.Cells[TeamTable.COL.INDEX].Value;

            if (index is DBNull) return;

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
                value.OnUpdateMatch += this.Value_OnUpdateMatch;
            }
        }

        private void Value_OnUpdateMatch(MatchRow matchRow) {
            if (matchRow.Lane != Lane) return;

            this.MatchRow = matchRow;            

            this.membersGrid.DataSource = matchRow.Members;
            this.teamsGrid.DataSource = matchRow.Teams;

            this.membersGrid.Columns[MembersTable.COL.MATCH].Visible = false;
            this.membersGrid.Columns[MembersTable.COL.INDEX].HeaderText = "team";

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
