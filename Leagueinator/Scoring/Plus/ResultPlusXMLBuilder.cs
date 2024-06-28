using Leagueinator.Forms.Results.Plus;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Leagueinator.Forms {
    internal class ResultPlusXMLBuilder(EventRow eventRow) : IXMLBuilder {
        private readonly EventRow EventRow = eventRow;

        /// <resultsPlus>
        /// Build a XML element with a results resultsPlus organized by team.
        /// </resultsPlus>
        /// <param name="this.EventRow"></param>
        /// <returns></returns>
        public Element BuildElement() {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.TeamStandings.EventScoreForm.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.TeamStandings.EventScoreForm.xml");
            Element teamXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.TeamStandings.TeamScore.xml");
            Element rowXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.TeamStandings.ScoreRow.xml");

            ReadOnlyDictionary<Team, ResultsPlus> resultsPlus = ResultsPlus.AllResults(this.EventRow);
            List<ResultsPlus> sortedResults = [.. resultsPlus.Values];
            sortedResults.Sort();

            for (int i = 0; i < sortedResults.Count; i++) {
                ResultsPlus currentResult = sortedResults[i];
                Element xmlFragment = teamXML.Clone();

                xmlFragment["placement"][0].InnerText = $"{i + 1}";

                // Set names to the xml fragment.
                foreach (string name in currentResult.Team.Players) {
                    xmlFragment["names"][0].AddChild(
                        new Element {
                            TagName = "Name",
                            InnerText = name
                        }
                    );
                }

                // Set match sortedResults to the xml fragment.
                foreach (MatchResultsPlus match in currentResult.MatchResults) {
                    Element row = rowXML.Clone();

                    row["index"][0].InnerText = (match.Round + 1).ToString();
                    row["lane"][0].InnerText = (match.Lane + 1).ToString();
                    row["result"][0].InnerText = $"{match.Result().ToString().ToCharArray()[0]}";
                    row["bowls_for"][0].InnerText = match.BowlsFor.ToString();
                    row["bowls_against"][0].InnerText = match.BowlsAgainst.ToString();
                    row["tie"][0].InnerText = match.TieBreaker.ToString();
                    row["score_for"][0].InnerText = $"{match.PointsFor}+{match.PlusFor}";
                    row["score_against"][0].InnerText = $"{match.PointsAgainst}+{match.PlusAgainst}";
                    row["ends_played"][0].InnerText = match.Ends.ToString();

                    xmlFragment["rounds"][0].AddChild(row);
                }

                Element sumRow = rowXML.Clone();
                sumRow["index"][0].InnerText = "Σ";
                sumRow["lane"][0].InnerText = "";
                sumRow["ends_played"][0].InnerText = currentResult.Summary.Ends.ToString();
                sumRow["bowls_for"][0].InnerText = currentResult.Summary.BowlsFor.ToString();
                sumRow["bowls_against"][0].InnerText = currentResult.Summary.BowlsAgainst.ToString();
                sumRow["tie"][0].InnerText = "";
                sumRow["result"][0].InnerText = currentResult.Summary.Wins.ToString();
                sumRow["score_for"][0].InnerText = $"{currentResult.Summary.PointsFor}+{currentResult.Summary.PlusFor}";
                sumRow["score_against"][0].InnerText = $"{currentResult.Summary.PointsAgainst}+{currentResult.Summary.PlusAgainst}";

                xmlFragment["rounds"][0].AddChild(sumRow);
                docroot["page"][0].AddChild(xmlFragment);
            }

            styles.AssignTo(docroot);
            return docroot;
        }
    }
}
