using Antlr4.Runtime.Misc;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Leagueinator.Forms {
    internal class ResultBuilder {

        /// <summary>
        /// Build a XML element with a results summary organized by team.
        /// </summary>
        /// <param name="eventRow"></param>
        /// <returns></returns>
        public static Element BuildElement(EventRow eventRow) {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.EventScoreForm.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.EventScoreForm.xml");
            Element teamXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.TeamScore.xml");
            Element rowXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.ScoreRow.xml");

            var matchResults = eventRow.MatchResults();
            var matchSummaries = eventRow.MatchSummaries();

            for (int i = 0; i < matchSummaries.Count; i++){
                MatchSummary summary = matchSummaries[i];
                Element xmlFragment = teamXML.Clone();
                Team team = summary.Team;
                IReadOnlyList<MatchResults> results = matchResults[team];

                xmlFragment["placement"][0].InnerText = $"{i + 1}";

                // Add names to the xml fragment.
                foreach (string name in team.Players) {
                    xmlFragment["names"][0].AddChild(
                        new Element {
                            TagName = "Name",
                            InnerText = name
                        }
                    );
                }

                // Add match summaries to the xml fragment.
                foreach (MatchResults match in results) {
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
                sumRow["ends_played"][0].InnerText = summary.Ends.ToString();
                sumRow["bowls_for"][0].InnerText = summary.BowlsFor.ToString();
                sumRow["bowls_against"][0].InnerText = summary.BowlsAgainst.ToString();
                sumRow["tie"][0].InnerText = "";
                sumRow["result"][0].InnerText = summary.Wins.ToString();
                sumRow["score_for"][0].InnerText = $"{summary.PointsFor}+{summary.PlusFor}";
                sumRow["score_against"][0].InnerText = $"{summary.PointsAgainst}+{summary.PlusAgainst}";

                xmlFragment["rounds"][0].AddChild(sumRow);
                docroot["page"][0].AddChild(xmlFragment);
            }

            styles.AssignTo(docroot);
            return docroot;
        }
    }
}
