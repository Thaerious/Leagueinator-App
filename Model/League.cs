using Model.Tables;
using System.Data;

namespace Model {
    public class League : DataSet {
        public event DataRowChangeEventHandler RowChanged = delegate { };

        public EventsTable EventTable { init; get; }
        public RoundTable RoundsTable { init; get; }
        public MatchTable MatchTable { init; get; }
        public TeamTable TeamTable { init; get; }
        public PlayersTable PlayersTable { init; get; }
        public SettingsTable SettingsTable { init; get; }
        public MembersTable MembersTable { init; get; }
        public IdleTable IdleTable { init; get; }

        public League() {
            this.PlayersTable = new(this);
            this.Tables.Add(this.PlayersTable);

            this.EventTable = new(this);
            this.Tables.Add(this.EventTable);

            this.RoundsTable = new(this);
            this.Tables.Add(this.RoundsTable);

            this.MatchTable = new(this);
            this.Tables.Add(this.MatchTable);

            this.TeamTable = new(this);
            this.Tables.Add(this.TeamTable);

            this.SettingsTable = new(this);
            this.Tables.Add(this.SettingsTable);

            this.MembersTable = new(this);
            this.Tables.Add(this.MembersTable);

            this.IdleTable = new(this);
            this.Tables.Add(this.IdleTable);

            this.PlayersTable.BuildColumns();
            this.EventTable.BuildColumns();
            this.RoundsTable.BuildColumns();
            this.MatchTable.BuildColumns();
            this.TeamTable.BuildColumns();
            this.SettingsTable.BuildColumns();
            this.IdleTable.BuildColumns();
            this.MembersTable.BuildColumns();

            foreach (DataTable table in this.Tables) {
                table.RowChanged += (s, e) => {
                    this.RowChanged.Invoke(s, e);
                };
            }
        }

        public string PrettyPrint() {
            return this.MatchTable.PrettyPrint() + "\n" +
                this.TeamTable.PrettyPrint() + "\n" +
                this.IdleTable.PrettyPrint() + "\n" +
                this.EventTable.PrettyPrint() + "\n" +
                this.SettingsTable.PrettyPrint();
        }
    }
}
