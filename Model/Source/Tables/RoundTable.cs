﻿using Leagueinator.Model.Views;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model.Tables {

    public class RoundRow : CustomRow {
        public readonly RowBoundView<IdleRow> IdlePlayers;
        public readonly RowBoundView<MatchRow> Matches;

        public RoundRow(DataRow dataRow) : base(dataRow) {
            this.IdlePlayers = new(this.League.IdleTable, [IdleTable.COL.ROUND], [this.UID]);
            this.Matches = new(this.League.MatchTable, [MatchTable.COL.ROUND], [this.UID]);
        }

        public int UID {
            get => (int)this[RoundTable.COL.UID];
        }

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

        public static implicit operator int(RoundRow roundRow) => roundRow.UID;

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this[RoundTable.COL.EVENT]);
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
            int ends = int.Parse(this.Event.Settings.Get("ends")!.Value);
            int laneCount = int.Parse(this.Event.Settings.Get("lanes")!.Value);
            int teams = int.Parse(this.Event.Settings.Get("teams")!.Value);

            for (int lane = 0; lane < laneCount; lane++) {
                if (!this.Matches.Has(lane)) {
                    this.Matches.Add(lane, ends);
                }

                MatchRow matchRow = this.Matches[lane]!;
                while (matchRow.Teams.Count < teams) matchRow.Teams.Add(matchRow.Teams.Count);
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

