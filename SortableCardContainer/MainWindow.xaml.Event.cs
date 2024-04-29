using Leagueinator.Controls;
using Leagueinator.Model.Tables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SortableCardContainer {
    public partial class MainWindow : Window {
        private EventRow? _eventRow;
        private RoundRow? _currentRoundRow;
        private DataButton<RoundRow>? CurrentRoundButton;

        public RoundRow? CurrentRoundRow {
            get {
                return this._currentRoundRow;
            }
            set {
                // Save the round row value, if it's not null make it the current round.
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
                if (this.EventRow.Rounds.Count == 0) throw new NotSupportedException("Must set event with a minimum of one round");

                // Add a round button for each round.
                foreach (RoundRow roundRow in this.EventRow.Rounds) {
                    this.AddRoundButton(roundRow);
                }

                this.PopulateMatchCards(this.EventRow.Rounds[^1]!);

                // Click the last row button (sets current round row).
                var lastButton = this.RoundButtonContainer.Children[^1];
                this.EventRow.League.RoundTable.RowChanged += this.HndRoundTableRow;
                lastButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void PopulateMatchCards(RoundRow roundRow) {
            this.CardStackPanel.Children.Clear();
            foreach (MatchRow matchRow in roundRow.Matches) {
                this.CardStackPanel.Add(new MatchCard() {
                    MatchRow = matchRow
                });
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
            var button = this.AddRoundButton(new(e.Row));
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void RoundButtonClick(object? sender, EventArgs? _) {
            if (sender is not DataButton<RoundRow> button) throw new NotSupportedException();
            RoundRow? roundRow = button.Data;
            if (roundRow is null) throw new NotSupportedException();

            if (this.CurrentRoundButton is not null) {
                this.CurrentRoundButton.Background = Brushes.LightGray;
            }

            this.CurrentRoundRow = roundRow;
            this.CurrentRoundButton = button;

            button.Background = Brushes.LightCyan;

            foreach (MatchCard matchCard in this.CardStackPanel) matchCard.MatchRow = null;
            for (int i = 0; i < this.CurrentRoundRow.Matches.Count; i++) {
                this.CardStackPanel.GetMatchCard(i).MatchRow = this.CurrentRoundRow.Matches[i];
            }
        }

        private void HndAddRound(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) return;
            RoundRow roundRow = this.EventRow.Rounds.Add();
            int lanes = int.Parse(this.EventRow.Settings["lanes"]);
            int teams = int.Parse(this.EventRow.Settings["teams"]);
            roundRow.PopulateMatches(lanes, teams);
        }

        private void HndDeleteRound(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) return;
            throw new NotImplementedException();
        }
    }
}
