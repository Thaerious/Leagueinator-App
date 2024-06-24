using Leagueinator.Model.Tables;
using Model.Source.Tables.Event;

namespace Leagueinator.Formats {
    public interface TournamentFormat {
        public RoundRow GenerateNextRound(EventRow eventRow);
    }
}
