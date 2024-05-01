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
        private MatchRow? _matchRow;

        public MatchCard() {
            InitializeComponent();
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(HndUpdateText));
        }

        internal CardTarget? CardTarget { get; set; }

        public MatchRow? MatchRow {
            get => _matchRow;
            set {
                _matchRow = value;
                if (MatchRow is null) {
                    this.Clear();
                    return;
                }
                if (MatchRow.Teams[0] is not null) {
                    int i = 0;
                    foreach (MemberRow member in MatchRow.Teams[0].Members) {
                        TextBox textBox = (TextBox)this.Team0.Children[i++];
                        textBox.Text = member.Player;
                    }
                }
                if (MatchRow.Teams[1] is not null) {
                    int i = 0;
                    foreach (MemberRow member in MatchRow.Teams[1].Members) {
                        TextBox textBox = (TextBox)this.Team1.Children[i++];
                        textBox.Text = member.Player;
                    }
                }
            }
        }

        private void HndUpdateText(object sender, MemoryTextBoxArgs e) {
            if (this.MatchRow is null) throw new NullReferenceException(nameof(MatchRow));

            // Does name exist in another team?
            bool contains = this.MatchRow
                                .Teams
                                .SelectMany(team => team.Members)
                                .Any(member => member.Player.Equals(e.After));

            Debug.WriteLine($"Contains {contains}");

            if (contains) {
                // if the player already exists reject.
                MessageBox.Show($"Player {e.After} is already playing.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                // Remove the old name from the team
                if (!e.Before.IsEmpty()) {
                    this.MatchRow.Teams[parent.TeamIndex]!.Members.Get(e.Before).Remove();
                }

                // Add old name to the idle table
                if (!e.Before.IsEmpty()) {
                    this.MatchRow.Round.IdlePlayers.Add(e.Before);
                }

                Debug.WriteLine($"add player to team {parent.TeamIndex}");
            }


            Debug.WriteLine($"Before: {e.Before}\nAfter: {e.After}\nCause: {e.Cause}");
        }

        private void Clear() {
            foreach (TextBox textBox in this.Team0.Children) textBox.Text = string.Empty;
            foreach (TextBox textBox in this.Team1.Children) textBox.Text = string.Empty;
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "1";
            this.TxtEnds.Text = "0";
        }
    }
}
