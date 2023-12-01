﻿using Leagueinator.Utility;
using Leagueinator.Utility.Seek;
using Leagueinator_Model.Model.Tables;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Leagueinator.Model {
    [Serializable]
    public class LeagueEvent {
        [JsonProperty] public readonly string Date;
        [JsonProperty] public readonly LeagueSettings Settings;
        [JsonProperty] public readonly string Name;

        [DoSeek]
        [JsonProperty]
        public ObservableCollection<Round> Rounds { get; } = new();

        [JsonIgnore] public List<Match> Matches => this.SeekDeep<Match>().ToList();

        [JsonIgnore] public List<Team> Teams => this.SeekDeep<Team>().Where(t => !t.Players.Values.IsEmpty()).ToList();

        [JsonIgnore] public List<PlayerInfo> Players => this.SeekDeep<PlayerInfo>().Unique();


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            this.Rounds.CollectionChanged += (src, args) => {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.COMPOSITION, "Rounds"));
            };
        }

        [JsonConstructor]
        public LeagueEvent(string date, string name, LeagueSettings settings) {
            this.Date = date;
            this.Name = name;
            this.Settings = settings;

            this.Rounds.CollectionChanged += (src, args) => {
                LeagueSingleton.Invoke(this, new ModelUpdateEventHandlerArgs(Change.COMPOSITION, "Rounds"));
            };
        }

        /// <summary>
        /// Add a new empty round to this event.
        /// The new round will contain all players in this event as idle players.
        /// The new round contains empty matches equal to the lane count.
        /// </summary>
        /// <returns>The new round.</returns>
        public Round NewRound() {
            var round = new Round(this.SeekDeep<PlayerInfo>().Unique(), this.Settings);
            this.Rounds.Add(round);
            return round;
        }

        /// <summary>
        /// Add an existing round to this event.
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public Round AddRound(Round round) {
            this.Rounds.Add(round);
            return round;
        }

        /// <summary>
        /// Replace a round in this event with a specified round.
        /// </summary>
        /// <param name="replace"></param>
        /// <param name="with"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ReplaceRound(Round replace, Round with) {
            int index = this.Rounds.IndexOf(replace);

            if (index < 0) {
                throw new ArgumentException(
                    $"Attempting to replace round that is not a member of League Event"
                );
            }

            this.Rounds[index] = with;
        }

        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();

            xsb.OpenTag("Event");
            xsb.Attribute("name", this.Name);
            xsb.Attribute("hash", this.GetHashCode("X"));
            xsb.InlineTag("Players");
            xsb.InnerText(this.SeekDeep<PlayerInfo>().DelString());

            foreach (Round round in this.Rounds) {
                xsb.AppendXML(round.ToXML());
            }
            xsb.CloseTag();

            return xsb;
        }
        public override string ToString() {
            return this.ToXML().ToString();
        }

        public Round PrevRound(Round round) {
            if (this.Rounds.Count < 2) throw new InvalidOperationException("Previous round not available");
            if (this.Rounds[0] == round) throw new InvalidOperationException("Previous round not available");

            for (int i = 1; i < this.Rounds.Count; i++) {
                if (this.Rounds[i] == round) return this.Rounds[i - 1];
            }
            throw new InvalidOperationException("Previous round not found");
        }

        public Round NextRound(Round round) {
            if (this.Rounds.Count < 2) throw new InvalidOperationException("Next round not available");
            if (this.Rounds.Last() == round) throw new InvalidOperationException("Next round not available");

            for (int i = 0; i < this.Rounds.Count - 1; i++) {
                if (this.Rounds[i] == round) return this.Rounds[i + 1];
            }
            throw new InvalidOperationException("Next round not found");
        }

        internal void RemoveRound(Round round) {
            this.Rounds.Remove(round);
        }

        public DataSet ToDataSet() {
            DataSet dataSet = new();

            var eventTable = EventTable.MakeEventTable();
            var teamTable = TeamTable.MakeTeamTable();

            PopulateEventTable(new EventTable(eventTable), new TeamTable(teamTable));
            dataSet.Tables.Add(eventTable);
            dataSet.Tables.Add(teamTable);

            return dataSet;
        }

        private void PopulateEventTable(EventTable eventTable, TeamTable teamTable) {
            for (int round = 0; round < this.Rounds.Count; round++) {
                for (int lane = 0; lane < this.Rounds[round].Matches.Count; lane++) {
                    var match = this.Rounds[round].Matches[lane];
                    if (match.Players.Count == 0) continue;

                    foreach (Team team in match.Teams.Values.NotNull()) {
                        int teamId = teamTable.TryAddTeam(team.Players.toArray().Select(p => p.Name).ToArray());
                        eventTable.AddRow(round, lane, teamId, team.Bowls, match.EndsPlayed);
                    }
                }
            }
        }
    }

    public interface IModelLeagueEvent {
        LeagueEvent LeagueEvent { get; set; }
    }
}
