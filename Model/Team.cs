using Model.Tables;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;

namespace Model {

    /// <summary>
    /// A view of TeamTableView paired with a EventTableRow from RoundTable.
    /// </summary>
    public class Team : DataView, IDeleted {

        /// <summary>
        /// Retrieve the league this team belongs to.
        /// </summary>
        public League League { get => this.LeagueEvent.League; }

        /// <summary>
        /// Retrievet the league event this team belongs to.
        /// </summary>
        public LeagueEvent LeagueEvent { get => this.Round.LeagueEvent; }

        /// <summary>
        /// Retrieve the round this team belongs to.
        /// </summary>
        public Round Round { get => this.Match.Round; }

        /// <summary>
        /// Retrieve the match this team belongs to.
        /// </summary>
        public Match Match { get; }

        /// <summary>
        /// Retrive the team index of this team.
        /// </summary>
        public int TeamIndex { get; }

        /// <summary>
        /// Property to get the unique UID from the event table row. 
        /// </summary>
        public int EventTableUID { get => (int)EventTableRow[RoundTable.COL.UID]; }

        /// <summary>
        /// Property to retrieve a non-reflective list of players.
        /// </summary>
        public List<string> Players {
            get => this.GetPlayers();
        }

        public DataRow EventTableRow { get; }

        /// <summary>
        /// Property to track if delete has been invoked.
        /// </summary>
        public bool Deleted { get; private set; } = false;
        public int Bowls {
            get => (int)this.EventTableRow[RoundTable.COL.BOWLS];
            set => this.EventTableRow[RoundTable.COL.BOWLS] = value;
        }

        public int Ends {
            get => (int)this.EventTableRow[RoundTable.COL.ENDS];
            set => this.EventTableRow[RoundTable.COL.ENDS] = value;
        }
        public int Tie {
            get => (int)this.EventTableRow[RoundTable.COL.TIE];
            set => this.EventTableRow[RoundTable.COL.TIE] = value;
        }

        internal Team(Match match, DataRow eventTableRow, int teamIndex) : base(match.League.TeamTable) {
            this.Match = match;
            this.EventTableRow = eventTableRow;
            this.TeamIndex = teamIndex;

            this.RowFilter = $"{TeamTable.COL.ROUND_UID} = {this.EventTableUID}";
        }

        /// <summary>
        /// Add a new player to this team at the next available index.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True </returns>
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
        /// Add a new player to this team at the next available index.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True </returns>
        public void AddPlayers(IEnumerable<string> names) {
            foreach (string name in names) this.AddPlayer(name);
        }

        /// <summary>
        /// Determine if this team contains the specified player.
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
            DataRowView rowView = this[rowIndex];

            if (this.Table is null) throw new NullReferenceException();
            this.Table.Rows.Remove(rowView.Row);

            return true;
        }

        /// <summary>
        /// Remove all players from this team.
        /// </summary>
        /// <returns>A list of the players removed.</returns>
        public List<string> ClearPlayers() {
            var players = this.Players;
            foreach (string player in players) {
                this.RemovePlayer(player);
            }
            return players;
        }

        /// <summary>
        /// Remove this team from the underlying dataset.
        /// </summary>
        public void Delete() {
            DeletedException.ThrowIf(this);

            foreach (string player in this.Players) this.RemovePlayer(player);
            var eventTable = this.League.RoundTable;
            eventTable.Rows.Remove(this.EventTableRow);

            this.Deleted = true;
        }

        public void MovePlayer(string player, int newTeamIndex) {
            this.League.TeamTable.RemoveRows(this.EventTableUID, player);
            this.League.IdleTable.AddRow(this.LeagueEvent.UID, this.Round.RoundIndex, player);
        }

        public string PrettyPrint() {
            if (this.Table is null) throw new NullReferenceException();
            return this.PrettyPrint() + "\n" +
                   this.EventTableRow.PrettyPrint();
        }
    }
}

