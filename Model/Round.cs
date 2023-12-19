using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model {

    public class IdlePlayers : DataView {
        private Round Round;

        internal IdlePlayers(Round round) : base(round.LeagueEvent.League.TeamTable) {
            this.Round = round;

            RowFilter = 
                $"{TeamTable.COL.EVENT_NAME} = '{round.LeagueEvent.EventName}' AND " +
                $"{TeamTable.COL.ROUND} = {round.RoundIndex} AND " +
                $"{TeamTable.COL.TEAM_IDX} = -1";

            Sort = TeamTable.COL.PLAYER_NAME;
        }

        public bool Contains(string playerName) {
            return this.Find(playerName) != -1;
        }

        public void Remove(string playerName) {
            int index = this.Find(playerName);
            if (index == -1) throw new KeyNotFoundException();
            if (this.Table is null) throw new NullReferenceException();

            this.Table.Rows.RemoveAt(index);
        }

        public void Add(string playerName) {
            if (this.Contains(playerName)) throw new ArgumentException(null, nameof(playerName));
            this.Round.LeagueEvent.League.TeamTable.AddRow(
                eventName: this.Round.LeagueEvent.EventName,
                round: this.Round.RoundIndex,
                playerName: playerName,
                teamIdx: -1
            );
        }
    }

    /// <summary>
    /// A view of EventTable restricted to event name and Round.
    /// The public methods do not directly change the data set.
    /// </summary>
    public class Round : DataView, IDeleted {
        public LeagueEvent LeagueEvent { get; }

        public int RoundIndex { get; }

        public bool Deleted { get; private set; } = false;

        public IdlePlayers IdlePlayers { get; }

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
            this.IdlePlayers = new IdlePlayers(this);

            RowFilter = $"event_name = '{this.LeagueEvent.EventName}' AND round = {roundIndex}";
        }

        public Match GetMatch(int lane) {
            DeletedException.ThrowIf(this);
            return new Match(this, lane) {
                RowFilter = $"{EventTable.COL.LANE} = {lane}"
            };
        }

        internal DataRow AddRow(int lane, int teamUID) {
            DeletedException.ThrowIf(this);
            return this.LeagueEvent.AddRow(this.RoundIndex, lane, teamUID);
        }

        /// <summary>
        /// Retrieve all matches that contain at least one player.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private List<Match> GetMatches() {
            DeletedException.ThrowIf(this);
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
            DeletedException.ThrowIf(this);

            foreach (Match match in this.Matches) {
                match.Delete();
            }
            this.LeagueEvent.RoundCount = this.LeagueEvent.RoundCount - 1;

            this.Deleted = true;
        }
        public string PrettyPrint() {
            return this.Table.PrettyPrint(this, $"Round {RoundIndex} of {LeagueEvent.EventName}");
        }

        public void DeletePlayer(string playerName) {
            throw new NotImplementedException();
        }
    }
}
