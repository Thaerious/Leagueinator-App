using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace Leagueinator.Formats {
    internal class LaneAssigner {
        private readonly Random Random = new();
        private readonly EventRow EventRow;
        private readonly RoundRow RoundRow;

        // The lanes not assigned to a match
        private readonly List<int> FreeLanes = [];

        // The lanes that a match hasn't played on (only lanes with players are included).
        private readonly Dictionary<MatchRow, List<int>> AvailableLanes = [];

        // The previous lanes a player has played on
        private readonly Dictionary<string, HashSet<int>> PreviousLanes = [];

        // Matches that have not been assigned a lane
        private readonly List<MatchRow> UnassignedMatches = [];

        /// <summary>
        /// </summary>
        /// <param name="roundRow">The round the lanes are being assigned for</param>
        public LaneAssigner(RoundRow roundRow) {
            this.EventRow = roundRow.Event;
            this.RoundRow = roundRow;
        }

        internal void AssignLanes() {
            this.RoundRow.League.EnforceConstraints = false;

            this.PopulateUnassignedMatches();
            this.PopulateFreeLanes();
            this.PopulateAvailableLanes();
            this.PopulatePreviousLanes();
            this.RemovePlayedLanes();
            this.AssignLanesToMatches();
            this.AssignLanesToDeferredMatches();

            this.RoundRow.League.EnforceConstraints = true;
        }

        /// <summary>
        /// Fill the unassigned matches list with each matches from the target round.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void PopulateUnassignedMatches() {
            foreach (MatchRow matchRow in this.RoundRow.Matches) {
                this.UnassignedMatches.Add(matchRow);
            }
        }

        /// <summary>
        /// Fill the free lanes list with all available lanes.
        /// ie [0, 1, 2, 3, 4, 5, 6, 7]
        /// </summary>
        private void PopulateFreeLanes() {
            for (int i = 0; i < this.EventRow.LaneCount; i++) {
                this.FreeLanes.Add(i);
            }
        }

        /// <summary>
        /// Set a list of lanes to available lanes for each match.
        /// This list will have an entry for each possible lane.
        /// ie (Adam, Eve) => [0, 1, 2, 3, 4, 5, 6, 7]
        /// </summary>
        private void PopulateAvailableLanes() {
            foreach (MatchRow matchRow in this.RoundRow.Matches) {
                if (matchRow.Members.ToList().Count <= 0) continue;
                this.AvailableLanes[matchRow] = [];

                for (int i = 0; i < this.EventRow.LaneCount; i++) {
                    this.AvailableLanes[matchRow].Add(i);
                }
            }
        }

        /// <summary>
        /// Set a list of lanes that each player has already played on.
        /// ie (Adam) => [2, 4];
        /// </summary>
        private void PopulatePreviousLanes() {
            foreach (RoundRow roundRow in this.EventRow.Rounds) {
                if (roundRow == this.RoundRow) continue;
                foreach (MatchRow matchRow in roundRow.Matches) {
                    foreach (TeamRow teamRow in matchRow.Teams) {
                        foreach (MemberRow memberRow in teamRow.Members) {
                            string player = memberRow.Player;
                            if (!this.PreviousLanes.ContainsKey(player)) this.PreviousLanes[player] = [];
                            this.PreviousLanes[player].Add(matchRow.Lane);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// For each player in a match, remove that players previous lanes from the 
        /// match's available lanes;
        /// </summary>
        private void RemovePlayedLanes() {
            foreach (MatchRow matchRow in this.AvailableLanes.Keys) {
                List<int> lanes = this.AvailableLanes[matchRow];
                foreach (TeamRow teamRow in matchRow.Teams) {
                    foreach (MemberRow memberRow in teamRow.Members) {
                        foreach (int lane in this.PreviousLanes[memberRow.Player]) {
                            lanes.Remove(lane);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Assign a lane to each match from the matches available lane list.
        /// If the match has no available lanes, defer it.
        /// Remove this lane from the free lanes list.
        /// Remove this lane from the available lanes of each match.
        /// </summary>
        private void AssignLanesToMatches() {
            foreach (MatchRow matchRow in this.AvailableLanes.Keys) {
                if (this.AvailableLanes[matchRow].Count == 0) {
                    this.UnassignedMatches.Add(matchRow);
                }
                else {
                    List<int> potentialLanes = [.. this.AvailableLanes[matchRow]];
                    int randomIndex = this.Random.Next(potentialLanes.Count);
                    int lane = potentialLanes[randomIndex];
                    matchRow.Lane = lane;
                    this.UnassignedMatches.Remove(matchRow);
                    this.FreeLanes.Remove(lane);

                    foreach (List<int> lanes in this.AvailableLanes.Values) {
                        lanes.Remove(lane);
                    }
                }
            }
        }

        private void AssignLanesToDeferredMatches() {
            foreach (MatchRow matchRow in this.UnassignedMatches) {
                if (this.FreeLanes.Count == 0) {
                    throw new NotSupportedException("Not Enough Lanes to Assign to All Matches");
                }

                int randomIndex = this.Random.Next(this.FreeLanes.Count);
                int lane = this.FreeLanes[randomIndex];
                matchRow.Lane = lane;
                this.FreeLanes.Remove(lane);
            }
        }
    }
}
