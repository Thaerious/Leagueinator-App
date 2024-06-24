using Model.Source.Tables.Team;
using System.Data;

namespace Leagueinator.Model.Tables {
    public class TeamRow : CustomRow {
        public TeamRow(DataRow dataRow) : base(dataRow) {
            this.Members = new(this);
        }

        public readonly TeamBoundMembers Members;

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
