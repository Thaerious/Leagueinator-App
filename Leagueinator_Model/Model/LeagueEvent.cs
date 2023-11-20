using Leagueinator.Utility;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
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
        /// The new round will contain all players (idle) found in this event.
        /// The round will be populated with empty matches equal to the lane cound.
        /// </summary>
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

            _ = xsb.OpenTag("Event", $"name='{this.Name}' hash='{this.GetHashCode():X}'");
            _ = xsb.InlineTag("Players", this.SeekDeep<PlayerInfo>().DelString());
            foreach (Round round in this.Rounds) {
                _ = xsb.AppendXML(round.ToXML());
            }
            _ = xsb.CloseTag();

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
            _ = this.Rounds.Remove(round);
        }

        public DataSet ToDataSet() {
            DataSet dataSet = new();
            
            var pTable = MakeEventTable();
            var playerTable = MakePlayersTable();
            PopulateEventTable(pTable, playerTable);            
            dataSet.Tables.Add(pTable);
            dataSet.Tables.Add(playerTable);

            var sumTable = MakeSummaryTable();
            PopulateSummaryTable(sumTable);
            dataSet.Tables.Add(sumTable);                       

            return dataSet;
        }

        private void PopulateEventTable(DataTable eventTable, DataTable playersTable) {
            for (int round = 0; round < this.Rounds.Count; round++) {
                for (int lane = 0; lane < this.Rounds[round].Matches.Count; lane++) {
                    var match = this.Rounds[round].Matches[lane];
                    if (match.Players.Count == 0) continue;

                    foreach (Team team in match.Teams.Values.NotNull()) {
                        var names = team.Players.Values.NotNull().Select(p => p.Name).ToList();
                        names.Sort();
                        var key =  String.Join(",", names);

                        var eRow = eventTable.NewRow();
                        eRow["round"] = round;
                        eRow["lane"] = lane;
                        eRow["team"] = key;
                        eRow["bowls"] = team.Bowls;
                        eRow["ends"] = match.EndsPlayed;
                        eventTable.Rows.Add(eRow);

                        foreach(PlayerInfo player in match.Players) {
                            var pRow = playersTable.NewRow();
                            pRow["event.uid"] = eRow["uid"];
                            pRow["name"] = player.Name;
                            playersTable.Rows.Add(pRow);
                        }
                    }
                }
            }
        }

        private void PopulateSummaryTable(DataTable table) {
            for (int round = 0; round < this.Rounds.Count; round++) {
                for (int lane = 0; lane < this.Rounds[round].Matches.Count; lane++) {
                    var match = this.Rounds[round].Matches[lane];
                    if (match.Players.Count == 0) continue;

                    foreach (Team team in match.Teams.Values.NotNull()) {
                        var names = team.Players.Values.NotNull().Select(p => p.Name).ToList();
                        names.Sort();
                        var key = String.Join(",", names);
                        var rows = table.Select($"team = '{key}'");

                        if (rows.Length == 0) {
                            var newRow = table.NewRow();
                            newRow["team"] = key;
                            newRow["bowls"] = 0;
                            newRow["ends"] = 0;
                            table.Rows.Add(newRow);
                        }

                        var row = table.Select($"team = '{key}'")[0];
                        row["bowls"] = team.Bowls + (int)row["bowls"];
                        row["ends"] = match.EndsPlayed + (int)row["ends"];
                    }
                }
            }
        }

        public static DataTable MakePlayersTable() {
            DataTable table = new DataTable("players");
            DataColumn column;

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "event.uid"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(string),
                ColumnName = "name"
            };
            table.Columns.Add(column);

            return table;
        }

        public static DataTable MakeEventTable() {
            DataTable table = new DataTable("event");
            DataColumn column;

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "uid",
                Unique = true,
                AutoIncrement = true
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "round"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "lane"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(string),
                ColumnName = "team"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "bowls"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "ends"
            };
            table.Columns.Add(column);

            return table;
        }

        public static DataTable MakeSummaryTable() {
            DataTable table = new DataTable("summary");
            DataColumn column;

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "uid",
                Unique = true,
                AutoIncrement = true
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(string),
                ColumnName = "team",
                Unique = true
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "bowls"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "ends"
            };
            table.Columns.Add(column);

            return table;
        }
    }

    public interface IModelLeagueEvent {
        LeagueEvent LeagueEvent { get; set; }
    }
}
