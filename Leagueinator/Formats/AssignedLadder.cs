using Leagueinator.Forms.Results.Plus;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Utility;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Leagueinator.Formats {

    [TimeTrace]
    public class AssignedLadder : TournamentFormat {
        private readonly static Random random = new();
        private PlayersPreviousLanes previousLanes = [];

        public RoundRow GenerateNextRound(EventRow eventRow) {
            this.previousLanes = new(eventRow);

            RoundRow roundRow = eventRow.Rounds.Add();
            ReadOnlyDictionary<Team, ResultsPlus> results = ResultsPlus.AllResults(eventRow);

            List<ResultsPlus> sortedResults = [.. results.Values];
            sortedResults.Sort();
            AssignMatches(eventRow, roundRow, sortedResults);
            new LaneAssigner(eventRow, roundRow).AssignLanes();

            Debug.WriteLine(roundRow.League.MatchTable.PrettyPrint());
            Debug.WriteLine(roundRow.League.TeamTable.PrettyPrint());
            Debug.WriteLine(roundRow.League.MemberTable.PrettyPrint());

            return roundRow;
        }

        private static void AssignMatches(EventRow eventRow, RoundRow roundRow, List<ResultsPlus> sortedResults) {
            roundRow.PopulateMatches();

            int match = 0;
            int team = 0;

            // Assign Matches
            foreach (ResultsPlus result in sortedResults) {
                int maxTeams = Match.TeamCount(roundRow.Matches[match]!.MatchFormat);

                MatchRow matchRow = roundRow.Matches[match]!;
                while (matchRow.Teams.Count < maxTeams) {
                    matchRow.Teams.Add(matchRow.Teams.Count);
                }

                foreach (string player in result.Team.Players) {
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
