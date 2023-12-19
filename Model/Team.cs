using Model.Tables;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;

namespace Model {

    /// <summary>
    /// A view of TeamTableView paired with a EventTableRow from EventTable.
    /// The public methods may update the data set.
    /// </summary>
    public class Team : DataView, IDeleted {

        public League League { get => this.LeagueEvent.League; }

        public LeagueEvent LeagueEvent { get => this.Round.LeagueEvent; }

        public Round Round { get => this.Match.Round; }

        public Match Match { get; }

        public int TeamIndex { get; }

        public int EventTableUID { get => (int)EventTableRow[EventTable.COL.ID]; }

        public List<string> Players {
            get => this.GetPlayers();
        }

        private DataRow EventTableRow { get; }

        public bool Deleted { get; private set; } = false;
        public int Bowls {
            get => (int)this.EventTableRow[EventTable.COL.BOWLS];
            set => this.EventTableRow[EventTable.COL.BOWLS] = value;
        }

        internal Team(Match match, DataRow eventTableRow, int teamIndex) : base(match.League.TeamTable) {
            this.Match = match;
            this.EventTableRow = eventTableRow;
            this.TeamIndex = teamIndex;

            this.RowFilter = $"{TeamTable.COL.EVENT_TABLE_UID} = {this.EventTableUID}";

            var row = this.League.EventTable.GetRow(
                eventName: this.LeagueEvent.EventName,
                round: this.Round.RoundIndex,
                lane: this.Match.Lane,
                teamIdx: this.TeamIndex
            );
        }


        public bool AddPlayer(string name) {
            DeletedException.ThrowIf(this);
            if (this.HasPlayer(name)) return false;

            this.League.TeamTable.AddRow(
                eventTableUID: this.EventTableUID,
                playerName: name
            );

            return true;
        }

        /// <summary>
        /// Add the player to the TeamTable for this team.
        /// </summary>
        /// <param playerName="playerName"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool HasPlayer(string playerName) {
            DeletedException.ThrowIf(this);

            this.Sort = TeamTable.COL.PLAYER_NAME;
            int index = this.Find(playerName);
            return index != -1;
        }

        private List<string> GetPlayers() {
            DeletedException.ThrowIf(this);

            List<string> list = [];
            var table = this.League.TeamTable;

            foreach (DataRowView row in this) {
                list.Add((string)row[TeamTable.COL.PLAYER_NAME]);
            }

            return list;
        }

        /// <summary>
        /// Remove a player from this team.
        /// If the player doesn't exist no change is made.
        /// </summary>
        /// <param playerName="v"></param>
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
            var eventTable = this.League.EventTable;
            eventTable.Rows.Remove(this.EventTableRow);

            this.Deleted = true;
        }

        public void MovePlayer(string player, int newTeamIndex) {
            this.League.TeamTable.RemoveRows(this.EventTableUID, player);
            this.League.IdleTable.AddRow(this.LeagueEvent.EventName, this.Round.RoundIndex, player);
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this) + "\n" +
                   this.EventTableRow.PrettyPrint();
        }
    }
}

