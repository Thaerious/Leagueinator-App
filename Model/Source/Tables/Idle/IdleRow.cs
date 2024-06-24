using System.Data;

namespace Leagueinator.Model.Tables {
    public class IdleRow(DataRow dataRow) : CustomRow(dataRow) {

        public string Player {
            get => (string)this[IdleTable.COL.PLAYER];
        }

        public RoundRow Round {
            get => this.League.RoundTable.GetRow((int)this[IdleTable.COL.ROUND]);
        }

        public EventRow Event {
            get => this.Round.Event;
        }
    }
}
