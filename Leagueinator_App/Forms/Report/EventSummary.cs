using Leagueinator.Utility;
using Leagueinator.Utility.Seek;

namespace Leagueinator.Model {
    /// <summary>
    /// Object for displaying a row in the FormReport class for a single round.
    /// </summary>
    internal class EventSummary : HasRefresh {
        public LeagueEvent lEvent;
        private List<Match> matches;
        private List<Team> teams;
        private PlayerInfo player;
        private Score score;

        public EventSummary(LeagueEvent lEvent, PlayerInfo player) {
            if (lEvent == null) throw new NullReferenceException("lEvent");
            if (player == null) throw new NullReferenceException("player");

            this.lEvent = lEvent;
            this.player = player;

            this.matches = this.lEvent.Matches.Where(m => m.Players.Contains(player)).ToList();
            this.teams = this.lEvent.SeekDeep<Team>().Where(t => t.Players.Contains(player)).ToList();

            this.Refresh();
        }

        public string Name {
            get => this.player.Name;
        }

        public int Bowls {
            get => this.teams.Select(t => t.Bowls).Sum();
        }

        public int Ends { get => this.matches.Select(m => m.EndsPlayed).Sum(); }

        public int Wins { get => this.score.Wins; }
        public int Ties { get => this.score.Ties; }
        public int Losses { get => this.score.Losses; }
        public int Against { get => this.score.Against; }
        public int PointsFor { get => this.score.PointsFor; }
        public int PlusFor { get => this.score.PlusFor; }
        public int PointsAgainst { get => this.score.PointsAgainst; }
        public int PlusAgainst { get => this.score.PlusAgainst; }

        public void Refresh() {
            this.score = new Score();

            foreach (Match match in this.matches) {
                Team team = match.Teams.Values.NotNull().Where(t => t.Players.Contains(this.player)).ToList().First();
                this.score += new Score(match, team);
            }
        }
    }
}
