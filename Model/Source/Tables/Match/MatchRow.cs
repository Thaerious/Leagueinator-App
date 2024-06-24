using Leagueinator.Model.Views;
using System.Data;

namespace Leagueinator.Model.Tables {
    public class MatchRow : CustomRow {
        public MatchRow(DataRow dataRow) : base(dataRow) {
            this.Teams = new(this.League.TeamTable, [TeamTable.COL.MATCH], [this.UID]);
        }

        public readonly RowBoundView<TeamRow> Teams;

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

        public int UID => (int)this[MatchTable.COL.UID];        

        public RoundRow Round => this.League.RoundTable.GetRow((int)this[MatchTable.COL.ROUND]);

        public EventRow Event => this.Round.Event;

        public IReadOnlyList<string> Players {
            get {
                return this.Teams
                .SelectMany(teamRow => teamRow.Members)
                .Select(memberRow => memberRow.Player)
                .ToList();
            }
        }
    }
}
