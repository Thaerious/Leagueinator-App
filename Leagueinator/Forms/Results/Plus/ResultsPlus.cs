using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using System.Collections.ObjectModel;

namespace Leagueinator.Forms.Results.Plus {

    /// <summary>
    /// The scoring results for a single team.  
    /// Includes each match (Matches[index]) and a summery result (Summary).
    /// When sorted, will sort by the summary result.
    /// </summary>
    public class ResultsPlus (Team team) : IComparable<ResultsPlus> {
        public readonly Team Team = team;

        public IReadOnlyList<MatchResultsPlus> MatchResults { get => this._matchResults; }

        public SummaryResultsPlus Summary { get; private set; }

        /// <summary>
        /// Retrieve a dictionary of each team that has a unique set of players.
        /// The key is the team, the value is a list of match results.
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyDictionary<Team, ResultsPlus> AllResults(EventRow eventRow) {
            Dictionary<Team, ResultsPlus> allTeams = [];
            List<TeamRow> allTeamsInEvent = eventRow.Rounds.SelectMany(matchRow => matchRow.Teams).ToList();

            foreach (TeamRow teamRow in allTeamsInEvent) {
                Team team = new(teamRow);
                if (team.Players.Count <= 0) continue;
                if (!allTeams.ContainsKey(team)) allTeams[team] = new(team);
                allTeams[team]._matchResults.Add(new(teamRow));
            }

            foreach (Team team in allTeams.Keys) {
                allTeams[team].Summary = new SummaryResultsPlus(team, allTeams[team].MatchResults);
            }

            return new ReadOnlyDictionary<Team, ResultsPlus>(allTeams);
        }

        public int CompareTo(ResultsPlus? that) {
            if (that is null) return 1;
            return this.Summary.CompareTo(that.Summary);            
        }

        private readonly List<MatchResultsPlus> _matchResults = [];
    }
}
