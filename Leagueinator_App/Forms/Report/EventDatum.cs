using Leagueinator.Utility;
using Leagueinator.Utility.Seek;

namespace Leagueinator.Model {
    /// <summary>
    /// Object for displaying a row in the FormReport class for a single round.
    /// </summary>
    internal class EventDatum : IComparable<EventDatum>{
        public readonly LeagueEvent lEvent;
        public readonly PlayerInfo Player;

        private List<Match> matches;
        private List<Team> teams;
        private Score score;

        public EventDatum(LeagueEvent lEvent, PlayerInfo player) {
            if (lEvent == null) throw new NullReferenceException("lEvent");
            if (player == null) throw new NullReferenceException("Player");

            this.lEvent = lEvent;
            this.Player = player;

            this.matches = this.lEvent.Matches.Where(m => m.Players.Contains(player)).ToList();
            this.teams = this.lEvent.SeekDeep<Team>().Where(t => t.Players.Contains(player)).ToList();

            this.score = this.BuildScore();
        }

        public string Name {
            get => this.Player.Name;
        }

        public int Bowls {
            get => this.teams.Select(t => t.Bowls).Sum();
        }

        public int Ends { get => this.matches.Select(m => m.EndsPlayed).Sum(); }

        public int Rank { get; set; } = -1;
        public int Wins { get => this.score.Wins; }
        public int Ties { get => this.score.Ties; }
        public int Losses { get => this.score.Losses; }
        public int Against { get => this.score.Against; }
        public int PointsFor { get => this.score.PointsFor; }
        public int PlusFor { get => this.score.PlusFor; }
        public int PointsAgainst { get => this.score.PointsAgainst; }
        public int PlusAgainst { get => this.score.PlusAgainst; }

        public Score BuildScore() {
            Score score = new Score();

            foreach (Match match in this.matches) {
                Team team = match.Teams.Values.NotNull().Where(t => t.Players.Contains(this.Player)).ToList().First();
                score += new Score(match, team);
            }

            return score;
        }

        public int CompareTo(EventDatum? that) {
            if (that == null) return 1;
            return this.score.CompareTo(that.score);
        }
    }
}
