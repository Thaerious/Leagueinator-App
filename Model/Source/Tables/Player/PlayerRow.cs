using System.Data;

namespace Leagueinator.Model.Tables {
    public class PlayerRow(DataRow dataRow) : CustomRow(dataRow) {
        public string Name {
            get => (string)this[PlayerTable.COL.NAME];
            set => this[PlayerTable.COL.NAME] = value;
        }

        public static implicit operator string(PlayerRow playerRow) => playerRow.Name;
    }
}
