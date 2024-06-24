using Leagueinator.Controls;
using Leagueinator.Controls.MatchCards;
using Leagueinator.Model.Tables;
using System.Data;
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

            while (this.CardStackPanel.Children.Count > 0) {
                var child = this.CardStackPanel.Children[0];
                MatchCard matchCard = (MatchCard)child;
                this.CardStackPanel.Children.Remove(matchCard);
            }

            List<MatchRow> matchList = [.. roundRow.Matches];
            matchList.Sort((m1, m2) => m1.Lane.CompareTo(m2.Lane));

            foreach (MatchRow matchRow in matchList) {
                MatchCard matchCard = MatchCardFactory.GenerateMatchCard(matchRow);
                this.CardStackPanel.Children.Add(matchCard);
            }
        }

        /// <summary>
        /// Triggered when a match card changes type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void HndFormatChanged(object source, RoutedEventArgs args) {
            MatchCardFormatArgs formatArgs = (MatchCardFormatArgs)args;
            int index = this.CardStackPanel.Children.IndexOf(formatArgs.MatchCard);
            this.CardStackPanel.Children.Remove(formatArgs.MatchCard);
            formatArgs.MatchCard.MatchRow.MatchFormat = formatArgs.MatchFormat;

            MatchCard? newMatchCard = null;
            switch (formatArgs.MatchFormat) {
                case MatchFormat.VS1:
                    break;
                case MatchFormat.VS2:
                    break;
                case MatchFormat.VS3:
                    break;
                case MatchFormat.VS4:
                    newMatchCard = new MatchCardV4() {
                        MatchRow = formatArgs.MatchCard.MatchRow
                    };
                    break;
                case MatchFormat.A4321:
                    newMatchCard = new MatchCard4321() {
                        MatchRow = formatArgs.MatchCard.MatchRow
                    };
                    break;
            }

            if (newMatchCard is not null) {
                this.CardStackPanel.Children.Insert(index, newMatchCard);
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
            button.Click += this.HndClickSelectRound;
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
        /// Triggered when the "Add RoundIndex" button is clicked.
        /// </summary>
        /// <param name="__"></param>
        /// <param name="_"></param>
        private void HndClickAddRound(object? __, RoutedEventArgs? _) {
            if (this.EventRow is null) return;
            this.ClearFocus();
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

            RoundRow newRoundRow = this.EventRow.Rounds.Add();

            foreach (MatchRow matchRow in this.CurrentRoundRow.Matches) {
                MatchRow newMatchRow = newRoundRow.Matches.Add(matchRow.Lane, matchRow.Ends);
                foreach (TeamRow teamRow in matchRow.Teams) {
                    TeamRow newTeamRow = newMatchRow.Teams.Add(teamRow.Index);
                    foreach (MemberRow memberRow in teamRow.Members) {
                        newTeamRow.Members.Add(memberRow.Player);
                    }
                }
            }

            foreach (IdleRow idleRow in this.CurrentRoundRow.IdlePlayers) {
                newRoundRow.IdlePlayers.Add(idleRow.Player);
            }

            this.AddRoundButton(newRoundRow);
            this.InvokeLastRoundButton();
        }

        private void HndMemberTableDeletingRow(object sender, DataRowChangeEventArgs e) {
            MemberRow memberRow = new(e.Row);
            TeamRow teamRow = memberRow.Team;
            MatchRow matchRow = teamRow.Match;
            RoundRow roundRow = matchRow.Round;

            if (!roundRow.Equals(this.CurrentRoundRow)) return;

            MatchCard matchCard = (MatchCard)this.CardStackPanel.Children[matchRow.Lane];
            TeamCard? teamCard = matchCard.GetTeamCard(teamRow.Index);
            if (teamCard is null) return;

            teamCard.RemoveName(memberRow.Player);
        }
    }
}
