using Model.Tables;
using System.Data;
using Leagueinator.Utility;

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
            PlayersTable  = new(this);
            this.Tables.Add(PlayersTable);

            EventTable    = new(this);
            this.Tables.Add(EventTable);

            RoundsTable   = new(this);
            this.Tables.Add(RoundsTable);

            MatchTable    = new(this);
            this.Tables.Add(MatchTable);

            TeamTable     = new(this);
            this.Tables.Add(TeamTable);

            SettingsTable = new(this);
            this.Tables.Add(SettingsTable);

            MembersTable  = new(this);
            this.Tables.Add(MembersTable);

            IdleTable     = new(this);
            this.Tables.Add(IdleTable);

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
            return MatchTable.PrettyPrint() + "\n" +
                TeamTable.PrettyPrint() + "\n" +
                IdleTable.PrettyPrint() + "\n" +
                EventTable.PrettyPrint() + "\n" +
                SettingsTable.PrettyPrint();
        }
    }
}
