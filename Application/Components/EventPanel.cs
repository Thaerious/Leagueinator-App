using Model.Tables;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Leagueinator.Components {
    public partial class EventPanel : UserControl {
        private Controller? _controller;
        private RoundRow? currentRound;

        public EventPanel() {
            InitializeComponent();
            this.idleDataGrid.RowValidating += this.DataGridIdle_RowValidating;
        }

        private void DataGridIdle_RowValidating(object? sender, DataGridViewCellCancelEventArgs e) {
            if (this.currentRound is null) return;

            var row = this.idleDataGrid.Rows[e.RowIndex];
            var player = row.Cells[MemberTable.COL.PLAYER].Value;
            if (player is DBNull) return;

            // insert the Round UID into the row
            row.Cells[IdleTable.COL.ROUND].Value = this.currentRound.UID;

            // make sure player is in player table            
            PlayerTable playerTable = this.currentRound.League.PlayerTable;
            if (!playerTable.Has(PlayerTable.COL.NAME, player)) {
                playerTable.AddRow((string)player);
            }
        }

        [Category("Custom Properties")]
        [Description("Model Controller.")]
        public Controller? Controller {
            get => _controller;
            set {
                if (_controller == value) return;
                _controller = value;
                if (value == null) return;

                value.OnAddRound += this.OnAddRound;
            }
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
            Console.WriteLine(roundRow.Matches.PrettyPrint());

            this.currentRound = roundRow;
            this.SetIdlePlayers(roundRow);

            foreach (MatchCard card in this.GetControls<MatchCard>()) {
                card.MatchRow = null;
            }

            foreach (MatchRow matchRow in roundRow.Matches) {
                this.GetControls<MatchCard>("Lane", matchRow.Lane)
                .First()
                .MatchRow = matchRow;
            }
        }

        private void SetIdlePlayers(RoundRow roundRow) {
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
    }
}
