using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Windows;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard4321 : MatchCard {
        public MatchCard4321() {
            InitializeComponent();
        }

        public override MatchRow MatchRow {
            get => _matchRow ?? throw new InvalidOperationException("MatchRow not initialized");
            set {

            }
        }

        /// <summary>
        /// Triggered when the value of a players name (MemoryTextBox) is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NullReferenceException"></exception>
        private void HndUpdateText(object sender, MemoryTextBoxArgs e) {
            if (this.MatchRow is null) throw new NullReferenceException(nameof(MatchRow));

            if (this.MatchRow.Round.AllPlayers.Contains(e.After)) {
                // if the player already exists reject.
                MessageBox.Show($"Player {e.After} previously assigned.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.TextBox.Text = e.Before;
            }
            else {
                TeamCard parent = e.TextBox.Ancestors<TeamCard>()[0];

                // Remove new name from the idle table
                if (this.MatchRow.Round.IdlePlayers.Has(e.After)) {
                    this.MatchRow.Round.IdlePlayers.Get(e.After)!.Remove();
                }

                // Add new name to the teams table
                if (!e.After.IsEmpty()) {
                    this.MatchRow.League.PlayerTable.AddRowIf(e.After);
                    this.MatchRow.Teams[parent.TeamIndex]!.Members.Add(e.After);
                }

                // Remove the old name from the members table
                if (!e.Before.IsEmpty()) {
                    TeamRow teamRow = this.MatchRow.Teams[parent.TeamIndex] ?? throw new NullReferenceException();
                    MemberRow memberRow = teamRow.Members.Get(e.Before) ?? throw new NullReferenceException();
                    memberRow.Remove();
                }
            }

            if (e.Cause == Cause.EnterPressed) {
                TeamStackPanel parent = (TeamStackPanel)e.TextBox.Parent;
                int index = parent.Children.IndexOf(e.TextBox);
                if (index + 1 < parent.Children.Count) {
                    parent.Children[index + 1].Focus();
                }
            }
        }

        public void Clear() {
            this.Team0.Clear();
            this.Team1.Clear();
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtEnds.Text = "0";
        }
    }
}
