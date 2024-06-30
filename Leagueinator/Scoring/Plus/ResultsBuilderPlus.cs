using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Leagueinator.Scoring.Plus {
    public class ResultsBuilderPlus {

        /// <summary>
        /// Build a results summary for each team in the event.
        /// The list is sorted by highest scoring team first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventRow"></param>
        /// <returns>A new non-reflective list of result summaries</returns>
        public static List<SummaryPlus> GetResults(EventRow eventRow) {
            List<SummaryPlus> results = [];

            foreach (TeamView teamView in eventRow.TeamViews()) {
                results.Add(new SummaryPlus(teamView));
            }

            results.Sort();

            return results;
        }
    }
}
