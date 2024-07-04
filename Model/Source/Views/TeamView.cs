using Leagueinator.Model.Tables;
using Leagueinator.Utility;

namespace Leagueinator.Model.Views {

    public class TeamView : IEquatable<TeamView> {
        public TeamRow TeamRow { get; }

        public TeamView(TeamRow teamRow) {
            this.TeamRow = teamRow;
        }

        public List<MatchRow> Matches {
            get {
                List<MatchRow> matches = [];

                foreach (TeamRow teamRow in this.TeamRow.Event.Teams) {
                    if (this.Equals(teamRow)) matches.Add(teamRow.Match);
                }

                return matches;
            }
        }

        /// <summary>
        /// A read only prevLanes of players on the team.
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

        public HashSet<int> PrevLanes(RoundRow skip) {
            HashSet<int> prevLanes = [];

            EventRow eventRow = this.TeamRow.Match.Round.Event;
            foreach (RoundRow roundRow in eventRow.Rounds) {
                if (roundRow.Equals(skip)) continue;
                foreach (MatchRow matchRow in roundRow.Matches) {
                    foreach (TeamRow teamRow in matchRow.Teams) {
                        foreach (MemberRow memberRow in teamRow.Members) {
                            if (this.Players.Contains(memberRow.Player)) {
                                prevLanes.Add(matchRow.Lane);
                            }
                        }
                    }
                }
            }
            return prevLanes;
        }

        /// <summary>
        /// Return true is the other team has exactly the same set of players.
        /// Can compart to both TeamView and TeamRow.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public override bool Equals(object? @object) {
            if (@object is null) return false;
            List<string> thisPlayers = this.Players.ToList();
            List<string> thatPlayers = [];

            if (@object is TeamView otherTeam) thatPlayers = otherTeam.Players.ToList();
            else if (@object is TeamRow teamRow) thatPlayers = teamRow.Members.Select(mr => mr.Player).ToList();

            if (thisPlayers.Count != thatPlayers.Count) return false;

            thisPlayers.Sort((a, b) => a.CompareTo(b));
            thatPlayers.Sort((a, b) => a.CompareTo(b));

            for (int i = 0; i < thisPlayers.Count; i++) {
                if (!thisPlayers[i].Equals(thatPlayers[i])) return false;
            }

            return true;
        }

        public bool Equals(TeamView? that) {
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
