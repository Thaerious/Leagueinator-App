using Leagueinator.Model.Tables;

namespace Leagueinator.Formats {
    public interface TournamentFormat {
        public RoundRow GenerateNextRound(EventRow eventRow);
    }
}
