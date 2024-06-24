﻿using Leagueinator.Model.Tables;
using System.Data;

namespace Leagueinator.Model.Tables {
    public class EventRow : CustomRow {
        public readonly EventBoundRounds Rounds;

        public EventRow(DataRow dataRow) : base(dataRow) {
            Rounds = new(this.League.RoundTable, this);
        }

        public int EndsDefault {
            get => (int)this[EventTable.COL.ENDS_DEFAULT];
            set => this[EventTable.COL.ENDS_DEFAULT] = value;
        }

        public int LaneCount {
            get => (int)this[EventTable.COL.LANE_COUNT];
            set => this[EventTable.COL.LANE_COUNT] = value;
        }

        public EventFormat EventFormat {
            get => (EventFormat)this[EventTable.COL.EVENT_FORMAT];
            set => this[EventTable.COL.EVENT_FORMAT] = value;
        }

        public int UID {
            get => (int)this[EventTable.COL.UID];
        }

        public static implicit operator int(EventRow eventRow) => eventRow.UID;

        public string Name {
            get => (string)this[EventTable.COL.NAME];
            set => this[EventTable.COL.NAME] = value;
        }

        public string Date {
            get => (string)this[EventTable.COL.DATE];
            set => this[EventTable.COL.DATE] = value;
        }
    }
}
