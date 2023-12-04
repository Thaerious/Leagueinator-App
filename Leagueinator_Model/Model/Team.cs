﻿using Leagueinator.Utility;
using Leagueinator.Utility.ObservableDiscreteCollection;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Leagueinator.Model {
    public class TeamUpdateArgs {
        public readonly string Field;
        public readonly object? PrevValue;
        public readonly object? NewValue;

        public TeamUpdateArgs(string field, object? prevValue, object? newValue) {
            this.Field = field;
            this.PrevValue = prevValue;
            this.NewValue = newValue;
        }
    }

    [Serializable]
    public class Team {
        public delegate void TeamUpdateHnd(Team source, TeamUpdateArgs args);
        public event TeamUpdateHnd OnUpdate = delegate { };
        public LeagueSettings Settings;

        public int Bowls {
            get => _bowls;
            set {
                if (this._bowls == value) return;
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.VALUE, "bowls"));
                OnUpdate.Invoke(this, new TeamUpdateArgs("bowls", _bowls, value));
                this._bowls = value;
            }
        }

        [JsonIgnore] [DoSeek] public NullableDiscreteList<PlayerInfo> Players => this._players;
        
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            this.Players.CollectionChanged += (src, args) => {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.COMPOSITION, "players"));
            };
        }

        [JsonConstructor]
        public Team(LeagueSettings settings) {
            this.Settings = settings;
            this._players = new NullableDiscreteList<PlayerInfo>(settings.TeamSize);

            this.Players.CollectionChanged += (src, args) => {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.COMPOSITION, "players"));
            };
        }

        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();
            xsb.OpenTag("Team");
            xsb.Attribute("hash", this.GetHashCode("X"));
            xsb.Attribute("bowls", this.Bowls);

            foreach (PlayerInfo? player in this.Players.Values) {
                if (player is null) continue;
                xsb.AppendXML(player.ToXML());
            }

            xsb.CloseTag();

            return xsb;
        }

        public override string ToString() {
            return this.ToXML().ToString();
        }

        public void Clear() {
            this._players.Clear();
            this.Bowls = 0;
        }

        public void AddPlayer(PlayerInfo player) {
            this.Players[this.Players.Count] = player;
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

        [JsonProperty] public readonly NullableDiscreteList<PlayerInfo> _players;

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
