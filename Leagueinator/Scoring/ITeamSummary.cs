using Leagueinator.Model.Views;

namespace Leagueinator.Scoring {
    public interface ITeamSummary : IComparable<ITeamSummary> {
        public Team Team { get; }
    }
}
