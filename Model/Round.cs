using Model.Tables;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Model {

    /// <summary>
    /// A view of IdleTable showing only the idle players for the specified round.
    /// </summary>
    public class IdlePlayers : DataView, IEnumerable<string> {
        private readonly Round Round;

        public IdleTable IdleTable { get => this.Round.League.IdleTable; }

        internal IdlePlayers(Round round) : base(round.League.IdleTable) {
            this.Round = round;

            RowFilter =
                $"{IdleTable.COL.EVENT_NAME} = '{round.LeagueEvent.EventName}' AND " +
                $"{IdleTable.COL.ROUND} = {round.RoundIndex}";

            Sort = TeamTable.COL.PLAYER_NAME;
        }

        public bool Contains(string playerName) {
            return this.Find(playerName) != -1;
        }

        public void Remove(string playerName) {
            this.IdleTable.RemoveRows(this.Round.LeagueEvent.EventName, this.Round.RoundIndex, playerName);
        }

        public void Add(string playerName) {
            if (this.Contains(playerName)) throw new ArgumentException(null, nameof(playerName));
            
            this.IdleTable.AddRow(
                eventName: this.Round.LeagueEvent.EventName,
                round: this.Round.RoundIndex,
                playerName: playerName
            );
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            foreach (DataRowView row in this) {
                yield return (string)row[TeamTable.COL.PLAYER_NAME];
            }
        }
    }


    /// <summary>
    /// A view of EventTable restricted to event name and Round.
    /// The public methods do not directly change the data set.
    /// </summary>
    public class Round : DataView, IDeleted {

        public League League { get => this.LeagueEvent.League; }

        public LeagueEvent LeagueEvent { get; }

        public int RoundIndex { get; }

        public bool Deleted { get; private set; } = false;

        public ICollection<string> Players {
            get {
                List<string> list = [];
                foreach (Match match in this.Matches) {
                    list.AddRange(match.Players);
                }
                return list;
            }
        }

        public IdlePlayers IdlePlayers { get; }

        public ICollection<string> AllPlayers {
            get {
                List<string> list = [];
                list.AddRange(Players);
                list.AddRange(IdlePlayers);
                return list;
            }
        }

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

        /// <summary>
        /// Remove all players from all matches and add them to idle players.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void ResetPlayers() {
            foreach (Match match in this.Matches) {
                foreach (Team team in match.Teams) {
                    foreach (string Player in team.Players) {
                        team.RemovePlayer(Player);
                        this.IdlePlayers.Add(Player);
                    }
                }
            }

        }
    }
}
