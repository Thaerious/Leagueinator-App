using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model {
    /// <summary>
    /// A view of IdleTable showing only the idle players for the specified round.
    /// </summary>
    public class IdlePlayers : DataView, IEnumerable<string> {
        public event DataRowChangeEventHandler? CollectionChanged = delegate { };

        private readonly Round Round;

        public IdleTable IdleTable { get => this.Round.League.IdleTable; }

        internal IdlePlayers(Round round) : base(round.League.IdleTable) {
            this.Round = round;

            RowFilter =
                $"{IdleTable.COL.EVENT_UID} = {round.LeagueEvent.UID} AND " +
                $"{IdleTable.COL.ROUND} = {round.RoundIndex}";

            Sort = TeamTable.COL.PLAYER_NAME;

            this.IdleTable.RowChanged += this.DataRowChangeEventHandler;
        }

        private void DataRowChangeEventHandler(object sender, DataRowChangeEventArgs e) {
            if ((int)e.Row[IdleTable.COL.EVENT_UID] != this.Round.LeagueEvent.UID) return;
            if ((int)e.Row[IdleTable.COL.ROUND] != this.Round.RoundIndex) return;
            this.CollectionChanged?.Invoke(this, e);
        }

        public bool Contains(string playerName) {
            return this.Find(playerName) != -1;
        }

        public void Remove(string playerName) {
            this.IdleTable.RemoveRows(this.Round.LeagueEvent.UID, this.Round.RoundIndex, playerName);
        }

        public void Add(string playerName) {
            if (this.Contains(playerName)) throw new ArgumentException(null, nameof(playerName));

            Debug.WriteLine("Before");
            this.IdleTable.AddRow(
                eventUID: (Int32)this.Round.LeagueEvent.UID,
                round: (Int32)this.Round.RoundIndex,
                playerName: (string)playerName
            );
            Debug.WriteLine("After");
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            foreach (DataRowView row in this) {
                yield return (string)row[TeamTable.COL.PLAYER_NAME];
            }
        }

        public void Update(string textBefore, string textAfter) {
            var row = this.IdleTable.GetRow(Round.LeagueEvent.UID, Round.RoundIndex, textBefore);
            if (row == null) throw new KeyNotFoundException(textBefore);
            row[TeamTable.COL.PLAYER_NAME] = textAfter;
        }
    }
}
