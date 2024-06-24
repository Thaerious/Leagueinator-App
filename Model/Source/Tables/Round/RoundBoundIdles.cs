﻿using Leagueinator.Model.Tables;
using System.Data;

namespace Model.Source.Tables.Round {
    public class RoundBoundIdles(RoundRow roundRow): DataView, IEnumerable<IdleRow> {

        private IdleTable ChildTable { get; } = roundRow.League.IdleTable;

        public IdleRow Add(string name) => this.ChildTable.AddRow(roundRow.UID, name);

        public IdleRow Get(string name) => this.ChildTable.GetRow(roundRow.UID, name);

        public bool Has(string name) => this.ChildTable.HasRow(roundRow.UID, name);

        public new IdleRow? this[int index] {
            get => new(base[index].Row);
        }

        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<IdleRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }
    }
}
