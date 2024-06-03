using Leagueinator.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Controls.MatchCards {
    public static class MatchCardFactory {
        public static MatchCard GenerateMatchCard(MatchRow matchRow) {
            switch (matchRow.MatchType) {
                case MatchType.VS1:
                    throw new NotImplementedException();
                case MatchType.VS2:
                    throw new NotImplementedException();
                case MatchType.VS3:
                    throw new NotImplementedException();
                case MatchType.VS4:
                    return new MatchCardV4() { MatchRow = matchRow };
                case MatchType.A4321:
                    return new MatchCard4321() { MatchRow = matchRow };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
