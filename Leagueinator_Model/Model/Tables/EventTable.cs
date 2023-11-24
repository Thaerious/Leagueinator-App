using Leagueinator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator_Model.Model.Tables {
    public class EventTable {
        private DataTable source;

        public EventTable(DataTable source) {
            this.source = source;
        }

        public static DataTable MakeEventTable() {
            DataTable table = new DataTable("event");
            DataColumn column;

            column = new DataColumn {
                DataType = typeof(int),
                ColumnName = "uid",
                Unique = true,
                AutoIncrement = true
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(int),
                ColumnName = "round"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(int),
                ColumnName = "lane"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(int),
                ColumnName = "team"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(int),
                ColumnName = "bowls"
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = typeof(int),
                ColumnName = "ends"
            };
            table.Columns.Add(column);

            return table;
        }

        internal void AddRow(int round, int lane, int teamId, int bowls, int endsPlayed) {
            var eRow = this.source.NewRow();
            eRow["round"] = round;
            eRow["lane"] = lane;
            eRow["team"] = teamId;
            eRow["bowls"] = bowls;
            eRow["ends"] = endsPlayed;
            this.source.Rows.Add(eRow);
        }
    }
}
