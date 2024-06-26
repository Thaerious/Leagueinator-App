using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.Controls.MatchCards {
    [TimeTrace]
    public static class MatchCardFactory {
        public static MatchCard GenerateMatchCard(MatchRow matchRow) {
            switch (matchRow.MatchFormat) {
                case MatchFormat.VS1:
                    throw new NotImplementedException();
                case MatchFormat.VS2:
                    throw new NotImplementedException();
                case MatchFormat.VS3:
                    throw new NotImplementedException();
                case MatchFormat.VS4:
                    return new MatchCardV4() { MatchRow = matchRow };
                case MatchFormat.A4321:
                    return new MatchCard4321() { MatchRow = matchRow };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
