using Leagueinator.Utility;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Leagueinator.Model {
    [Serializable]
    public class Round {
        public readonly LeagueSettings Settings;

        [JsonProperty] [Model] public ObservableCollection<Match> Matches => this._matches;

        [Model]
        [JsonProperty]  public ObservableCollection<PlayerInfo> IdlePlayers { get; } = new ObservableCollection<PlayerInfo>();

        public List<Team> Teams => this.SeekDeep<Team>().Where(t => !t.Players.Values.IsEmpty()).ToList();

        public List<PlayerInfo> ActivePlayers => this.Matches.SeekDeep<PlayerInfo>().Unique();

        public List<PlayerInfo> AllPlayers {
            get {
                var list = new List<PlayerInfo>();
                _ = list.AddUnique(this.IdlePlayers);
                _ = list.AddUnique(this.ActivePlayers);
                return list;
            }
        }

        public Round(LeagueSettings settings) {
            if (settings is null) throw new NullReferenceException("Settings");
            this.Settings = settings;
            for (int i = 0; i < settings.LaneCount; i++) this._matches.Add(new Match(settings));
        }

        public Round(List<PlayerInfo> idlePlayers, LeagueSettings settings) : this(settings) {
            this.Settings = settings;
            this.IdlePlayers = new ObservableCollection<PlayerInfo>(idlePlayers);
        }

        public void CopyFrom(Round that) {
            this.IdlePlayers.Clear();

            for (int m = 0; m < this.Matches.Count; m++) {
                for (int t = 0; t < this.Matches[m].Teams.Length; t++) {
                    for (int p = 0; p < this.Matches[m].Teams[t].Players.Count; p++) {
                        this.Matches[m].Teams[t].Players[p] = that.Matches[m].Teams[t].Players[p];
                    }
                }
            }
        }

        public void InjectByes() {
            foreach (Match match in this.Matches) {
                if (match.Teams[0].Players.Values.IsEmpty() && !match.Teams[1].Players.Values.IsEmpty()) match.Teams[0] = new TeamBye(this.Settings);
                if (!match.Teams[0].Players.Values.IsEmpty() && match.Teams[1].Players.Values.IsEmpty()) match.Teams[1] = new TeamBye(this.Settings);
            }
        }

        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();
            _ = xsb.OpenTag("Round", $"hash='{this.GetHashCode():X}'");
            _ = xsb.InlineTag("Players", this.AllPlayers.DelString());
            _ = xsb.InlineTag("Idle", this.IdlePlayers.DelString());

            for (int i = 0; i < this._matches.Count; i++) {
                _ = xsb.AppendXML(this._matches[i].ToXML(i));
            }

            _ = xsb.CloseTag();
            return xsb;
        }

        public override string ToString() {
            return this.ToXML().ToString();
        }

        /// <summary>
        /// Move all players in teams to idle, 
        /// retaining players previously in idle.
        /// </summary>
        public void ResetPlayers() {
            List<PlayerInfo> players = this.AllPlayers;
            this.IdlePlayers.Clear();

            foreach (PlayerInfo player in players) {
                this.IdlePlayers.Add(player);
            }

            foreach (Match match in this._matches) {
                match.ClearPlayers();
            }
        }

        public Match GetMatch(PlayerInfo player) {
            foreach (Match match in this.Matches) {
                if (match.Players.Contains(player)) return match;
            }
            throw new InvalidOperationException("Round does not contain a match with player");
        }

        public Team GetTeam(PlayerInfo player) {
            foreach (Team team in this.Teams) {
                if (team.Players.Contains(player)) return team;
            }
            throw new InvalidOperationException("Round does not contain a team with player");
        }

        private readonly ObservableCollection<Match> _matches = new();
    }
}
