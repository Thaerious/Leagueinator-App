using Leagueinator.Model.Views;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model.Tables {
    public class EventRow : CustomRow {

        internal EventRow(DataRow dataRow) : base(dataRow) {
            Rounds = new(this);
        }

        public readonly EventBoundRounds Rounds;

        public IEnumerable<IdleRow> IdlePlayers => this.Rounds.SelectMany(x => x.IdlePlayers);

        public IEnumerable<MatchRow> Matches => this.Rounds.SelectMany(x => x.Matches);

        public IEnumerable<TeamRow> Teams => this.Matches.SelectMany(x => x.Teams);

        public IEnumerable<MemberRow> Members => this.Teams.SelectMany(x => x.Members);

        public int EndsDefault {
            get => (int)this[EventTable.COL.ENDS_DEFAULT];
            set {
                if (value < 1) throw new ArgumentOutOfRangeException(nameof(EndsDefault), "Default ends cannot be less than 0.");
                this[EventTable.COL.ENDS_DEFAULT] = value;
            }
        }

        public int LaneCount {
            get => (int)this[EventTable.COL.LANE_COUNT];
            set {
                if (value < 1) throw new ArgumentOutOfRangeException(nameof(LaneCount), "Lane count cannot be less than 0.");
                this[EventTable.COL.LANE_COUNT] = value;
            }
        }

        public EventFormat EventFormat {
            get => (EventFormat)this[EventTable.COL.EVENT_FORMAT];
            set => this[EventTable.COL.EVENT_FORMAT] = value;
        }

        internal int UID {
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

        public IReadOnlyList<TeamView> TeamViews() {
            HashSet<TeamView> teamViews = [];

            foreach (TeamRow teamRow in this.Teams) {
                if (teamRow.Members.Count <= 0) continue;
                teamViews.Add(new(teamRow));
            }

            return [.. teamViews];
        }
    }
}
