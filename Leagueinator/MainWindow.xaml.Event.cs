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
                if (this.EventRow != null) {
                    this.EventRow.League.RoundTable.RowChanged -= this.HndRoundTableRow;
                }

                this._eventRow = value;
                this.RoundButtonContainer.Children.Clear();

                if (this.EventRow is null) return;
                if (this.EventRow.Rounds.Count == 0) throw new NotSupportedException("Must set event with a minimum of one round");

                this.CardStackPanel.Size = int.Parse(this.EventRow!.Settings["lanes"]);

                // Add a round button for each round.
                foreach (RoundRow roundRow in this.EventRow.Rounds) {
                    this.AddRoundButton(roundRow);
                }

                this.PopulateMatchCards(this.EventRow.Rounds[^1]!);

                // Click the last round button (sets current round).
                var lastButton = this.RoundButtonContainer.Children[^1];
                this.EventRow.League.RoundTable.RowChanged += this.HndRoundTableRow;
                lastButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        /// <summary>
        /// Fill match cards with values from "roundRow".
        /// Clears all match cards that does not have a value in "roundRow".
        /// </summary>
        /// <param name="roundRow"></param>
        private void PopulateMatchCards(RoundRow roundRow) {
            if (this.EventRow is null) throw new NullReferenceException();

            foreach (MatchCard matchCard in this.CardStackPanel) {
                matchCard.Clear();
            }

            foreach (MatchRow matchRow in roundRow.Matches) {
                this.CardStackPanel[matchRow.Lane].MatchRow = matchRow;
            }
        }

        /// <summary>
        /// Add a new select-round button.
        /// </summary>
        /// <param name="roundRow">The associated roundRow</param>
        /// <returns></returns>
        public DataButton<RoundRow> AddRoundButton(RoundRow roundRow) {
            DataButton<RoundRow> button = new() {
                Data = roundRow,
                Content = $"Round {this.RoundButtonContainer.Children.Count + 1}",
                Margin = new Thickness(3),
            };

            this.RoundButtonContainer.Children.Add(button);
            button.Click += HndClickSelectRound;
            return button;
        }

        /// <summary>
        /// Triggered when a round button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_"></param>
        /// <exception cref="NotSupportedException"></exception>
        private void HndClickSelectRound(object? sender, EventArgs? _) {
            if (sender is not DataButton<RoundRow> button) throw new NotSupportedException();
            RoundRow? roundRow = button.Data ?? throw new NotSupportedException();
            this.ClearFocus();

            if (this.CurrentRoundButton is not null) {
                this.CurrentRoundButton.Background = Brushes.LightGray;
            }

            this.CurrentRoundRow = roundRow;
            this.CurrentRoundButton = button;
            button.Background = Brushes.LightCyan;

            this.PopulateMatchCards(roundRow);
        }

        /// <summary>
        /// Triggered when the "Add Round" button is clicked.
        /// See HndRoundTableRow
        /// </summary>
        /// <param name="__"></param>
        /// <param name="_"></param>
        private void HndClickAddRound(object? __, RoutedEventArgs? _) {
            if (this.EventRow is null) return;
            this.ClearFocus();

            RoundRow roundRow = this.EventRow.Rounds.Add();
            int lanes = int.Parse(this.EventRow.Settings["lanes"]);
            int teams = int.Parse(this.EventRow.Settings["teams"]);
            roundRow.PopulateMatches(lanes, teams);
        }

        /// <summary>
        /// Triggered when the delete round button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HndClickDeleteRound(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) return;
            if (this.CurrentRoundRow is null) return;
            this.ClearFocus();

            // Remove the UI button and the model round.
            this.RoundButtonContainer.Children.Remove(this.CurrentRoundButton);
            this.CurrentRoundRow.Remove();

            // Make sure there is at least one button and select the last one.
            if (this.EventRow.Rounds.Count == 0) this.HndClickAddRound(null, null);
            this.InvokeRoundButton();

            // Rename buttons
            int i = 1;
            foreach (DataButton<RoundRow> button in this.RoundButtonContainer.Children) {
                button.Content = $"Round {i++}";
            }
        }

        /// <summary>
        /// When a row is added to the table, add a button.
        /// This button is added to the end of the button container.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HndRoundTableRow(object sender, System.Data.DataRowChangeEventArgs e) {
            if ((int)e.Row[RoundTable.COL.EVENT] != this.EventRow!.UID) return;
            if (e.Action != System.Data.DataRowAction.Add) return;
            this.AddRoundButton(new(e.Row));
            this.InvokeRoundButton();
        }

        private void InvokeRoundButton(DataButton<RoundRow>? button = null) {
            button ??= (DataButton<RoundRow>)this.RoundButtonContainer.Children[^1];
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
