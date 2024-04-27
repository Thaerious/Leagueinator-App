using Leagueinator.Controls;
using Leagueinator.Model.Tables;
using System.Diagnostics;
using System.Windows;

namespace SortableCardContainer {
    public partial class MainWindow : Window {
        private EventRow? _eventRow;
        private RoundRow? _currentRoundRow;

        public RoundRow? CurrentRoundRow {
            get {
                return this._currentRoundRow;
            }
            set {
                this._currentRoundRow = value;
                if (this.CurrentRoundRow != null) {
                    this.IdlePlayerContainer.RoundRow = this.CurrentRoundRow;
                }
            }
        }

        public EventRow? EventRow {
            get => this._eventRow;
            set {
                this._eventRow = value;
                this.RoundButtonContainer.Children.Clear();
                if (this.EventRow is null) return;

                foreach (RoundRow roundRow in this.EventRow.Rounds) {
                    this.AddRoundButton(roundRow);
                }

                this.EventRow.League.RoundTable.RowChanged += this.HndRoundTableRow;                
                Debug.WriteLine(this.EventRow?.Rounds?.Count);
                if (this.EventRow!.Rounds.Count > 0) {
                    this.CurrentRoundRow = this.EventRow.Rounds.Last();
                }
            }
        }

        public DataButton<RoundRow> AddRoundButton(RoundRow roundRow) {
            DataButton<RoundRow> button = new() {
                Data = roundRow,
                Content = $"Round {this.RoundButtonContainer.Children.Count + 1}",
                Margin = new Thickness(3)
            };

            this.RoundButtonContainer.Children.Add(button);
            button.Click += RoundButtonClick;
            return button;
        }

        private void HndRoundTableRow(object sender, System.Data.DataRowChangeEventArgs e) {
            if ((int)e.Row[RoundTable.COL.EVENT] != this.EventRow!.UID) return;
            RoundRow roundRow = new RoundRow(e.Row);
            this.AddRoundButton(roundRow);
        }

        private void RoundButtonClick(object? sender, EventArgs? _) {
            RoundRow? roundRow = (sender as DataButton<RoundRow>)!.Data;
            if (roundRow is null) return;
            this.CurrentRoundRow = roundRow;

            //this.CurrentRoundRow = roundRow;
            //this.SetIdlePlayers(roundRow);

            //foreach (MatchCard card in this.GetControls<MatchCard>()) {
            //    card.MatchRow = null;
            //}

            //foreach (MatchRow matchRow in roundRow.Matches) {
            //    var controls = this.GetControls<MatchCard>("Lane", matchRow.Lane).ToList();
            //    if (controls.IsNotEmpty()) controls[0].MatchRow = matchRow;
            //}

            //if (sender is RoundButton button) {
            //    this.GetControls<RoundButton>().ToList().ForEach(b => b.BackColor = Color.White);
            //    this.BackColor = Color.White;
            //    button.BackColor = Color.LightYellow;
            //}
        }
    }
}
