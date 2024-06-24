using Leagueinator.Model.Tables;
using System.Diagnostics;
using Leagueinator.Utility;
using Model.Source.Tables.Event;

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

        public LaneAssigner(EventRow eventRow, RoundRow roundRow) {
            EventRow = eventRow;
            RoundRow = roundRow;
        }

        internal void AssignLanes() {
            PopulateUnassignedMatches();
            PopulateFreeLanes();
            PopulateAvailableLanes();
            PopulatePreviousLanes();
            RemovePlayedLanes();
            foreach (MatchRow match in AvailableLanes.Keys) {
                Debug.WriteLine($"[{match.Players.DelString()}] - [{AvailableLanes[match].DelString()}]");
            }
            AssignLanesToMatches();
            foreach (MatchRow match in AvailableLanes.Keys) {
                Debug.WriteLine($"[{match.Players.DelString()}] => {match.Lane}");
            }
            AssignLanesToDeferredMatches();
        }

        /// <summary>
        /// Fill the unassigned matches list will each match.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void PopulateUnassignedMatches() {
            foreach (MatchRow matchRow in RoundRow.Matches) {
                this.UnassignedMatches.Add(matchRow);
            }
        }

        /// <summary>
        /// Fill the free lanes list with all available lanes.
        /// ie [0, 1, 2, 3, 4, 5, 6, 7]
        /// </summary>
        private void PopulateFreeLanes() {
            for (int i = 0; i < EventRow.LaneCount; i++) {
                FreeLanes.Add(i);
            }
        }

        /// <summary>
        /// Add a list of lanes to available lanes for each match.
        /// This list will have an entry for each possible lane.
        /// ie (Adam, Eve) => [0, 1, 2, 3, 4, 5, 6, 7]
        /// </summary>
        private void PopulateAvailableLanes() {
            foreach (MatchRow matchRow in RoundRow.Matches) {
                if (matchRow.Players.Count <= 0) continue;
                AvailableLanes[matchRow] = [];

                for (int i = 0; i < EventRow.LaneCount; i++) {
                    AvailableLanes[matchRow].Add(i);
                }
            }
        }

        /// <summary>
        /// Add a list of lanes that each player has already played on.
        /// ie (Adam) => [2, 4];
        /// </summary>
        private void PopulatePreviousLanes() {
            foreach (RoundRow roundRow in EventRow.Rounds) {
                if (roundRow == RoundRow) continue;
                foreach (MatchRow matchRow in roundRow.Matches) {
                    foreach (TeamRow teamRow in matchRow.Teams) {
                        foreach (MemberRow memberRow in teamRow.Members) {
                            string player = memberRow.Player;
                            if (!PreviousLanes.ContainsKey(player)) PreviousLanes[player] = [];
                            PreviousLanes[player].Add(matchRow.Lane);
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
            foreach (MatchRow matchRow in AvailableLanes.Keys) {
                List<int> lanes = AvailableLanes[matchRow];
                foreach (TeamRow teamRow in matchRow.Teams) {
                    foreach (MemberRow memberRow in teamRow.Members) {
                        foreach (int lane in PreviousLanes[memberRow.Player]) {
                            lanes.Remove(lane);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Assign a late to each match from the matches available lane list.
        /// If the match has no available lanes, defer it.
        /// Remove this lane from the free lanes list.
        /// Remove this lane from the available lanes of each match.
        /// </summary>
        private void AssignLanesToMatches() {
            foreach (MatchRow matchRow in AvailableLanes.Keys) {
                if (AvailableLanes[matchRow].Count == 0) {
                    UnassignedMatches.Add(matchRow);
                }
                else {
                    List<int> potentialLanes = [.. AvailableLanes[matchRow]];
                    int randomIndex = Random.Next(potentialLanes.Count);
                    int lane = potentialLanes[randomIndex];
                    matchRow.Lane = lane;
                    this.UnassignedMatches.Remove(matchRow);
                    FreeLanes.Remove(lane);

                    foreach (List<int> lanes in AvailableLanes.Values) {
                        lanes.Remove(lane);
                    }
                }
            }
        }

        private void AssignLanesToDeferredMatches() {
            foreach (MatchRow matchRow in UnassignedMatches) {
                if (FreeLanes.Count == 0) throw new NotSupportedException();
                int randomIndex = Random.Next(FreeLanes.Count);
                int lane = FreeLanes[randomIndex];
                matchRow.Lane = lane;
                FreeLanes.Remove(lane);
            }
        }
    }
}
