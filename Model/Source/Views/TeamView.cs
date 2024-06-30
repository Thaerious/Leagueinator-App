using Leagueinator.Model.Tables;

namespace Leagueinator.Model.Views {
    public class TeamView(TeamRow teamRow) : IEquatable<TeamView> {
        public TeamRow TeamRow { get; } = teamRow;

        public List<MatchRow> Matches {
            get {
                List<MatchRow> matches = [];

                foreach (TeamRow teamRow in teamRow.Event.Teams) {
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
            else if (@object is TeamView otherTeam) {
                if (otherTeam.Players.Count != this.Players.Count) return false;
                foreach (string player in this.Players) {
                    if (!otherTeam.Players.Contains(player)) return false;
                }
            }
            else if (@object is TeamRow otherTeamRow) {
                if (otherTeamRow.Members.Count != this.Players.Count) return false;
                foreach (string player in this.Players) {
                    if (!otherTeamRow.Members.Has(player)) return false;
                }
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
