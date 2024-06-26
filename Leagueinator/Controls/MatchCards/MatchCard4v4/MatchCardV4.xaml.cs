using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    [TimeTrace]
    public partial class MatchCardV4 : MatchCard {
        public MatchCardV4() : base() {
            this.InitializeComponent();
        }

        public override MatchFormat MatchFormat {
            get => MatchFormat.VS4;
        }

        public override MatchRow MatchRow {
            get => this._matchRow ?? throw new InvalidOperationException("MatchRow not initialized");
            set {
                Debug.WriteLine($"MatchCardV4.MatchRow.Set {value.Round.Index}.{value.Lane}");
                Debug.WriteLine(value.PrettyPrint());
                base.MatchRow = value;

                value.ReduceTeams(2);
                value.IncreaseTeams(2);
                value.TrimTeams(4);

                this.CheckTie0.IsChecked = this.MatchRow.Teams[0]!.Tie == 1;
                this.CheckTie1.IsChecked = this.MatchRow.Teams[1]!.Tie == 1;
                this.DataContext = value;

                Debug.WriteLine($"{value.Round.Index}.{value.Lane}.{value.Teams[0].Index}");
                Debug.WriteLine(value.Teams[0].PrettyPrint());
                this.TeamCard0.TeamRow = value.Teams[0];
                Debug.WriteLine($"{value.Round.Index}.{value.Lane}.{value.Teams[1].Index}");
                Debug.WriteLine(value.Teams[1].PrettyPrint());
                this.TeamCard1.TeamRow = value.Teams[1];
            }
        }

        public override void Clear() {
            this.TeamCard0.Clear();
            this.TeamCard1.Clear();
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtEnds.Text = "0";
        }

        private void HndCheckTie(object sender, RoutedEventArgs e) {
            if (this.MatchRow is null) throw new NullReferenceException(nameof(this.MatchRow));
            if (this.MatchRow.Teams[0] is null) throw new NullReferenceException();
            if (this.MatchRow.Teams[1] is null) throw new NullReferenceException();
            if (sender is not CheckBox checkBox) return;

            if (checkBox.IsChecked == true && checkBox == this.CheckTie0) {
                this.CheckTie1.IsChecked = false;
            }
            else if (checkBox.IsChecked == true && checkBox == this.CheckTie1) {
                this.CheckTie0.IsChecked = false;
            }

            if (this.CheckTie0.IsChecked == true && this.CheckTie1.IsChecked == false) {
                this.MatchRow.Teams[0]!.Tie = 1;
                this.MatchRow.Teams[1]!.Tie = -1;
            }
            else if (this.CheckTie0.IsChecked == false && this.CheckTie1.IsChecked == true) {
                this.MatchRow.Teams[0]!.Tie = -1;
                this.MatchRow.Teams[1]!.Tie = 1;
            }
            else {
                this.MatchRow.Teams[0]!.Tie = 0;
                this.MatchRow.Teams[1]!.Tie = 0;
            }
        }
    }
}
