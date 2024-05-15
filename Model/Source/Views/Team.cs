using Leagueinator.Model.Tables;

namespace Leagueinator.Model.Views {
    public class Team(TeamRow teamRow) : IEquatable<Team> {
        public TeamRow TeamRow { get; } = teamRow;

        /// <summary>
        /// A read only list of players on the team.
        /// Recalculates with every call.
        /// </summary>
        public IReadOnlySet<string> Players {
            get {
                HashSet<string> players = [];
                foreach (MemberRow membersRow in this.TeamRow.Members) {
                    players.Add(membersRow.Player);
                }
                return players;
            }
        }

        public override bool Equals(object? @object) {
            if (@object is null) return false;
            else if (@object is Team that) {
                if (that.Players.Count != this.Players.Count) return false;
                foreach (string player in this.Players) {
                    if (!that.Players.Contains(player)) return false;
                }
            }
            else if (@object is TeamRow teamRow) {
                if (teamRow.Members.Count != this.Players.Count) return false;
                foreach (string player in this.Players) {
                    if (!teamRow.Members.Has(player)) return false;
                }
            }

            return true;
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
    }
}
