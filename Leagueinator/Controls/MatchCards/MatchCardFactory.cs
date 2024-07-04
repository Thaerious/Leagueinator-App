using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.Controls.MatchCards {
    public static class MatchCardFactory {
        public static MatchCard GenerateMatchCard(MatchRow matchRow) {
            switch (matchRow.MatchFormat) {
                case MatchFormat.VS1:
                    return new MatchCardV1() { MatchRow = matchRow };
                case MatchFormat.VS2:
                    return new MatchCardV2() { MatchRow = matchRow };
                case MatchFormat.VS3:
                    return new MatchCardV3() { MatchRow = matchRow };
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
