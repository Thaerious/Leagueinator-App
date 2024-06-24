using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard4321 : MatchCard {
        public MatchCard4321() : base() {
            InitializeComponent();
        }

        public override MatchFormat MatchFormat {
            get => MatchFormat.A4321;
        }

        public override MatchRow MatchRow {
            get => _matchRow ?? throw new InvalidOperationException("MatchRow not initialized");

            set {
                base.MatchRow = value;

                this.Deferred = delegate {
                    this.NormalizeTeams(4, 1);
                    this.DataContext = value;
                };
            }
        }

        private void ProcessMatchRow() {


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

            this.DataContext = MatchRow;
        }

        public override void Clear() {
            this.Team0.Clear();
            this.Team1.Clear();
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "0";
            this.TxtEnds.Text = "0";
        }


    }
}
