using Model.Tables;
using System.Collections.ObjectModel;
using System.Data;

namespace Model {
    public class EventSettings {
        public LeagueEvent LeagueEvent { get; }

        public string this[string key] {
            get {
                return this.LeagueEvent.League.EventSettings.GetValue(LeagueEvent.UID, key);
            }
            set {
                this.LeagueEvent.League.EventSettings.SetValue(LeagueEvent.UID, key, value);
            }
        }

        public EventSettings(LeagueEvent leagueEvent) {
            this.LeagueEvent = leagueEvent;
        }

        
    }


    /// <summary>
    /// A view of RoundTable restricted to event.
    /// The public methods do not directly change the data set.
    /// </summary>
    public class LeagueEvent : DataView, IDeleted {
        internal DataRow DirectoryRow { get; }
        public string EventName { get => (string)this.DirectoryRow[EventDirectoryTable.COL.EVENT_NAME]; }

        public string EventDate { get => (string)this.DirectoryRow[EventDirectoryTable.COL.DATE]; }

        public League League { get; }

        public bool Deleted { get; private set; } = false;

        public EventSettings Settings { get; }

        public ReadOnlyCollection<Round> Rounds {
            get => new(this.GetRounds());
        }

        /// <summary>
        /// The primary key form the event directory table.
        /// </summary>
        public int UID { get => (int)DirectoryRow[EventDirectoryTable.COL.UID]; }

        internal LeagueEvent(League league, int uid) : base(league.RoundTable) {
            this.DirectoryRow = league.EventDirectoryTable.GetRow(uid);            
            this.League = league;
            this.Settings = new EventSettings(this);
            this.RowFilter = $"{RoundTable.COL.DIR_UID} = {this.UID}";
        }

        internal DataRow AddRow(int round, int lane, int teamIDX) {
            return this.League.RoundTable.AddRow(
                eventUID: this.UID,
                round: round,
                lane: lane,
                teamIdx: teamIDX
            );
        }

        private List<Round> GetRounds() {
            DeletedException.ThrowIf(this);

            DataTable table = this.Table ?? throw new NullReferenceException();
            List<Round> rounds = [];

            for (int i = 0; i < this.RoundCount; i++) {
                rounds.Add(this.GetOrCreateRound(i));
            }

            return rounds;
        }

        private Round GetOrCreateRound(int roundIndex) {
            DeletedException.ThrowIf(this);
            var round = new Round(this, roundIndex);

            return round;
        }

        /// <summary>
        /// Create a new Round and add it to the Round directory.
        /// </summary>
        /// <returns></returns>
        public Round NewRound() {
            int nextIndex = RoundCount;
            this.RoundCount = nextIndex + 1;
            return GetOrCreateRound(nextIndex);
        }

        public int RoundCount {
            get {
                DeletedException.ThrowIf(this);
                return (int)this.DirectoryRow[EventDirectoryTable.COL.ROUND_COUNT];
            }
            internal set {
                this.DirectoryRow[EventDirectoryTable.COL.ROUND_COUNT] = value;
            }
        }

        public string PrettyPrint() {
            if (this.Table is null) throw new NullReferenceException("Table");
            return this.PrettyPrint(this.EventName) + "\n" +
                   this.DirectoryRow.PrettyPrint();
        }

        public void Delete() {
            DeletedException.ThrowIf(this);
            foreach (Round round in this.Rounds) round.Delete();
            League.EventDirectoryTable.Rows.Remove(this.DirectoryRow);
            this.Deleted = true;
        }
    }
}
