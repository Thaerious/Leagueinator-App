using Leagueinator.Model.Tables;

namespace Leagueinator.Model.Views {
    public class Team(TeamRow teamRow) : IEquatable<Team> {
        public TeamRow Row { get; } = teamRow;

        public SortedList<Match, Match> Matches = new(Comparer<Match>.Create((x, y) => {
            if (x.Round != y.Round) return x.Round - y.Round;
            if (x.Lane != y.Lane) return x.Lane - y.Lane;
            return 0;
        }));

        public IReadOnlySet<string> Players {
            get {
                HashSet<string> players = [];
                foreach (MemberRow membersRow in this.Row.Members) {
                    players.Add(membersRow.Player);
                }
                return players;
            }
        }

        public bool Equals(Team? that) {
            if (that is null) return false;
            if (that.Players.Count != this.Players.Count) return false;
            foreach (string player in this.Players) {
                if (!that.Players.Contains(player)) return false;
            }
            return true;
        }

        public override int GetHashCode() {
            int hash = 0;
            foreach (var p in this.Players) hash += p.GetHashCode();
            return hash;
        }

        public override bool Equals(object? obj) {
            return Equals(obj as Team);
        }
    }
}
