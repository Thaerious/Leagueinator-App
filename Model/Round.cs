using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model {

    /// <summary>
    /// A view of IdleTable showing only the idle players for the specified round.
    /// </summary>
    public class IdlePlayers : DataView, IEnumerable<string> {
        public event DataRowChangeEventHandler? CollectionChanged = delegate { };

        private readonly Round Round;

        public IdleTable IdleTable { get => this.Round.League.IdleTable; }

        internal IdlePlayers(Round round) : base(round.League.IdleTable) {
            this.Round = round;

            RowFilter =
                $"{IdleTable.COL.EVENT_UID} = {round.LeagueEvent.UID} AND " +
                $"{IdleTable.COL.ROUND} = {round.RoundIndex}";

            Sort = TeamTable.COL.PLAYER_NAME;

            this.IdleTable.RowChanged += this.DataRowChangeEventHandler;
        }

        private void DataRowChangeEventHandler(object sender, DataRowChangeEventArgs e) {
            if ((int)e.Row[IdleTable.COL.EVENT_UID] != this.Round.LeagueEvent.UID) return;
            if ((int)e.Row[IdleTable.COL.ROUND] != this.Round.RoundIndex) return;
            this.CollectionChanged?.Invoke(this, e);
        }

        public bool Contains(string playerName) {
            return this.Find(playerName) != -1;
        }

        public void Remove(string playerName) {
            this.IdleTable.RemoveRows(this.Round.LeagueEvent.UID, this.Round.RoundIndex, playerName);
        }

        public void Add(string playerName) {
            if (this.Contains(playerName)) throw new ArgumentException(null, nameof(playerName));

            Debug.WriteLine("Before");
            this.IdleTable.AddRow(
                eventUID: (Int32)this.Round.LeagueEvent.UID,
                round: (Int32)this.Round.RoundIndex,
                playerName: (string)playerName
            );
            Debug.WriteLine("After");
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            foreach (DataRowView row in this) {
                yield return (string)row[TeamTable.COL.PLAYER_NAME];
            }
        }

        public void Update(string textBefore, string textAfter) {
            var row = this.IdleTable.GetRow(Round.LeagueEvent.UID, Round.RoundIndex, textBefore);
            if (row == null) throw new KeyNotFoundException(textBefore);
            row[TeamTable.COL.PLAYER_NAME] = textAfter;
        }
    }

    /// <summary>
    /// A view of RoundTable restricted to event name and Round.
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

            RowFilter = $"{RoundTable.COL.DIR_UID} = {this.LeagueEvent.UID} AND {RoundTable.COL.ROUND} = {roundIndex}";
        }

        public Match GetMatch(int lane) {
            DeletedException.ThrowIf(this);
            return new Match(this, lane);
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
                int roundIndex = (row.Field<int>(RoundTable.COL.ROUND));
                int laneIndex = (row.Field<int>(RoundTable.COL.LANE));

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
            if (this.Table is null) throw new NullReferenceException();
            return this.PrettyPrint($"Round {RoundIndex} of {LeagueEvent.EventName}");
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
