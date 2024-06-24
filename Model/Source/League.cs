using Leagueinator.Model.Tables;
using System.Data;

namespace Leagueinator.Model {
    public class League : DataSet {
        public event EventHandler<EventArgs> LeagueUpdate = delegate { };

        public EventTable EventTable { init; get; }
        public RoundTable RoundTable { init; get; }
        public MatchTable MatchTable { init; get; }
        public TeamTable TeamTable { init; get; }
        public MemberTable MemberTable { init; get; }
        public IdleTable IdleTable { init; get; }

        public League() {
            this.EventTable = new();
            this.RoundTable = new();
            this.MatchTable = new();
            this.TeamTable = new();
            this.MemberTable = new();
            this.IdleTable = new();

            this.AddTables();
            this.AddListeners();
            this.BuildColumns();
            this.AddConstraints();
        }

        public League(League that) : this() {
            this.EventTable.ImportTable(that.EventTable);
            this.RoundTable.ImportTable(that.RoundTable);
            this.MatchTable.ImportTable(that.MatchTable);
            this.TeamTable.ImportTable(that.TeamTable);
            this.MemberTable.ImportTable(that.MemberTable);
            this.IdleTable.ImportTable(that.IdleTable);
        }

        private void AddListeners() {
            foreach (DataTable table in this.Tables) {
                table.RowChanged += (s, e) => this.LeagueUpdate.Invoke(s, e);
                table.RowDeleted += (s, e) => this.LeagueUpdate.Invoke(s, e);
            }
        }

        private void AddTables() {
            this.Tables.Add(this.EventTable);
            this.Tables.Add(this.RoundTable);
            this.Tables.Add(this.MatchTable);
            this.Tables.Add(this.TeamTable);
            this.Tables.Add(this.MemberTable);
            this.Tables.Add(this.IdleTable);
        }

        private void BuildColumns() {
            this.EventTable.BuildColumns();
            this.RoundTable.BuildColumns();
            this.MatchTable.BuildColumns();
            this.TeamTable.BuildColumns();
            this.IdleTable.BuildColumns();
            this.MemberTable.BuildColumns();
        }

        private void AddConstraints() {
            this.RoundTable.Constraints.Add(
                            new ForeignKeyConstraint(
                                "FK_Round_Event",
                                [this.EventTable.Columns[EventTable.COL.UID]!],
                                [this.RoundTable.Columns[RoundTable.COL.EVENT]!]
                            )
                        );

            this.MatchTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Match_Round",
                    [this.RoundTable.Columns[RoundTable.COL.UID]!],
                    [this.MatchTable.Columns[MatchTable.COL.ROUND]!]
                )
            );

            this.TeamTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Team_Match",
                    [this.MatchTable.Columns[MatchTable.COL.UID]!],
                    [this.TeamTable.Columns[TeamTable.COL.MATCH]!]
                )
            );

            this.MemberTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Member_Team",
                    [this.TeamTable.Columns[TeamTable.COL.MATCH]!, this.TeamTable.Columns[TeamTable.COL.INDEX]!],
                    [this.MemberTable.Columns[MemberTable.COL.MATCH]!, this.MemberTable.Columns[MemberTable.COL.INDEX]!]
                )
            );

            this.IdleTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Idle_Round",
                    [this.RoundTable.Columns[RoundTable.COL.UID]!],
                    [this.IdleTable.Columns[IdleTable.COL.ROUND]!]
                )
            );
        }

        public string PrettyPrint() {
            return this.EventTable.PrettyPrint() + "\n" +
                   this.RoundTable.PrettyPrint() + "\n" +
                   this.MatchTable.PrettyPrint() + "\n" +
                   this.IdleTable.PrettyPrint() + "\n" +
                   this.TeamTable.PrettyPrint() + "\n" +
                   this.MemberTable.PrettyPrint();
                   
                   
        }
    }
}
