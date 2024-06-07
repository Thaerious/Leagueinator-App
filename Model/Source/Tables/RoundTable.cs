﻿using Leagueinator.Model.Views;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model.Tables {

    public class RoundRow : CustomRow {
        public RoundRow(DataRow dataRow) : base(dataRow) {
            this.IdlePlayers = new(this.League.IdleTable, [IdleTable.COL.ROUND], [this.UID]);
            this.Matches = new(this.League.MatchTable, [MatchTable.COL.ROUND], [this.UID]);
        }

        public readonly RowBoundView<IdleRow> IdlePlayers;
        public readonly RowBoundView<MatchRow> Matches;

        public int UID => (int)this[RoundTable.COL.UID];

        public EventRow Event => this.League.EventTable.GetRow((int)this[RoundTable.COL.EVENT]);


        public IReadOnlyList<string> AllPlayers {
            get {
                List<string> allPlayers = [];
                foreach (IdleRow idleRow in this.IdlePlayers) {
                    allPlayers.Add(idleRow.Player);
                }

                allPlayers.AddRange(this.Matches
                    .SelectMany(matchRow => matchRow.Teams)
                    .SelectMany(teamRow => teamRow.Members)
                    .Select(memberRow => memberRow.Player)
                    .ToList()
                );

                return allPlayers;
            }
        }

        /// <summary>
        /// Retreive a collection of team rows for all matches in this round.
        /// </summary>
        public IEnumerable<TeamRow> Teams {
            get {
                return this.Matches.SelectMany(matchRow => matchRow.Teams);
            }
        }

        public IEnumerable<MemberRow> Members {
            get {
                return this.Teams.SelectMany(teamRow => teamRow.Members);
            }
        }

        /// <summary>
        /// Add empty matches to this round so that there are as many matches are laneCount matches.
        /// </summary>
        /// <param name="laneCount"></param>
        /// <returns></returns>
        public RoundRow PopulateMatches() {
            int ends = this.Event.EndsDefault;
            int laneCount = this.Event.LaneCount;

            for (int lane = 0; lane < laneCount; lane++) {
                if (!this.Matches.Has(lane)) {
                    this.Matches.Add(lane, ends);
                }
            }
            return this;
        }

        /// <summary>
        /// Return a 1-indexed value for the next lane.
        /// The next lane is one larger than the largest lane value.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private int NextLane() {
            int next = 0;
            foreach (MatchRow matchRow in this.Matches) {
                if (next <= matchRow.Lane) next = matchRow.Lane + 1;
            }

            return next;
        }

        /// <summary>
        /// Removes all instances of the specified player's name from the IdlePlayers collection
        /// for this round.
        /// </summary>
        /// <param name="name">The name of the player to be removed from the idle table.</param>
        public void RemoveNameFromIdle(string name) {
            foreach (IdleRow idleRow in this.IdlePlayers) {
                if (idleRow.Player == name) idleRow.Remove();
            }
        }

        /// <summary>
        /// Removes all members with the specified player's name from the Members collection for
        /// for any match/team in this round.
        /// </summary>
        /// <param name="name">The name of the player to be removed from the member table.</param>
        public void RemoveNameFromTeams(string name) {
            foreach (TeamRow teamRow in this.Teams) {
                foreach (MemberRow memberRow in teamRow.Members) {
                    if (memberRow.Player == name) memberRow.Remove();
                }
            }
        }
    }

    public class RoundTable : LeagueTable<RoundRow> {
        public RoundTable() : base("rounds"){
            this.NewInstance = dataRow => new RoundRow(dataRow);
            GetInstance = args => this.GetRow((int)args[0]);
            HasInstance = args => this.HasRow((int)args[0]);
            AddInstance = args => this.AddRow((int)args[0]);
        }

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
        }

        public RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(foundRows[0]);
        }

        public bool HasRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            return foundRows.Length > 0;
        }

        public RoundRow AddRow(int eventUID) {
            var row = this.NewRow();
            row[COL.EVENT] = eventUID;
            this.Rows.Add(row);
            return new(row);
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT,
                Unique = false
            });
        }
    }
}

