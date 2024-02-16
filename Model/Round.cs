using Model.Tables;
using System.Data;

namespace Model {
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
