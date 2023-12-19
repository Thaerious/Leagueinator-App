using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model {

    /// <summary>
    /// A view of TeamTable paired with a Row from EventTable.
    /// The public methods may update the data set.
    /// </summary>
    public class Team : DataView, IDeleted{
        public Match Match { get; }

        public int TeamIndex { get; }

        public List<string> Players {
            get => this.GetPlayers();
        }

        private DataRow Row { get; }

        public bool Deleted { get; private set; } = false;

        internal Team(Match match, DataRow row, int teamIndex) : base(match.Round.LeagueEvent.League.TeamTable) {            
            this.Match = match;
            this.Row = row;
            this.TeamIndex = teamIndex;
        }

        public bool AddPlayer(string name) {
            DeletedException.ThrowIf(this);
            if (this.HasPlayer(name)) return false;

            this.Match.Round.LeagueEvent.League.TeamTable.AddRow(
                eventName:  (string) this.Row[EventTable.COL.EVENT_NAME],
                round:      (int) this.Row[EventTable.COL.ROUND],
                teamIdx:    this.TeamIndex,
                playerName: name
            );

            return true;
        }

        public bool HasPlayer(string name) {
            DeletedException.ThrowIf(this);
            DataTable table = this.Table ?? throw new NullReferenceException();
            string eventName = (string)this.Row[EventTable.COL.EVENT_NAME];

            var rows = table.Select($"{TeamTable.COL.TEAM_IDX} = {this.TeamIndex}" +
                                    $" AND {TeamTable.COL.PLAYER_NAME} = '{name}'" +
                                    $" AND {TeamTable.COL.EVENT_NAME} = '{eventName}'");
            return rows.Length > 0;
        }

        public List<string> GetPlayers() {
            DeletedException.ThrowIf(this);
            List<string> list = [];
            var table = this.Match.Round.LeagueEvent.League.TeamTable;

            var view = new DataView(table) {
                RowFilter = $"{TeamTable.COL.TEAM_IDX} = {this.TeamIndex}"
            };

            foreach (DataRowView row in view) {
                list.Add((string)row[TeamTable.COL.PLAYER_NAME]);
            }

            return list;
        }

        /// <summary>
        /// Remove a player from this team.
        /// If the player doesn't exist no change is made.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>True if a change was made</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool RemovePlayer(string name) {
            DeletedException.ThrowIf(this);
            this.Sort = TeamTable.COL.PLAYER_NAME;
            int rowIndex = this.Find(name);

            if (rowIndex == -1) return false;

            if (this.Table is null) throw new NullReferenceException();
            this.Table.Rows.RemoveAt(rowIndex);

            return true;
        }

        public void Delete() {
            DeletedException.ThrowIf(this);

            foreach (string player in this.Players) this.RemovePlayer(player);
            var eventTable = this.Match.Round.LeagueEvent.League.EventTable;
            eventTable.Rows.Remove(this.Row);

            this.Deleted = true;
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this) + "\n" +
                   this.Row.PrettyPrint();
        }
    }
}
