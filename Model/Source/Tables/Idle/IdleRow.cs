using System.Data;

namespace Leagueinator.Model.Tables {
    public class IdleRow : CustomRow {

        internal IdleRow(DataRow dataRow) : base(dataRow) {}

        public string Player {
            get => (string)this[IdleTable.COL.PLAYER];
            set => this[IdleTable.COL.PLAYER] = value;
        }

        public RoundRow Round {
            get => this.League.RoundTable.GetRow((int)this[IdleTable.COL.ROUND]);
        }

        public EventRow Event {
            get => this.Round.Event;
        }
    }
}
