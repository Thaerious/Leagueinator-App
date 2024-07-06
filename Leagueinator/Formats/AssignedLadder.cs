﻿using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Scoring.Plus;
using System.Collections.ObjectModel;

namespace Leagueinator.Formats {

    public class AssignedLadder : TournamentFormat {
        private readonly static Random random = new();
        private PlayersPreviousLanes previousLanes = [];

        public RoundRow GenerateNextRound(EventRow eventRow) {
            this.previousLanes = new(eventRow);

            RoundRow roundRow = eventRow.Rounds.Add();

            List<PlusSummary> sortedResults = WTLResultsBuilder.GetResults(eventRow);
            AssignMatches(eventRow, roundRow, sortedResults);
            new LaneAssigner(roundRow).AssignLanes();

            return roundRow;
        }

        private static void AssignMatches(EventRow eventRow, RoundRow roundRow, List<PlusSummary> sortedResults) {
            roundRow.PopulateMatches();

            int match = 0;
            int team = 0;

            // Assign Matches
            foreach (PlusSummary result in sortedResults) {
                int maxTeams = roundRow.Matches[match]!.MatchFormat.TeamCount();

                MatchRow matchRow = roundRow.Matches[match]!;
                while (matchRow.Teams.Count < maxTeams) {
                    matchRow.Teams.Add(matchRow.Teams.Count);
                }

                foreach (string player in result.TeamView.Players) {
                    matchRow.Teams[team]!.Members.Add(player);
                }
                team++;

                if (team >= maxTeams) {
                    match++;
                    team = 0;
                }
            }
        }
    }

    internal class MatchLaneList {
        public MatchRow MatchRow;
        public List<int> lanes = [];

        public MatchLaneList(MatchRow matchRow) {
            this.MatchRow = matchRow;
        }
    }

    internal class PlayersPreviousLanes : Dictionary<string, HashSet<int>> {

        public PlayersPreviousLanes() { }

        public PlayersPreviousLanes(EventRow eventRow) {
            foreach (RoundRow roundRow in eventRow.Rounds) {
                foreach (MatchRow matchRow in roundRow.Matches) {
                    foreach (TeamRow teamRow in matchRow.Teams) {
                        foreach (MemberRow memberRow in teamRow.Members) {
                            if (!this.ContainsKey(memberRow.Player)) this[memberRow.Player] = [];
                            this[memberRow.Player].Add(matchRow.Lane);
                        }
                    }
                }
            }
        }
    }
}
