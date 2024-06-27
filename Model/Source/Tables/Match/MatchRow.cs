using System.Data;

namespace Leagueinator.Model.Tables {
    public class MatchRow : CustomRow {      

        internal MatchRow(DataRow dataRow) : base(dataRow) {
            this.Teams = new(this);
        }

        public readonly MatchBoundTeams Teams;

        public IEnumerable<MemberRow> Members => this.Teams.SelectMany(x => x.Members);

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
