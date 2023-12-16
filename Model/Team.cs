using Model.Tables;
using System.Data;

namespace Model {

    /// <summary>
    /// A view of TeamTable paired with a Row from EventTable.
    /// The public methods may update the data set.
    /// </summary>
    public class Team : DataView{
        private Match Match { get; }

        public int TeamIndex { get; }

        private DataRow Row { get; }

        internal Team(Match match, DataRow row, int teamIndex) : base(match.Round.LeagueEvent.League.TeamTable) {            
            this.Match = match;
            this.Row = row;
            this.TeamIndex = teamIndex;
        }

        public bool AddPlayer(string name) {
            if (this.HasPlayer(name)) return false;

            var table = this.Match.Round.LeagueEvent.League.TeamTable;
            var row = table.NewRow();

            row[TeamTable.COL.EVENT_NAME] = this.Row[EventTable.COL.EVENT_NAME];
            row[TeamTable.COL.TEAM_IDX] = this.TeamIndex;
            row[TeamTable.COL.PLAYER_NAME] = name;
            table.Rows.Add(row);

            return true;
        }

        public bool HasPlayer(string name) {
            DataTable table = this.Table ?? throw new NullReferenceException();
            string eventName = (string)this.Row[EventTable.COL.EVENT_NAME];

            var rows = table.Select($"{TeamTable.COL.TEAM_IDX} = {this.TeamIndex}" +
                                    $" AND {TeamTable.COL.PLAYER_NAME} = '{name}'" +
                                    $" AND {TeamTable.COL.EVENT_NAME} = '{eventName}'");
            return rows.Length > 0;
        }

        public List<string> GetPlayers() {
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

    }
}
