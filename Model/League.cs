using Model.Tables;
using System.Data;
using Leagueinator.Utility;

namespace Model {
    public class League : DataSet {
        public event DataRowChangeEventHandler RowChanged = delegate { };

        public EventsTable EventTable { init; get; }

        public RoundTable RoundTable { init; get; }

        public IdleTable IdleTable { init; get; }

        public MatchTable MatchTable { init; get; }
        public TeamTable TeamTable { init; get; }

        public PlayerTable PlayerTable { init; get; }

        public SettingsTable SettingsTable { init; get; }

        public League() {
            EventTable = new(this);
            RoundTable = new(this);
            MatchTable = new(this);
            TeamTable = new(this);
            IdleTable = new(this);
            PlayerTable = new(this);
            SettingsTable = new(this);

            Tables.Add(EventTable);
            Tables.Add(RoundTable);
            Tables.Add(MatchTable);
            Tables.Add(TeamTable);            
            Tables.Add(IdleTable);
            Tables.Add(PlayerTable);
            Tables.Add(SettingsTable);

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
