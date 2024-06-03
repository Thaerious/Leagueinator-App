using Leagueinator.Model.Tables;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCardV4 : MatchCard {
        public MatchCardV4() {
            InitializeComponent();
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(HndUpdateText));
        }

        public override MatchRow MatchRow {
            get => this._matchRow ?? throw new InvalidOperationException("MatchRow not initialized");
            set {
                this._matchRow = value;
                if (MatchRow is null) {
                    this.Clear();
                    this.DataContext = null;
                    return;
                }

                if (MatchRow.Teams.Count != 2) throw new NullReferenceException("MatchRow in MatchCard must have exactly 2 teams");

                this.CheckTie0.IsChecked = MatchRow.Teams[0]!.Tie == 1;
                this.CheckTie1.IsChecked = MatchRow.Teams[1]!.Tie == 1;

                this.DataContext = MatchRow;
                this.Team0.TeamRow = MatchRow.Teams[0];
                this.Team1.TeamRow = MatchRow.Teams[1];
                this.TxtEnds.DataContext = MatchRow;
            }
        }

        public void Clear() {
            this.Team0.Clear();
            this.Team1.Clear();
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
