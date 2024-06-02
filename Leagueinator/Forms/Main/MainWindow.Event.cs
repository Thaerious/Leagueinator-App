using Leagueinator.Controls;
using Leagueinator.Controls.MatchCards;
using Leagueinator.Model.Tables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Leagueinator.Forms.Main {
    public partial class MainWindow : Window {
        private DataButton<RoundRow>? CurrentRoundButton;

        /// <summary>
        /// Fill matchRow cards with values from "roundRow".
        /// Clears all matchRow cards that does not have a value in "roundRow".
        /// </summary>
        /// <param name="roundRow"></param>
        private void PopulateMatchCards(RoundRow roundRow) {
            if (this.EventRow is null) throw new NullReferenceException();
            this.CardStackPanel.Children.Clear();

            foreach (MatchRow matchRow in roundRow.Matches) {
                this.CardStackPanel.Children.Add(MatchCardFactory.GenerateMatchCard(matchRow));
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
        /// </summary>
        /// <param name="__"></param>
        /// <param name="_"></param>
        private void HndClickAddRound(object? __, RoutedEventArgs? _) {
            if (this.EventRow is null) return;
            this.ClearFocus();
            if (this.EventRow is null) return;
            RoundRow nextRound = this.TournamentFormat.GenerateNextRound(this.EventRow);
            this.AddRoundButton(nextRound);
            this.InvokeLastRoundButton();
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
            this.InvokeLastRoundButton();

            // Rename buttons
            int i = 1;
            foreach (DataButton<RoundRow> button in this.RoundButtonContainer.Children) {
                button.Content = $"Round {i++}";
            }
        }

        private void InvokeLastRoundButton(DataButton<RoundRow>? button = null) {
            button ??= (DataButton<RoundRow>)this.RoundButtonContainer.Children[^1];
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void HndGenEmptyRound(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) return;
            this.ClearFocus();
            RoundRow roundRow = this.EventRow.Rounds.Add();
            roundRow.PopulateMatches();
            this.AddRoundButton(roundRow);
            this.InvokeLastRoundButton();
        }

        public void HndGenNextRound(object sender, RoutedEventArgs args) {
            if (this.EventRow is null) return;
            this.ClearFocus();
            RoundRow nextRound = this.TournamentFormat.GenerateNextRound(this.EventRow);
            this.AddRoundButton(nextRound);
            this.InvokeLastRoundButton();
        }

        public void HndCopyRnd(object sender, RoutedEventArgs args) {
            if (this.EventRow is null) return;
            if (this.CurrentRoundRow is null) return;
            this.ClearFocus();
            RoundRow nextRound = this.EventRow.Rounds.Add();
            nextRound.PopulateMatches();

            for (int m = 0; m < this.CurrentRoundRow.Matches.Count; m++) {
                MatchRow matchRow = this.CurrentRoundRow.Matches[m]!;
                for (int t = 0; t < matchRow.Teams.Count; t++) {
                    TeamRow teamRow = matchRow.Teams[t]!;
                    foreach (MemberRow member in teamRow.Members) {
                        nextRound.Matches[m]!.Teams[t]!.Members.Add(member.Player);
                    }
                }
            }

            this.AddRoundButton(nextRound);
            this.InvokeLastRoundButton();
        }
    }
}
