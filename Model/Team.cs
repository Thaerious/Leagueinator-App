using System.Data;

namespace Model.Tables {
    public class Team {
        private Match Match { get; }

        public int TeamIndex { get; }

        internal Team(Match match, int matchIndex) {
            this.Match = match;
            this.TeamIndex = matchIndex;
        }

        public bool AddPlayer(string name) {
            if (this.HasPlayer(name)) return false;

            var table = this.Match.Round.LeagueEvent.League.TeamTable;
            var row = table.NewRow();

            row[TeamTable.TEAM_ID_COL] = this.TeamIndex;
            row[TeamTable.NAME_COL] = name;
            table.Rows.Add(row);

            return true;
        }

        public bool HasPlayer(string name) {
            var table = this.Match.Round.LeagueEvent.League.TeamTable;
            var rows = table.Select($"{TeamTable.TEAM_ID_COL} = {this.TeamIndex} AND {TeamTable.NAME_COL} = '{name}'");
            return rows.Length > 0;
        }

        public List<Player> GetPlayers(int team) {
            List<Player> list = [];
            var table = this.Match.Round.LeagueEvent.League.TeamTable;

            var view = new DataView(table) {
                RowFilter = $"team = ${this.TeamIndex}"
            };

            foreach (DataRowView row in view) {
                list.Add(new Player(row));
            }

            return list;
        }

    }
}
