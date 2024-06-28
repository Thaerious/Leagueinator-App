using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard4321 : MatchCard {
        public MatchCard4321() : base() {
            this.InitializeComponent();
        }

        public override MatchFormat MatchFormat {
            get => MatchFormat.A4321;
        }

        public override MatchRow MatchRow {
            get => this._matchRow ?? throw new InvalidOperationException("MatchRow not initialized");

            set {
                base.MatchRow = value;
                value.ReduceTeams(4);
                value.IncreaseTeams(4);
                value.TrimTeams(1);

                this.TeamCard0.TeamRow = value.Teams[0];
                this.TeamCard1.TeamRow = value.Teams[1];
                this.TeamCard2.TeamRow = value.Teams[2];
                this.TeamCard3.TeamRow = value.Teams[3];

                this.DataContext = value;
            }
        }

        private void ProcessMatchRow() {


            if (this.MatchRow is null) {
                this.Clear();
                this.DataContext = null;
                return;
            }

            foreach (TeamRow teamRow in this.MatchRow.Teams) {
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

            this.DataContext = this.MatchRow;
        }

        public override void Clear() {
            this.TeamCard0.Clear();
            this.TeamCard1.Clear();
            this.TeamCard2.Clear();
            this.TeamCard3.Clear();
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtBowls2.Text = "0";
            this.TxtBowls3.Text = "0";
            this.TxtEnds.Text = "0";
        }


    }
}
