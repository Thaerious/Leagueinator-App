using Model.Tables;
using System.ComponentModel;

namespace Leagueinator.Components {
    public partial class MatchCard : UserControl {
        [Category("Custom Properties")]
        [Description("Defines the lane index for this control.")]
        public int Lane { get; set; } = -1;

        public MatchRow? MatchRow { 
            get => this._matchRow;

            set {
                this._matchRow = value;

                if (value == null) {
                    this.txtEnds.Text = "";
                    this.membersGrid.DataSource = null;
                    this.teamsGrid.DataSource = null;
                    return;
                }

                if (value.Lane != this.Lane) return;

                this.txtEnds.Text = value.Ends.ToString();

                this.membersGrid.DataSource = value.Members;
                this.teamsGrid.DataSource = value.Teams;

                this.membersGrid.Columns[MemberTable.COL.MATCH].Visible = false;
                this.membersGrid.Columns[MemberTable.COL.INDEX].HeaderText = "team";

                this.teamsGrid.Columns[TeamTable.COL.MATCH].Visible = false;
                this.teamsGrid.Columns[TeamTable.COL.INDEX].HeaderText = "team";

                this.teamsGrid.Columns[TeamTable.COL.INDEX].DisplayIndex = 0;
                this.teamsGrid.Columns[TeamTable.COL.BOWLS].DisplayIndex = 1;
                this.teamsGrid.Columns[TeamTable.COL.TIE].DisplayIndex = 2;
            }
        }

        public MatchCard() {
            InitializeComponent();
            this.membersGrid.RowValidating += this.MembersGrid_RowValidating;
            this.teamsGrid.RowValidating += this.TeamsGrid_RowValidating;
        }

        private void MembersGrid_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            Console.WriteLine("MembersGrid_RowValidating");
            if (this.MatchRow is null) return;

            DataGridViewRow gridRow = this.membersGrid.Rows[e.RowIndex];
            var index = gridRow.Cells[MemberTable.COL.INDEX].Value;
            var player = gridRow.Cells[MemberTable.COL.PLAYER].Value;

            if (player is DBNull) return;

            // default team value
            if (index is DBNull) {
                index = this.MatchRow.League.TeamTable.LastIndex(MatchRow);
                if ((int)index == 0) index = 1;
                gridRow.Cells[MemberTable.COL.INDEX].Value = index;
                //Console.WriteLine($"Before {index} {index.GetType()} {(int)index == 0}");
                //if ((int)index == 0) index = 1;
                //Console.WriteLine($"After {index} {index.GetType()} {(int)index == 0}");
            }

            // insert the UID into the gridRow
            gridRow.Cells[MemberTable.COL.MATCH].Value = this.MatchRow.UID;

            // make sure player is in player table
            PlayerTable playerTable = this.MatchRow.League.PlayerTable;
            if (!playerTable.Has(PlayerTable.COL.NAME, player)) {
                playerTable.AddRow((string)player);
            }

            // make sure {match, index} is in team table
            TeamTable teamTable = this.MatchRow.League.TeamTable;
            if (!teamTable.Has([TeamTable.COL.MATCH, TeamTable.COL.INDEX], [this.MatchRow.UID, (int)index])) {
                TeamRow teamRow = MatchRow.Teams.Add((int)index);                
            }
        }

        private void TeamsGrid_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            if (this.MatchRow is null) return;

            var row = this.teamsGrid.Rows[e.RowIndex];
            var index = row.Cells[TeamTable.COL.INDEX].Value;

            if (index is DBNull) return;

            // insert the UID into the gridRow
            row.Cells[TeamTable.COL.MATCH].Value = this.MatchRow.UID;
        }

        private MatchRow? _matchRow;
    }
}
