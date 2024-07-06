using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Leagueinator.Scoring.Plus {
    public class WTLResultsBuilder {

        /// <summary>
        /// Build a results summary for each team in the event.
        /// The list is sorted by highest scoring team first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventRow"></param>
        /// <returns>A new non-reflective list of result summaries</returns>
        public static List<PlusSummary> GetResults(EventRow eventRow) {
            List<PlusSummary> results = [];

            foreach (TeamView teamView in eventRow.TeamViews()) {
                results.Add(new PlusSummary(teamView));
            }

            results.Sort();

            return results;
        }
    }
}
