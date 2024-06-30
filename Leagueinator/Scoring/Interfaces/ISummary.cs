using Leagueinator.Model.Views;

namespace Leagueinator.Scoring {
    /// <summary>
    /// The summary and match results for a single team.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISummary<T, SELF> : IComparable<SELF> where T : IMatchResult<T> {
        public TeamView TeamView { get; }
        public List<T> MatchResults();
    }
}
