using Leagueinator.Model.Views;
using System.Data;

namespace Leagueinator.Model.Tables {

    public class RoundRow : CustomRow {
        public readonly RowBoundView<IdleRow> IdlePlayers;
        public readonly RowBoundView<MatchRow> Matches;

        public RoundRow(DataRow dataRow) : base(dataRow) {
            this.IdlePlayers = new(this.League.IdleTable, [IdleTable.COL.ROUND], [this.UID]);
            this.Matches = new(this.League.MatchTable, [MatchTable.COL.ROUND], [this.UID]);
        }

        public int UID {
            get => (int)this.DataRow[RoundTable.COL.UID];
        }

        public static implicit operator int(RoundRow roundRow) => roundRow.UID;

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this.DataRow[RoundTable.COL.EVENT]);
        }

        /// <summary>
        /// Retreive a colloection of team rows for all matches in this round.
        /// </summary>
        public IEnumerable<TeamRow> Teams {
            get {
                return this.Matches.SelectMany(matchRow => matchRow.Teams);
            }
        }

        public IEnumerable<MemberRow> Members {
            get {
                return this.Teams.SelectMany(teamRow => teamRow.Members);
            }
        }

        public RoundRow PopulateMatches() {
            int matchCount = int.Parse(this.Event.Settings["match_count"] ?? "8");

            while (this.Matches.Count < matchCount) {
                int ends = int.Parse(this.Event.Settings["ends"] ?? "10");
                this.Matches.Add(this.NextLane(), ends);
            }

            return this;
        }

        /// <summary>
        /// Return a 1-indexed value for the next lane.
        /// The next lane is one larger than the largest lane value.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private int NextLane() {
            int next = 0;
            foreach (MatchRow matchRow in this.Matches) {
                if (next <= matchRow.Lane) next = matchRow.Lane + 1;
            }

            return next;
        }
    }

    public class RoundTable() : LeagueTable<RoundRow>("rounds") {
        public IEnumerable<RoundRow> Rounds {
            get {
                return this.AsEnumerable().Select(row => new RoundRow(row));
            }
        }

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
        }

        public RoundRow AddRow(int eventUID) {
            var row = this.NewRow();
            row[COL.EVENT] = eventUID;
            this.Rows.Add(row);
            return new(row);
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT,
                Unique = false
            });
        }

        internal RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(foundRows[0]);
        }
    }
}

