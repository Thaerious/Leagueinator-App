using Leagueinator.Model.Views;

namespace Leagueinator.Scoring {
    /// <summary>
    /// The summary and match results for a single team.
    /// </summary>
    /// <typeparam name="MATCH_TYPE">The data type for a single match result.</typeparam>
    public interface ISummary<MATCH_TYPE, SELF> : IComparable<SELF> where MATCH_TYPE : IMatchResult<MATCH_TYPE> {
        public TeamView TeamView { get; }
        public List<MATCH_TYPE> MatchResults();
    }
}
