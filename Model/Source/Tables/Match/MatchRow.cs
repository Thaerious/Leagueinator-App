using System.Data;

namespace Leagueinator.Model.Tables {
    public class MatchRow : CustomRow {
        public readonly MatchBoundTeams Teams;

        internal MatchRow(DataRow dataRow) : base(dataRow) {
            this.Teams = new(this);
        }

        public int Lane {
            get => (int)this[MatchTable.COL.LANE];
            set => this[MatchTable.COL.LANE] = value;
        }

        public int Ends {
            get => (int)this[MatchTable.COL.ENDS];
            set => this[MatchTable.COL.ENDS] = value;
        }

        public MatchFormat MatchFormat {
            get => (MatchFormat)this[MatchTable.COL.FORMAT];
            set => this[MatchTable.COL.FORMAT] = value;
        }

        internal int UID => (int)this[MatchTable.COL.UID];

        public RoundRow Round => this.League.RoundTable.GetRow((int)this[MatchTable.COL.ROUND]);

        public EventRow Event => this.Round.Event;

        public IReadOnlyList<MemberRow> Members {
            get {
                return this.Teams
                .SelectMany(teamRow => teamRow.Members)
                .ToList();
            }
        }
    }
}
