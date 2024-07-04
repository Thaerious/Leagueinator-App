using Leagueinator.Controls;
using Leagueinator.Extensions;
using Leagueinator.Formats;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
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
                this.League.LeagueUpdate -= this.HndLeagueUpdate;
                this.League.IdleTable.RowUpdating -= this.HndIdleUpdating;
                this.League.MemberTable.RowDeleting -= this.HndMemberTableDeletingRow;                
                this.League.MemberTable.RowUpdating -= this.HndMemberUpdating;               

                this._league = value;
                this.EventRow = this.League.EventTable.GetLast();
                
                switch (this.EventRow.EventFormat) {
                    case EventFormat.AssignedLadder:
                        this.TournamentFormat = new AssignedLadder();
                        break;
                }

                this.League.LeagueUpdate += this.HndLeagueUpdate;
                this.League.LeagueSettingsTable.RowChanging += this.HndSettingsChanging;
                this.League.IdleTable.RowUpdating += this.HndIdleUpdating;
                this.League.MemberTable.RowDeleting += this.HndMemberTableDeletingRow;
                this.League.MemberTable.RowUpdating += this.HndMemberUpdating;
            }
        }

        private void HndSettingsChanging(object sender, System.Data.DataRowChangeEventArgs e) {
            
        }

        private void HndMemberUpdating(object sender, CustomRowUpdateEventArgs args) {
            if (args.Change.OldValue is null) return;
            if (args.Change.NewValue is null) return;

            var playerTextBoxes = this.MachCardStackPanel.Descendants<PlayerTextBox>();
            foreach (PlayerTextBox playerTextBox in playerTextBoxes) {
                if (playerTextBox.Text == (string)args.Change.OldValue) {
                    playerTextBox.Text = (string)args.Change.NewValue;
                }
            }
        }

        private void HndIdleUpdating(object sender, CustomRowUpdateEventArgs args) {
            if (args.Change.OldValue is null) return;
            if (args.Change.NewValue is null) return;

            this.IdlePlayerContainer.Rename((string)args.Change.OldValue, (string)args.Change.NewValue);
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

                // Set a round button for each round.
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
