using Leagueinator.Utility;
using Model.Tables;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;

namespace Leagueinator.Components {
    public partial class EventPanel : UserControl {
        private EventRow? _eventRow;
        private RoundRow? currentRound;

        private Dictionary<string, object> previousCellValues = new Dictionary<string, object>();

        public EventPanel() {
            this.InitializeComponent();
            this.idleDataGrid.RowValidating += this.DataGridIdle_RowValidating;
            this.idleDataGrid.DataError += this.DataErrorHnd;
        }

        private void DataErrorHnd(object? sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show("Data input error: " + e.Exception.Message);
            e.ThrowException = false;
        }

        private void DataGridIdle_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            if (this.currentRound is null) return;

            DataGridViewRow gridRow = this.idleDataGrid.Rows[e.RowIndex];
            var player = gridRow.Cells[MemberTable.COL.PLAYER].Value;
            if (player is DBNull || player is null) return;

            //remove values from members table
            this.currentRound.Members
                .Where(memberRow => memberRow.Player == (string)player)
                .ToList()
                .ForEach(memberRow => memberRow.Delete());

            // Set the UID
            gridRow.Cells[IdleTable.COL.ROUND].Value = this.currentRound.UID;

            //make sure player is in player table
            PlayerTable playerTable = this.currentRound.League.PlayerTable;
            if (!playerTable.Has(PlayerTable.COL.NAME, player)) {
                playerTable.AddRow((string)player);
            }
        }

        public EventRow? EventRow {
            get => this._eventRow;
            set => this.SetEvent(value);
        }

        private void SetEvent(EventRow? eventRow) {
            ArgumentNullException.ThrowIfNull(eventRow);
            this._eventRow = eventRow;

            foreach (RoundRow roundRow in eventRow.Rounds) {
                this.AddRound(roundRow);
            }

            this._eventRow.League.RoundTable.RowChanged += this.HndRoundTableRow;
        }

        private void HndRoundTableRow(object sender, System.Data.DataRowChangeEventArgs e) {
            if ((int)e.Row[RoundTable.COL.EVENT] != this.EventRow!.UID) return;
            RoundRow roundRow = new RoundRow(e.Row);
            this.AddRound(roundRow);
        }

        private void AddRound(RoundRow roundRow) {
            RoundButton button = new RoundButton(roundRow) {
                Text = $"Round {this.flowRounds.Controls.Count + 1}",
                Height = 35,
                Width = 280,
            };

            this.flowRounds.Controls.Add(button);
            button.Click += this.RoundButtonClick;
            this.RoundButtonClick(button, null);
        }

        private void RoundButtonClick(object? sender, EventArgs? _) {
            RoundRow roundRow = (sender as RoundButton)!.Round;

            this.currentRound = roundRow;
            this.SetIdlePlayers(roundRow);

            foreach (MatchCard card in this.GetControls<MatchCard>()) {
                card.MatchRow = null;
            }

            foreach (MatchRow matchRow in roundRow.Matches) {
                var controls = this.GetControls<MatchCard>("Lane", matchRow.Lane).ToList();
                if (controls.IsNotEmpty()) controls[0].MatchRow = matchRow;
            }

            if (sender is RoundButton button) {
                this.GetControls<RoundButton>().ToList().ForEach(b => b.BackColor = Color.White);
                BackColor = Color.White;
                button.BackColor = Color.LightYellow;
            }
        }

        private void SetIdlePlayers(RoundRow roundRow) {
            this.idleDataGrid.ClearSelection();
            this.idleDataGrid.DataSource = null;
            this.idleDataGrid.DataSource = roundRow.IdlePlayers;
            this.idleDataGrid.Columns[IdleTable.COL.ROUND].Visible = false;
        }

        private void LayoutEventHander(object sender, LayoutEventArgs e) {
            foreach (Control child in this.flowLayoutPanel1.Controls) {
                child.Width = this.flowLayoutPanel1.ClientSize.Width
                            - this.flowLayoutPanel1.Padding.Horizontal
                            - child.Margin.Horizontal;
            }
        }

        private void AddRound_Click(object sender, EventArgs e) {
            if (this.EventRow is null) return;
            RoundRow row = this.EventRow.Rounds.Add();
            row.PopulateMatches();
        }
    }
}
