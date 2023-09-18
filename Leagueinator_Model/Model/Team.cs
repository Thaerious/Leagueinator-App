using Leagueinator.Utility;
using Leagueinator.Utility.ObservableDiscreteCollection;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Leagueinator.Model {
    [Serializable]
    public class Team {
        public readonly LeagueSettings Settings;
        public int Bowls = 0;  // number of bowls for

        [JsonIgnore] [DoSeek] public ObservableDiscreteCollection<PlayerInfo> Players => this._players;

        public Team(LeagueSettings settings) {
            this.Settings = settings;
            this._players = new ObservableDiscreteCollection<PlayerInfo>(settings.TeamSize);
        }

        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();
            _ = xsb.OpenTag("Team", $"bowls='{this.Bowls}' hash='{this.GetHashCode():X}'");

            foreach (PlayerInfo? player in this.Players.Values) {
                if (player is null) continue;
                _ = xsb.AppendXML(player.ToXML());
            }

            _ = xsb.CloseTag();

            return xsb;
        }

        public override string ToString() {
            return this.ToXML().ToString();
        }

        public void Clear() {
            this._players.Clear();
        }

        public void AddPlayer(PlayerInfo player) {
            this.Players[^1] = player;
        }

        /// <summary>
        /// Remove a player from this Team.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>True, if a change was made.</returns>
        public void RemovePlayer(PlayerInfo player) {
            this.Players.RemoveValue(player);
        }

        public void CopyFrom(Team team) {
            foreach (int key in team.Players.Keys) {
                this.Players[key] = team.Players[key];
            }
        }

        [JsonProperty] private readonly ObservableDiscreteCollection<PlayerInfo> _players;
    }

    public class TeamBye : Team {
        public TeamBye(LeagueSettings settings) : base(settings) {
            for (int i = 0; i < settings.TeamSize; i++) {
                this.Players[i] = new PlayerBye();
            }
        }
    }
}
