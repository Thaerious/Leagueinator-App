using Leagueinator.Controls;
using Leagueinator.Controls.MatchCards;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using Leagueinator.Forms.MatchAssignments;
using Leagueinator.Utility;
using Leagueinator.Scoring.Plus;

namespace Leagueinator.Forms.Main {
    [TimeTrace]
    public partial class MainWindow : Window {
        private DataButton<RoundRow>? CurrentRoundButton;

        /// <summary>
        /// Fill matchRow cards with values from "roundRow".
        /// Clears all matchRow cards that does not have a value in "roundRow".
        /// </summary>
        /// <param name="roundRow"></param>
        private void PopulateMatchCards(RoundRow roundRow) {
            if (this.EventRow is null) throw new NullReferenceException();

            // Remove all match cards from panel.
            while (this.MachCardStackPanel.Children.Count > 0) {
                var child = this.MachCardStackPanel.Children[0];
                this.MachCardStackPanel.Children.Remove(child);
            }

            List<MatchRow> matchList = [.. roundRow.Matches];
            matchList.Sort((m1, m2) => m1.Lane.CompareTo(m2.Lane));

            foreach (MatchRow matchRow in matchList) {
                MatchCard matchCard = MatchCardFactory.GenerateMatchCard(matchRow);
                this.MachCardStackPanel.Children.Add(matchCard);
            }
        }

        /// <summary>
        /// Triggered when a match card changes type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void HndFormatChanged(object source, RoutedEventArgs args) {
            MatchCardFormatArgs formatArgs = (MatchCardFormatArgs)args;
            int index = this.MachCardStackPanel.Children.IndexOf(formatArgs.MatchCard);
            this.MachCardStackPanel.Children.Remove(formatArgs.MatchCard);
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
                this.MachCardStackPanel.Children.Insert(index, newMatchCard);
            }
        }

        /// <summary>
        /// Set a new select-round button.
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
        /// Triggered when the "Set RoundIndex" button is clicked.
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

        private void HndMemberTableDeletingRow(object sender, CustomRowAddEventArgs<MemberRow> e) {
            MemberRow memberRow = e.Row;
            TeamRow teamRow = memberRow.Team;
            MatchRow matchRow = teamRow.Match;
            RoundRow roundRow = matchRow.Round;

            if (!roundRow.Equals(this.CurrentRoundRow)) return;

            MatchCard matchCard = (MatchCard)this.MachCardStackPanel.Children[matchRow.Lane];
            TeamCard? teamCard = matchCard.GetTeamCard(teamRow.Index);
            if (teamCard is null) return;

            teamCard.RemoveName(memberRow.Player);
        }

        /// <summary>
        /// Indirectly triggered when a change is made to one of the underlying tables.
        /// </summary>
        /// <param name="isSaved">The new state isSaved</param>        
        private void HndStateChanged(object? sender, bool isSaved) {
            if (isSaved) this.Title = $"Leagueinator [{SaveState.Filename}]";
            else this.Title = $"Leagueinator [{SaveState.Filename}] *";
        }

        /// <summary>
        /// Triggered when a change is made to the underlying model.
        /// </summary>
        /// <param name="IsSaved">The new state isSaved</param>
        private void HndLeagueUpdate(object? sender, EventArgs? e) {
            SaveState.ChangeState(sender, false);
        }

        private void HndEventManagerClick(object sender, RoutedEventArgs e) {
            EventManager dialog = new EventManager(this.League);
            if (dialog.ShowDialog() == true) {
                if (dialog.Selected is null) return;

                League league = dialog.Selected.EventRow.League;
                EventRow eventRow = dialog.Selected.EventRow;
                if (eventRow.Rounds.Count == 0) {
                    RoundRow roundRow = eventRow.Rounds.Add();
                    roundRow.PopulateMatches();
                }

                this.League = dialog.Selected.EventRow.League;
                this.EventRow = dialog.Selected.EventRow;
            }
        }

        private void HndNewClick(object sender, RoutedEventArgs e) {
            this.Focus();
            this.League = NewLeague();
            SaveState.Filename = "";
            SaveState.ChangeState(this, false);
        }

        private void HndLoadClick(object sender, RoutedEventArgs e) {
            this.Focus();

            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "League Files (*.league)|*.league"
            };

            if (dialog.ShowDialog() == true) {
                League newLeague = new();
                newLeague.ReadXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;

                this.League = newLeague;
                this.EventRow = this.League.EventTable.GetLast();

                SaveState.ChangeState(sender, true);
            }
        }

        /// <summary>
        /// Menu > Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HndSaveClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            this.ClearFocus();

            if (SaveState.Filename.IsEmpty()) this.HndSaveAsClick(null, null);
            else this.League.WriteXml(SaveState.Filename);
            SaveState.ChangeState(sender, true);
        }

        private void HndSaveAsClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            this.ClearFocus();

            SaveFileDialog dialog = new();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == true) {
                this.League.WriteXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;
                SaveState.ChangeState(sender, true);
            }
        }
        private void HndCloseClick(object sender, RoutedEventArgs e) {
            // TODO check for unsaved changes
            // TODO remove current league
        }
        private void HndExitClick(object sender, RoutedEventArgs e) {
            this.Close(); // TODO check for unsaved changes
        }

        private void HndEventsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.EventTable);
        }
        private void HndRoundsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.RoundTable);
        }
        private void HndMatchesClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.MatchTable);
        }
        private void HndTeamsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.TeamTable);
        }
        private void HndMembersClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.MemberTable);
        }
        private void HndIdleClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.IdleTable);
        }
        
        [TimeTrace]
        private void HndViewResults(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) throw new NullReferenceException(nameof(this.EventRow));
            this.ClearFocus();
            PrinterForm form = new(new PointsPlusXMLBuilder(this.EventRow)) {
                Owner = this
            };
            form.Show();
        }

        private void HndMatchAssignments(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) throw new NullReferenceException(nameof(this.EventRow));
            this.ClearFocus();
            PrinterForm form = new(new MatchAssignmentsBuilder(this.CurrentRoundRow)) {
                Owner = this
            };
            form.Show();
        }

        private void HndMenuClick(object sender, RoutedEventArgs e) {
            this.Focus();
        }

        public void HndHelpAbout(object sender, RoutedEventArgs e) {
            MessageBox.Show(MainWindow.VERSION, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
