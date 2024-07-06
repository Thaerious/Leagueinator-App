
using Leagueinator.Scoring.Plus;

namespace Leagueinator.Scoring {
    public static class XMLBuilderFactory {
        public static IEventXMLBuilder ToXMLBuilder(this ScoringFormat format) {
            switch (format) {
                case ScoringFormat.POINTS_PLUS:
                    return new PlusXMLBuilder();
                case ScoringFormat.WTL:
                    return new WTLXMLBuilder();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
