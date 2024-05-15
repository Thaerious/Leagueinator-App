using Leagueinator.Forms.Results.Plus;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Utility;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Leagueinator.Formats {
    public class AssignedLadder : TournamentFormat {
        private static Random random = new();

        public RoundRow GenerateNextRound(EventRow eventRow) {
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches();
            ReadOnlyDictionary<Team, ResultsPlus> results = ResultsPlus.AllResults(eventRow);

            List<ResultsPlus> sortedResults = [.. results.Values];
            sortedResults.Sort();
            AssignMatches(eventRow, roundRow, sortedResults);
            AssignLanes(eventRow, roundRow, results);

            return roundRow;
        }

        private static void AssignMatches(EventRow eventRow, RoundRow roundRow, List<ResultsPlus> sortedResults) {
            int match = 0;
            int team = 0;

            // Assign Matches
            foreach (ResultsPlus result in sortedResults) {
                foreach (string player in result.Team.Players) {
                    roundRow.Matches[match]!.Teams[team]!.Members.Add(player);
                }
                team++;

                int maxTeams = int.Parse(eventRow.Settings.Get("teams").Value);
                if (team >= maxTeams) {
                    match++;
                    team = 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventRow">The event in question</param>
        /// <param name="roundRow">The new round being added</param>
        /// <param name="results"></param>

        private static void AssignLanes(EventRow eventRow, RoundRow roundRow, ReadOnlyDictionary<Team, ResultsPlus> results) {
            // Random Assignment
            int sanity = 20;
            while (sanity-- > 0) {
                RandomAssignment(eventRow, roundRow);
                if (CountRepeats(roundRow) == 0) break;
            }
        }


        private static void RandomAssignment(EventRow eventRow, RoundRow roundRow) {
            // Create a list of available lanes.
            List<int> availableLanes = [];
            for (int i = 0; i < int.Parse(eventRow.Settings.Get("lanes").Value); i++) {
                availableLanes.Add(i);
            }

            foreach (MatchRow matchRow in roundRow.Matches) {
                int randomLane = availableLanes[random.Next(availableLanes.Count)];
                availableLanes.Remove(randomLane);
                matchRow.Lane = randomLane;
            }
        }

        private static int CountRepeats(RoundRow roundRow) {
            int repeats = 0;
            Dictionary<MatchRow, List<int>> prevLanes = [];

            // Create a list of previous lanes for each match
            foreach (MatchRow matchRow in roundRow.Matches) {
                prevLanes[matchRow] = [];
                foreach (TeamRow teamRow in matchRow.Teams) {
                    Team team = new Team(teamRow);
                    prevLanes[matchRow].AddRange(team.PrevLanes(roundRow));
                }
            }

            // Count repeats
            foreach (MatchRow matchRow in prevLanes.Keys) {
                if (prevLanes[matchRow].Contains(matchRow.Lane)) repeats++;
            }

            return repeats;
        }

        public static List<string> Players(MatchRow matchRow) {
            List<string> list = [];

            foreach (TeamRow teamRow in matchRow.Teams) {
                foreach (MemberRow memberRow in teamRow.Members) {
                    if (memberRow.Player != null && memberRow.Player != "") {
                        list.Add(memberRow.Player);
                    }
                }
            }

            return list;
        }
    }
}
