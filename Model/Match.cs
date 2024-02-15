using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model {

    /// <summary>
    /// A view of RoundTable restricted to event name, Round, and match.
    /// </summary>
    public class Match : DataView, IDeleted {
        public ICollection<string> Players {
            get {
                List<string> list = [];
                foreach (Team team in this.Teams) {
                    list.AddRange(team.Players);
                }
                return list;
            }
        }

        public League League { get => this.LeagueEvent.League; }

        public LeagueEvent LeagueEvent { get => this.Round.LeagueEvent; }

        public Round Round { get; }

        public int Lane { get; }

        public bool Deleted { get; private set; } = false;

        public List<Team> Teams {
            get => this.GetTeams();
        }

        public int Size {
            get {
                DeletedException.ThrowIf(this);
                var eventTable = Round.LeagueEvent.League.RoundTable;

                var computedMax = eventTable.Compute($"MAX({RoundTable.COL.TEAM_IDX})", this.RowFilter);
                int lastTeamIndex = (computedMax != DBNull.Value) ? Convert.ToInt32(computedMax) : -1;

                return lastTeamIndex + 1;
            }
        }

        internal Match(Round round, int lane) : base(round.Table) {
            this.Round = round;
            this.Lane = lane;
            this.RowFilter = $"{RoundTable.COL.ROUND} = {round.RoundIndex} AND {RoundTable.COL.LANE} = {lane} ";            
        }

        /// <summary>
        /// Add a row to the RoundTable to represent a team in this match.
        /// </summary>
        /// <returns>A new Team view</returns>
        /// <exception cref="Exception"></exception>
        public Team NewTeam() {
            DeletedException.ThrowIf(this);
            int index = this.Size;

            this.Sort = RoundTable.COL.TEAM_IDX;
            DataRowView[] rows = this.FindRows(index);
            if (rows.Length != 0) throw new Exception("Sanity Check Failed");

            this.League.RoundTable.AddRow(
                eventUID: this.LeagueEvent.UID,
                round: this.Round.RoundIndex,
                lane: this.Lane,
                teamIdx: index
            );

            return GetTeam(index);
        }

        private Team GetTeam(int index) {
            this.Sort = RoundTable.COL.TEAM_IDX;
            DataRowView[] rows = this.FindRows(index);

            if (rows.Length > 1) throw new Exception("Sanity Check Failed on Rows");
            if (rows.Length < 1) throw new KeyNotFoundException($"team index: {index}");

            rows = this.FindRows(index);
            DataRow row = rows[0].Row;

            return new Team(this, row, index);
        }

        /// <summary>
        /// Retrieve all teams.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private List<Team> GetTeams() {
            DeletedException.ThrowIf(this);
            DataTable eventTable = this.Table ?? throw new NullReferenceException();

            SortedSet<int> ids = [];
            List<Team> teams = [];

            foreach (DataRow row in eventTable.AsEnumerable()) {
                int roundIndex = (row.Field<int>(RoundTable.COL.ROUND));
                int laneIndex = (row.Field<int>(RoundTable.COL.LANE));
                int teamIndex = (row.Field<int>(RoundTable.COL.TEAM_IDX));

                if (roundIndex != this.Round.RoundIndex) continue;
                if (laneIndex != this.Lane) continue;
                if (ids.Contains(teamIndex)) continue;

                ids.Add(teamIndex);
                teams.Add(this.GetTeam(teamIndex));
            }

            return teams;
        }

        public void Delete() {
            DeletedException.ThrowIf(this);
            foreach (Team team in this.Teams) team.Delete();
            this.Deleted = true;
        }
    }
}
