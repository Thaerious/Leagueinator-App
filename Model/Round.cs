using Model.Tables;
using System.Data;

namespace Model {
    /// <summary>
    /// A view of EventTable restricted to event name and round.
    /// The public methods do not directly change the data set.
    /// </summary>
    public class Round : DataView {
        public LeagueEvent LeagueEvent { get; }

        public int RoundIndex { get; }

        /// <summary>
        /// Retrieve all matches that contain at least one player.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<Match> Matches {
            get => this.GetMatches();
        }

        internal Round(LeagueEvent lEvent, int roundIndex) : base(lEvent.Table) {
            this.LeagueEvent = lEvent;
            this.RoundIndex = roundIndex;
        }

        public Match GetMatch(int lane) {
            return new Match(this, lane) {
                RowFilter = $"{EventTable.COL.LANE} = {lane}"
            };
        }

        internal DataRow AddRow(int lane, int teamUID) {
            return this.LeagueEvent.AddRow(this.RoundIndex, lane, teamUID);
        }

        /// <summary>
        /// Retrieve all matches that contain at least one player.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private List<Match> GetMatches() {
            DataTable table = this.Table ?? throw new NullReferenceException();

            SortedSet<int> ids = [];
            List<Match> matches = [];

            foreach (DataRow row in table.AsEnumerable()) {
                int roundIndex = (row.Field<int>(EventTable.COL.ROUND));
                int laneIndex = (row.Field<int>(EventTable.COL.LANE));

                if (roundIndex != this.RoundIndex) continue;
                if (ids.Contains(laneIndex)) continue;

                ids.Add(laneIndex);
                matches.Add(this.GetMatch(laneIndex));
            }

            return matches;
        }

        public void Delete() {
            foreach (Match match in this.Matches) {
                match.Delete();
            }
        }
        public string PrettyPrint() {
            return this.Table.PrettyPrint(this, $"Round {RoundIndex} of {LeagueEvent.EventName}");
        }
    }
}
