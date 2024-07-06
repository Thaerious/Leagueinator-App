using Leagueinator.Model.Tables;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer;
using System.Reflection;

namespace Leagueinator.Scoring.Plus {

    internal class WTLXMLBuilder : IEventXMLBuilder {
        /// <resultsPlus>
        /// Build a XML element with a results resultsPlus organized by team.
        /// </resultsPlus>
        /// <param name="eventRow"></param>
        /// <returns></returns>
        public Element BuildElement(EventRow eventRow) {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.EventScoreForm.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.EventScoreForm.xml");
            Element teamXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.WTL.TeamScore.xml");
            Element rowXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.WTL.ScoreRow.xml");

            List<PlusSummary> summaryPlus = WTLResultsBuilder.GetResults(eventRow);

            for (int i = 0; i < summaryPlus.Count; i++) {
                PlusSummary currentResult = summaryPlus[i];
                Element xmlFragment = teamXML.Clone();

                xmlFragment["placement"][0].InnerText = $"{i + 1}";

                // Set names to the xml fragment.
                foreach (string name in currentResult.TeamView.Players) {
                    xmlFragment["names"][0].AddChild(
                        new Element {
                            TagName = "Name",
                            InnerText = name
                        }
                    );
                }

                // Set match sortedResults to the xml fragment.
                foreach (PlusMatchResults match in currentResult.MatchResults()) {
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
                sumRow["ends_played"][0].InnerText = currentResult.Ends.ToString();
                sumRow["bowls_for"][0].InnerText = currentResult.BowlsFor.ToString();
                sumRow["bowls_against"][0].InnerText = currentResult.BowlsAgainst.ToString();
                sumRow["tie"][0].InnerText = "";
                sumRow["result"][0].InnerText = currentResult.Wins.ToString();
                sumRow["score_for"][0].InnerText = $"{currentResult.PointsFor}+{currentResult.PlusFor}";
                sumRow["score_against"][0].InnerText = $"{currentResult.PointsAgainst}+{currentResult.PlusAgainst}";

                xmlFragment["rounds"][0].AddChild(sumRow);
                docroot["page"][0].AddChild(xmlFragment);
            }

            styles.AssignTo(docroot);
            return docroot;
        }
    }
}
