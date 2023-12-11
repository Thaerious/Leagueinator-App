using Leagueinator.Utility;
using Leagueinator.Utility.ObservableDiscreteCollection;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;

namespace Leagueinator.Model {
    [Serializable]
    public class Match {
        public readonly LeagueSettings Settings;               

        [JsonIgnore] [DoSeek] public DiscreteList<Team> Teams => this._teams;

        [JsonIgnore] public List<PlayerInfo> Players => this.SeekDeep<PlayerInfo>().Unique();

        /// <summary>
        /// Count the number of teams that have more than 0 players.
        /// </summary>
        [JsonIgnore] public int Count => this._teams.Values.Where(t => t != null && t.Players.Values.IsNotEmpty()).Count();

        public int EndsPlayed { get; set; }

        public Match(LeagueSettings settings) {
            this.Settings = settings;
            this.EndsPlayed = this.Settings.NumberOfEnds;
            this._teams = new DiscreteList<Team>(settings.MatchSize, settings);

            for (int i = 0; i < settings.MatchSize; i++) {
                this.Teams[i] = new Team(settings);
            }
        }

        public XMLStringBuilder ToXML(int lane) {
            var xsb = new XMLStringBuilder();
            xsb.OpenTag("Team");
            xsb.Attribute("hash", this.GetHashCode("X"));
            xsb.Attribute("lane", lane);
            
            foreach (Team team in this.Teams.Values.NotNull()) {
                xsb.AppendXML(team.ToXML());
            }
            xsb.CloseTag();
            return xsb;
        }

        public override string ToString() {
            return this.ToXML(0).ToString();
        }

        public void ClearPlayers() {
            for (int i = 0; i < this.Teams.Count; i++) {
                this.Teams[i].Clear();
            }
        }

        public Team? WinningTeam() {
            foreach (Team team in this.Teams.Values.NotNull()) {
                if (this.IsWinningTeam(team)) return team;
            }
            return null;
        }

        public bool IsWinningTeam(Team team) {
            foreach (Team against in this.Teams.Values.NotNull()) {
                if (against == team) continue;
                if (against.Bowls >= team.Bowls) return false;
            }
            return true;
        }

        public Team? LosingTeam() {
            foreach (Team team in this.Teams.Values.NotNull()) {
                if (this.IsLosingTeam(team)) return team;
            }
            return null;
        }

        public bool IsLosingTeam(Team team) {
            foreach (Team against in this.Teams.Values.NotNull()) {
                if (against == team) continue;
                if (against.Bowls <= team.Bowls) return false;
            }
            return true;
        }

        /// <summary>
        /// Add team to next available position in this match.
        /// </summary>
        /// <param name="team"></param>
        public void AddTeam(Team team) {
            this._teams[this.Count] = team;
        }

        public int SumBowls() {
            int sum = 0;
            foreach (var team in this.Teams) sum += team.Bowls;
            return sum;
        }

        public void CopyFrom(Match that) {
            this.ClearPlayers();
            for (int t = 0; t < this.Teams.Count; t++) {
                Team? from = that.Teams[t];
                Team? to = this.Teams[t];
                if (from == null || to == null) { continue; }
                to.CopyFrom(from);
            }
        }

        [JsonProperty] private readonly DiscreteList<Team> _teams;
    }
}
