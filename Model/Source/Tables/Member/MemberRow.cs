using System.Data;

namespace Leagueinator.Model.Tables {
    public class MemberRow(DataRow dataRow) : CustomRow(dataRow) {
        
        /// <summary>
        /// Set the player name.
        /// Will remove name from the idle table and other teams.
        /// </summary>
        public string Player {
            get => (string)this[MemberTable.COL.PLAYER];
            set => this[MemberTable.COL.PLAYER] = value;
        }

        public int Index => (int)this[MemberTable.COL.INDEX];

        public TeamRow Team => this.League.TeamTable.GetRow((int)this[MemberTable.COL.MATCH], (int)this[MemberTable.COL.INDEX]);

        public MatchRow Match => this.Team.Match;

        public RoundRow Round => this.Match.Round;

        public EventRow Event => this.Round.Event;        
    }
}
