using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard : UserControl {
        private MatchRow? _matchRow = default;

        public MatchCard() {
            InitializeComponent();
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(HndUpdateText));
        }

        internal CardTarget? CardTarget { get; set; }

        public MatchRow MatchRow {
            get => _matchRow ?? throw new InvalidOperationException("MatchRow not initialized");
            set {
                _matchRow = value;
                if (MatchRow is null) {
                    this.Clear();
                    this.DataContext = null;
                    return;
                }

                if (MatchRow.Teams.Count != 2) throw new NullReferenceException("MatchRow in MatchCard must have exactly 2 teams");

                for (int i = 0; i < MatchRow.Teams[0]!.Members.Count; i++) {
                    MemberRow member = MatchRow.Teams[0]!.Members[i]!;
                    MemoryTextBox textBox = (MemoryTextBox)this.Team0.Children[i];
                    textBox.Text = member.Player;
                }

                for (int i = 0; i < MatchRow.Teams[1]!.Members.Count; i++) {
                    MemberRow member = MatchRow.Teams[1]!.Members[i]!;
                    MemoryTextBox textBox = (MemoryTextBox)this.Team1.Children[i];
                    textBox.Text = member.Player;
                }

                this.CheckTie0.IsChecked = MatchRow.Teams[0]!.Tie == 1;
                this.CheckTie1.IsChecked = MatchRow.Teams[1]!.Tie == 1;

                this.DataContext = MatchRow;
                this.TxtBowls0.DataContext = MatchRow.Teams[0];
                this.TxtBowls1.DataContext = MatchRow.Teams[1];
                this.TxtEnds.DataContext = MatchRow;
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
                TeamStackPanel parent = (TeamStackPanel)e.TextBox.Parent;

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
                    Debug.WriteLine($"Remove the old name ({e.Before}) from the members table");
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
            foreach (MemoryTextBox textBox in this.Team0.Children) textBox.Text = string.Empty;
            foreach (MemoryTextBox textBox in this.Team1.Children) textBox.Text = string.Empty;
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtEnds.Text = "0";
        }

        private void HndCheckTie(object sender, RoutedEventArgs e) {
            if (this.MatchRow is null) throw new NullReferenceException(nameof(this.MatchRow));
            if (this.MatchRow.Teams[0] is null) throw new NullReferenceException();
            if (this.MatchRow.Teams[1] is null) throw new NullReferenceException();
            if (sender is not CheckBox checkBox) return;

            if (checkBox.IsChecked == true) {
                if (checkBox == this.CheckTie0) {
                    this.CheckTie1.IsChecked = false;
                    this.MatchRow.Teams[0]!.Tie = 1;
                    this.MatchRow.Teams[1]!.Tie = -1;
                }
                else {
                    this.CheckTie0.IsChecked = false;
                    this.MatchRow.Teams[0]!.Tie = -1;
                    this.MatchRow.Teams[1]!.Tie = 1;
                }
            }
            else {
                this.MatchRow.Teams[0]!.Tie = 0;
                this.MatchRow.Teams[1]!.Tie = 0;
            }
        }
    }
}
