using Leagueinator.Model.Tables;
using System.Diagnostics;
using System.Windows;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard4321 : MatchCard {
        private bool isLoaded = false;

        public MatchCard4321() : base(){
            InitializeComponent();
            this.Loaded += this.HndLoaded;
        }

        private void HndLoaded(object sender, RoutedEventArgs e) {
            if (!this.isLoaded) this.ProcessMatchCard();
            this.isLoaded = true;
        }

        public override MatchFormat MatchFormat {
            get => MatchFormat.A4321;
        }

        public override MatchRow MatchRow {
            get => _matchRow ?? throw new InvalidOperationException("MatchRow not initialized");
            set {
                this._matchRow = value;
                if (!this.isLoaded) return;
                this.ProcessMatchCard();
            }
        }

        private void ProcessMatchCard() {
            if (MatchRow is null) {
                this.Clear();
                this.DataContext = null;
                return;
            }

            foreach (TeamRow teamRow in MatchRow.Teams) {
                TeamCard? teamCard = this.GetTeamCard(teamRow.Index);

                Debug.WriteLine($"Team Card #{teamRow.Index} is null {teamCard is null}");
                if (teamCard is null) {
                    teamRow.Remove();
                    return;
                }

                teamCard.Clear();

                if (teamRow.Members.Count > 0) {
                    teamCard.AddName(teamRow.Members[0].Player);
                }
            }

            //this.DataContext = MatchRow;
            //this.TxtEnds.DataContext = MatchRow;
        }

        // TODO DELETE
        ///// <summary>
        ///// Triggered when the value of a players name (MemoryTextBox) is changed.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ///// <exception cref="NullReferenceException"></exception>
        //private void HndUpdateText(object sender, MemoryTextBoxArgs e) {
        //    if (this.MatchRow is null) throw new NullReferenceException(nameof(MatchRow));

        //        TeamCard parent = e.TextBox.Ancestors<TeamCard>()[0];

        //        // Remove new name from the idle table
        //        if (this.MatchRow.RoundIndex.IdlePlayers.Has(e.After)) {
        //            this.MatchRow.RoundIndex.IdlePlayers.Get(e.After)!.Remove();
        //        }

        //        // Add new name to the teams table
        //        if (!e.After.IsEmpty()) {
        //            this.MatchRow.League.PlayerTable.AddRowIf(e.After);
        //            this.MatchRow.Teams[parent.TeamIndex]!.Members.Add(e.After);
        //        }

        //        // Remove the old name from the members table
        //        if (!e.Before.IsEmpty()) {
        //            TeamRow teamRow = this.MatchRow.Teams[parent.TeamIndex] ?? throw new NullReferenceException();
        //            MemberRow memberRow = teamRow.Members.Get(e.Before) ?? throw new NullReferenceException();
        //            memberRow.Remove();
        //        }

        //    if (e.Cause == Cause.EnterPressed) {
        //        StackPanel stackPanel = (StackPanel)e.TextBox.Parent;
        //        int index = stackPanel.Children.IndexOf(e.TextBox);
        //        if (index + 1 < stackPanel.Children.Count) {
        //            stackPanel.Children[index + 1].Focus();
        //        }
        //    }
        //}

        public override void Clear() {
            this.Team0.Clear();
            this.Team1.Clear();
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtEnds.Text = "0";
        }
    }
}
