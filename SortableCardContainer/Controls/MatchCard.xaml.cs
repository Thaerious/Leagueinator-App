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
            init {
                _matchRow = value;
                if (MatchRow is null) {
                    this.Clear();
                    this.DataContext = null;
                    return;
                }
                if (MatchRow.Teams[0] is not null) {
                    int i = 0;
                    foreach (MemberRow member in MatchRow.Teams[0].Members) {
                        MemoryTextBox textBox = (MemoryTextBox)this.Team0.Children[i++];
                        textBox.Text = member.Player;
                    }
                }
                if (MatchRow.Teams[1] is not null) {
                    int i = 0;
                    foreach (MemberRow member in MatchRow.Teams[1].Members) {
                        MemoryTextBox textBox = (MemoryTextBox)this.Team1.Children[i++];
                        textBox.Text = member.Player;
                    }
                }

                this.DataContext = MatchRow;
                this.TxtBowls0.DataContext = MatchRow.Teams[0];
                this.TxtBowls1.DataContext = MatchRow.Teams[1];
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
            TabbedDebug.ResetBlock($"MatchCard.HndUpdateText");

            // Does name exist in another team?
            bool contains = this.MatchRow
                                .Teams
                                .SelectMany(team => team.Members)
                                .Any(member => member.Player.Equals(e.After));

            TabbedDebug.StartBlock($"Contains '{e.After}' = {contains}");

            if (contains) {
                // if the player already exists reject.
                MessageBox.Show($"Player {e.After} previously assigned.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.TextBox.Text = e.Before;
            }
            else {
                TeamStackPanel parent = (TeamStackPanel)e.TextBox.Parent;
                TabbedDebug.WriteLine($"Team Index = {parent.TeamIndex}");

                // Remove new name from the idle table
                if (this.MatchRow.Round.IdlePlayers.Has(e.After)) {
                    this.MatchRow.Round.IdlePlayers.Get(e.After)!.Remove();
                }

                // Add new name to the teams table
                if (!e.After.IsEmpty()) {
                    this.MatchRow.League.PlayerTable.AddRowIf(e.After);
                    this.MatchRow.Teams[parent.TeamIndex]!.Members.Add(e.After);
                }

                TabbedDebug.StartBlock($"e.Before ({e.Before}) Is Empty '{e.Before.IsEmpty()}'");
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

        private void Clear() {
            foreach (TextBox textBox in this.Team0.Children) textBox.Text = string.Empty;
            foreach (TextBox textBox in this.Team1.Children) textBox.Text = string.Empty;
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtEnds.Text = "0";
        }
    }
}
