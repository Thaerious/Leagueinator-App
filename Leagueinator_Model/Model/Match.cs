using Leagueinator.Utility;
using Newtonsoft.Json;

namespace Leagueinator.Model {
    [Serializable]
    public class Match {
        public readonly LeagueSettings Settings;

        [JsonProperty] private readonly Team[] _teams;

        [Model] public Team[] Teams => this._teams;

        public List<PlayerInfo> Players => this.SeekDeep<PlayerInfo>().Unique();

        /// <summary>
        /// Count the number of teams that have more than 0 players.
        /// </summary>
        public int Count => this._teams.Where(t => t != null && t.Players.Values.IsNotEmpty()).Count();

        public int EndsPlayed { get; set; }

        public Match(LeagueSettings settings) {
            this.Settings = settings;
            this.EndsPlayed = this.Settings.NumberOfEnds;
            this._teams = new Team[settings.MatchSize].Populate(() => new Team(settings));
        }

        public XMLStringBuilder ToXML(int lane) {
            var xsb = new XMLStringBuilder();
            _ = xsb.OpenTag("Match", $"lane='{lane}'", $"hash='{this.GetHashCode():X}'");
            foreach (Team team in this.Teams) {
                _ = xsb.AppendXML(team.ToXML());
            }
            _ = xsb.CloseTag();
            return xsb;
        }

        public override string ToString() {
            return this.ToXML(0).ToString();
        }

        public void ClearPlayers() {
            for (int i = 0; i < this.Teams.Length; i++) {
                this.Teams[i].Clear();
            }
        }

        public Team? WinningTeam() {
            foreach (Team team in this.Teams) {
                if (this.IsWinningTeam(team)) return team;
            }
            return null;
        }

        public bool IsWinningTeam(Team team) {
            foreach (Team against in this.Teams) {
                if (against == team) continue;
                if (against.Bowls >= team.Bowls) return false;
            }
            return true;
        }

        public Team? LosingTeam() {
            foreach (Team team in this.Teams) {
                if (this.IsLosingTeam(team)) return team;
            }
            return null;
        }

        public bool IsLosingTeam(Team team) {
            foreach (Team against in this.Teams) {
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

        public void CopyFrom(Match that) {
            throw new NotImplementedException();
        }
    }
}
