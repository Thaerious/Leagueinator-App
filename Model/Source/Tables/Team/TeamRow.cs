using Leagueinator.Model.Tables.Event;
using Leagueinator.Model.Tables.Match;
using Leagueinator.Model.Tables.Member;
using Leagueinator.Model.Tables.Round;
using Leagueinator.Model.Views;
using System.Data;

namespace Leagueinator.Model.Tables.Team {
    public class TeamRow : CustomRow {
        public TeamRow(DataRow dataRow) : base(dataRow) {
            this.Members = new(
                this.League.MemberTable,
                [TeamTable.COL.MATCH, TeamTable.COL.INDEX],
                [this.Match.UID, this.Index]
            );
        }

        public readonly RowBoundView<MemberRow> Members;

        public int Bowls {
            get => (int)this[TeamTable.COL.BOWLS];
            set => this[TeamTable.COL.BOWLS] = value;
        }

        public int Tie {
            get => (int)this[TeamTable.COL.TIE];
            set => this[TeamTable.COL.TIE] = value;
        }

        public int Index => (int)this[TeamTable.COL.INDEX];

        public MatchRow Match => this.League.MatchTable.GetRow((int)this[TeamTable.COL.MATCH]);

        public RoundRow Round => this.Match.Round;

        public EventRow Event => this.Round.Event;     
    }
}
