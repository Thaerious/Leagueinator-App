using Leagueinator.Utility;
using Leagueinator.Utility.ObservableDiscreteCollection;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Leagueinator.Model {
    [Serializable]
    public class Team {
        public LeagueSettings Settings;
        public int Bowls {
            get => _bowls;
            set {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.VALUE, "bowls"));
                this._bowls = value;
            }
        }

        [JsonIgnore] [DoSeek] public ObservableDiscreteCollection<PlayerInfo> Players => this._players;
        
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            this.Players.CollectionChanged += (src, args) => {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.COMPOSITION, "players"));
            };
        }

        [JsonConstructor]
        public Team(LeagueSettings settings) {
            this.Settings = settings;
            this._players = new ObservableDiscreteCollection<PlayerInfo>(settings.TeamSize);

            this.Players.CollectionChanged += (src, args) => {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.COMPOSITION, "players"));
            };
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

        [JsonProperty] public readonly ObservableDiscreteCollection<PlayerInfo> _players;

        [JsonProperty] private int _bowls = 0;
    }

    public class TeamBye : Team {
        public TeamBye(LeagueSettings settings) : base(settings) {
            for (int i = 0; i < settings.TeamSize; i++) {
                this.Players[i] = new PlayerBye();
            }
        }
    }
}
