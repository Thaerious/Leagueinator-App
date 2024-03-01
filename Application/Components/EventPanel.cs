using Leagueinator.Utility;
using Model;
using Model.Tables;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Leagueinator.Components {
    public partial class EventPanel : UserControl {
        private EventRow? _eventRow;
        private RoundRow? currentRound;

        public EventPanel() {
            InitializeComponent();
            this.idleDataGrid.RowEnter += this.IdleDataGrid_RowEnter;
            this.idleDataGrid.RowValidating += this.DataGridIdle_RowValidating;
            this.idleDataGrid.DefaultValuesNeeded += this.IdleDataGrid_DefaultValuesNeeded;
        }

        private void IdleDataGrid_DefaultValuesNeeded(object? sender, DataGridViewRowEventArgs e) {
            Console.WriteLine($"IdleDataGrid_DefaultValuesNeeded");
            if (this.currentRound is null) return;
            e.Row.Cells[IdleTable.COL.ROUND].Value = this.currentRound.UID;
        }

        private void IdleDataGrid_RowEnter(object? sender, DataGridViewCellEventArgs e) {
            Console.WriteLine($"IdleDataGrid_RowEnters: {e.RowIndex}");
        }

        private void DataGridIdle_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            if (this.currentRound is null) return;
            DataGridViewRow row = this.idleDataGrid.Rows[e.RowIndex];

            Console.WriteLine($"DataGridIdle_RowValidating: {e.RowIndex} '{row.Cells[MemberTable.COL.PLAYER].Value}'");

            var player = row.Cells[MemberTable.COL.PLAYER].Value;
            if (player is DBNull || player.Equals("")) return;

            //// make sure player is in player table            
            PlayerTable playerTable = this.currentRound.League.PlayerTable;
            if (!playerTable.Has(PlayerTable.COL.NAME, player)) {
                playerTable.AddRow((string)player);
            }
        }

        [Category("Custom Properties")]
        public EventRow? EventRow {
            get => _eventRow;
            set {
                if (_eventRow == value) return;
                _eventRow = value;
                if (value == null) return;

                foreach (RoundRow roundRow in value.Rounds) {
                    this.OnAddRound(roundRow);
                }

                this.EventRow.League.RoundTable.RowChanged += this.RoundTable_RowChanged;
            }
        }

        private void RoundTable_RowChanged(object sender, System.Data.DataRowChangeEventArgs e) {
            if (this.EventRow is null) return;
            if ((int)e.Row[RoundTable.COL.EVENT] != this.EventRow.UID) return;
            this.OnAddRound(new RoundRow(e.Row));
        }

        private void OnAddRound(RoundRow roundRow) {
            RoundButton button = new RoundButton(roundRow);
            button.Text = $"Round {this.flowRounds.Controls.Count + 1}";
            this.flowRounds.Controls.Add(button);

            button.Height = 35;
            button.Width = 280;

            button.Click += this.RoundButtonClick;
        }

        private void RoundButtonClick(object? sender, EventArgs e) {
            if (sender is null) return;
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
