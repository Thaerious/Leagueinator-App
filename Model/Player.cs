using Model.Tables;
using System.Data;

namespace Model {
    public class Player {

        private DataRow DataRow { get; }

        internal Player(DataRowView dataRow) {
            this.DataRow = dataRow.Row;
        }

        public string PrettyPrint() {
            return this.DataRow.Table.PrettyPrint(this.DataRow, "Player");
        }
    }
}
