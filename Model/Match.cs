using Model.Tables;
using System;
using System.Data;
using System.Diagnostics;
using System.Numerics;

namespace Model {

    /// <summary>
    /// A view of EventTable restricted to event name, round, and match.
    /// The public methods may update the data set.
    /// </summary>
    public class Match : DataView {

        public Round Round { get; }

        public int Lane { get; }

        public List<Team> Teams {
            get => this.GetTeams();
        }

        public int Size {
            get {
                var eventTable = Round.LeagueEvent.League.EventTable;

                var computedMax = eventTable.Compute($"MAX({EventTable.COL.TEAM})", this.RowFilter);
                int lastTeamIndex = (computedMax != DBNull.Value) ? Convert.ToInt32(computedMax) : -1;

                return lastTeamIndex + 1;
            }
        }

        internal Match(Round round, int laneIndex) : base(round.Table) {
            this.Round = round;
            this.Lane = laneIndex;
        }

        /// <summary>
        /// Add a row to the EventTable.
        /// </summary>
        /// <returns>A new Team view</returns>
        /// <exception cref="Exception"></exception>
        public Team NewTeam() {
            int index = this.Size;

            this.Sort = EventTable.COL.TEAM;
            DataRowView[] rows = this.FindRows(index);
            if (rows.Length != 0) throw new Exception("Sanity Check Failed");

            this.AddRow(index);

            return GetTeam(index);
        }

        private Team GetTeam(int index) {
            this.Sort = EventTable.COL.TEAM;
            DataRowView[] rows = this.FindRows(index);

            if (rows.Length > 1) throw new Exception("Sanity Check Failed on Rows");
            if (rows.Length < 1) throw new KeyNotFoundException($"team index: {index}");

            rows = this.FindRows(index);
            DataRow row = rows[0].Row;

            return new Team(this, row, index) {
                RowFilter = $"{TeamTable.COL.EVENT_NAME} = '{this.Round.LeagueEvent.EventName}'"
            };
        }

        /// <summary>
        /// Retrieve all teams that contain at least one player.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private List<Team> GetTeams() {
            DataTable table = this.Table ?? throw new NullReferenceException();

            SortedSet<int> ids = [];
            List<Team> teams = [];

            foreach (DataRow row in table.AsEnumerable()) {
                int roundIndex = (row.Field<int>(EventTable.COL.ROUND));
                int laneIndex = (row.Field<int>(EventTable.COL.LANE));
                int teamIndex = (row.Field<int>(EventTable.COL.TEAM));

                if (roundIndex != this.Round.RoundIndex) continue;
                if (laneIndex != this.Lane) continue;
                if (ids.Contains(laneIndex)) continue;

                ids.Add(laneIndex);
                teams.Add(this.GetTeam(teamIndex));
            }

            return teams;
        }

        internal DataRow AddRow(int teamIndex, int bowls = 0, int ends = 0, int tiebreaker = 0) {
            return this.Round.AddRow(this.Lane, teamIndex, bowls, ends, tiebreaker);
        }

        public string PrettyPrint() {
            return this.Round.Table.PrettyPrint(this, $"Lane {Lane} in Round {Round.RoundIndex} of {Round.LeagueEvent.EventName}");
        }
    }
}
