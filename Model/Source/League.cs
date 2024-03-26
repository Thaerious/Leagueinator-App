using Leagueinator.Model.Tables;
using System.Data;

namespace Leagueinator.Model {
    public class League : DataSet {
        public event DataRowChangeEventHandler RowChanged = delegate { };

        public EventTable EventTable { init; get; }
        public RoundTable RoundTable { init; get; }
        public MatchTable MatchTable { init; get; }
        public TeamTable TeamTable { init; get; }
        public PlayerTable PlayerTable { init; get; }
        public SettingsTable SettingsTable { init; get; }
        public MemberTable MemberTable { init; get; }
        public IdleTable IdleTable { init; get; }

        public League() {
            this.PlayerTable = new();
            this.Tables.Add(this.PlayerTable);

            this.EventTable = new();
            this.Tables.Add(this.EventTable);

            this.RoundTable = new();
            this.Tables.Add(this.RoundTable);

            this.MatchTable = new();
            this.Tables.Add(this.MatchTable);

            this.TeamTable = new();
            this.Tables.Add(this.TeamTable);

            this.SettingsTable = new();
            this.Tables.Add(this.SettingsTable);

            this.MemberTable = new();
            this.Tables.Add(this.MemberTable);

            this.IdleTable = new();
            this.Tables.Add(this.IdleTable);

            this.PlayerTable.BuildColumns();
            this.EventTable.BuildColumns();
            this.RoundTable.BuildColumns();
            this.MatchTable.BuildColumns();
            this.TeamTable.BuildColumns();
            this.SettingsTable.BuildColumns();
            this.IdleTable.BuildColumns();
            this.MemberTable.BuildColumns();

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

            this.MemberTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Member_Player",
                    [this.PlayerTable.Columns[PlayerTable.COL.NAME]!],
                    [this.MemberTable.Columns[MemberTable.COL.PLAYER]!]
                )
            );

            this.IdleTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Idle_Player",
                    [this.PlayerTable.Columns[PlayerTable.COL.NAME]!],
                    [this.IdleTable.Columns[IdleTable.COL.PLAYER]!]
                )
            );

            this.IdleTable.Constraints.Add(
                new ForeignKeyConstraint(
                    "FK_Idle_Round",
                    [this.RoundTable.Columns[RoundTable.COL.UID]!],
                    [this.IdleTable.Columns[IdleTable.COL.ROUND]!]
                )
            );

            this.SettingsTable.Constraints.Add(
            new ForeignKeyConstraint(
                "FK_Settings_Events",
                [this.EventTable.Columns[EventTable.COL.UID]!],
                [this.SettingsTable.Columns[SettingsTable.COL.EVENT]!]
            )
);
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
