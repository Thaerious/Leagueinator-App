using Leagueinator.Formats;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Model.Source.Tables.Event;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Leagueinator.Forms.Main {
    /// <summary>
    /// Contains the publicly accessible model state.
    /// Includes the model it's self as well as any currently selected components.
    /// Pluggable components (such as tournament type) can be accessed from here.
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// The current league.
        /// All other state variables point to a model component in this.
        /// </summary>
        internal League League {
            get => this._league;
            set {
                this.League.MemberTable.RowDeleting -= this.HndMemberTableDeletingRow;

                this.League.LeagueUpdate -= this.HndLeagueUpdate;
                this._league = value;
                this.EventRow = this.League.EventTable.GetLast();
                value.LeagueUpdate += this.HndLeagueUpdate;

                switch (this.EventRow.EventFormat) {
                    case EventFormat.AssignedLadder:
                        this.TournamentFormat = new AssignedLadder();
                        break;
                }

                this.League.MemberTable.RowDeleting += this.HndMemberTableDeletingRow;
            }
        }

        /// <summary>
        /// The currently selected EventRow.
        /// Will throw an exception if attempting to retrieve before it's set.
        /// </summary>
        internal EventRow? EventRow {
            get {
                if (this._eventRow is null) throw new NullReferenceException();
                return this._eventRow;
            }
            set {
                this._eventRow = value;
                this.RoundButtonContainer.Children.Clear();

                if (this.EventRow is null) return;
                if (this.EventRow.Rounds.Count == 0) throw new NotSupportedException("Set event at minimum must have one round.");

                // Add a round button for each round.
                foreach (RoundRow roundRow in this.EventRow.Rounds) {
                    this.AddRoundButton(roundRow);
                }

                // Click the last round button (sets current round).
                var lastButton = this.RoundButtonContainer.Children[^1];
                lastButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        /// <summary>
        /// The currently selected RoundRow.
        /// Will throw an exception if attempting to retrieve before it's set.
        /// </summary>
        internal RoundRow CurrentRoundRow {
            get {
                if (this._currentRoundRow is null) throw new InvalidOperationException("CurrentRoundRow not initialized");
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

        internal TournamentFormat TournamentFormat { get; private set; } = new AssignedLadder();

        private League _league = NewLeague();
        private EventRow? _eventRow;
        private RoundRow? _currentRoundRow;
    }
}
